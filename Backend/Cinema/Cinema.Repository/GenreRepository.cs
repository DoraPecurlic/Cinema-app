using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;

namespace Cinema.Repository;

public class GenreRepository : IGenreRepository 
{
    private readonly string _connectionString;
    public GenreRepository(string connectionString)
    {
        this._connectionString = connectionString;
    }
    
    public async Task<Genre> AddGenreAsync(Genre genre)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var commandText = "INSERT INTO \"Genre\" (\"Id\", \"Name\") VALUES (@Id, @Name);";

        await using var command = new NpgsqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", genre.Id);
        command.Parameters.AddWithValue("@Name", genre.Name);

        await command.ExecuteNonQueryAsync();

        return genre;
    }
    
    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        var genres = new List<Genre>();

        await using var connection = new NpgsqlConnection(_connectionString);
        var commandText = "SELECT * FROM \"Genre\";";

        await using var command = new NpgsqlCommand(commandText, connection);
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var genre = new Genre
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                
            };
            genres.Add(genre);
        }

        return genres;
    }
}