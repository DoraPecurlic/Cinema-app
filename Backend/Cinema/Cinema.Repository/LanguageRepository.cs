using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;

namespace Cinema.Repository;

public class LanguageRepository : ILanguageRepository 
{
    private readonly string _connectionString;
    public LanguageRepository(string connectionString)
    {
        this._connectionString = connectionString;
    }
    
    public async Task<Language> AddLanguageAsync(Language language)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var commandText = "INSERT INTO \"Language\" (\"Id\", \"Name\") VALUES (@Id, @Name);";

        await using var command = new NpgsqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", language.Id);
        command.Parameters.AddWithValue("@Name", language.Name);

        await command.ExecuteNonQueryAsync();

        return language;
    }
    
    public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
    {
        var languages = new List<Language>();

        await using var connection = new NpgsqlConnection(_connectionString);
        var commandText = "SELECT * FROM \"Language\";";

        await using var command = new NpgsqlCommand(commandText, connection);
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var language = new Language
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                
            };
            languages.Add(language);
        }

        return languages;
    }
}