using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MovieModel;
using Cinema.Common;
using System.Text;

namespace Cinema.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            try
            {
                var addMovieCommandText = @"
                    INSERT INTO ""Movie"" 
                    (""Id"", ""Title"", ""Description"", ""Duration"", ""LanguageId"", 
                     ""CoverUrl"", ""TrailerUrl"", ""AdminId"", ""GenreId"", ""IsActive"", ""DateCreated"", ""DateUpdated"", 
                     ""CreatedByUserId"", ""UpdatedByUserId"")
                    VALUES 
                    (@Id, @Title, @Description, @Duration, @LanguageId, 
                     @CoverUrl, @TrailerUrl, @AdminId, @GenreId, @IsActive, @DateCreated, @DateUpdated, 
                     @CreatedByUserId, @UpdatedByUserId)";

                await using var addMovieCommand = new NpgsqlCommand(addMovieCommandText, connection);
                addMovieCommand.Parameters.AddWithValue("@Id", movie.Id);
                addMovieCommand.Parameters.AddWithValue("@Title", movie.Title);
                addMovieCommand.Parameters.AddWithValue("@Description", movie.Description);
                addMovieCommand.Parameters.AddWithValue("@Duration", movie.Duration);
                addMovieCommand.Parameters.AddWithValue("@LanguageId", movie.LanguageId);
                addMovieCommand.Parameters.AddWithValue("@CoverUrl", movie.CoverUrl);
                addMovieCommand.Parameters.AddWithValue("@TrailerUrl", movie.TrailerUrl);
                addMovieCommand.Parameters.AddWithValue("@AdminId", movie.CreatedByUserId);
                addMovieCommand.Parameters.AddWithValue("@GenreId", movie.GenreId);
                addMovieCommand.Parameters.AddWithValue("@IsActive", movie.IsActive);
                addMovieCommand.Parameters.AddWithValue("@DateCreated", movie.DateCreated);
                addMovieCommand.Parameters.AddWithValue("@DateUpdated", movie.DateUpdated);
                addMovieCommand.Parameters.AddWithValue("@CreatedByUserId", movie.CreatedByUserId);
                addMovieCommand.Parameters.AddWithValue("@UpdatedByUserId", movie.UpdatedByUserId);

                await addMovieCommand.ExecuteNonQueryAsync();

                foreach (var actorId in movie.ActorId)
                {
                    var addMovieActorCommandText = @"
                        INSERT INTO ""MovieActor"" 
                        (""MovieId"", ""ActorId"")
                        VALUES 
                        (@MovieId, @ActorId)";

                    await using var addMovieActorCommand = new NpgsqlCommand(addMovieActorCommandText, connection);
                    addMovieActorCommand.Parameters.AddWithValue("@MovieId", movie.Id);
                    addMovieActorCommand.Parameters.AddWithValue("@ActorId", actorId);

                    await addMovieActorCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding movie", ex);
            }
        }

        public async Task AddActorToMovieAsync(Guid movieId, Guid actorId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                var addActorToMovieCommandText = @"
                                        INSERT INTO ""MovieActor"" 
                                        (""MovieId"", ""ActorId"")
                                        VALUES 
                                        (@MovieId, @ActorId)";

                using var addActorToMovieCommand = new NpgsqlCommand(addActorToMovieCommandText, connection);
                addActorToMovieCommand.Parameters.AddWithValue("@MovieId", movieId);
                addActorToMovieCommand.Parameters.AddWithValue("@ActorId", actorId);

                await addActorToMovieCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Error adding actor to movie", ex);
            }
        }

        public async Task<MovieGet> GetMovieByIdAsync(Guid id)
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"
                            SELECT 
                                m.*, 
                                string_agg(a.""Name"", ', ') AS ""ActorNames"",
                                g.""Name"" AS ""Genre"",
                                l.""Name"" AS ""Language""
                            FROM ""Movie"" m
                            LEFT JOIN ""MovieActor"" ma ON m.""Id"" = ma.""MovieId""
                            LEFT JOIN ""Actor"" a ON ma.""ActorId"" = a.""Id""
                            LEFT JOIN ""Genre"" g ON m.""GenreId"" = g.""Id""
                            LEFT JOIN ""Language"" l ON m.""LanguageId"" = l.""Id""
                            WHERE m.""Id"" = @Id
                            GROUP BY m.""Id"", g.""Name"", l.""Name""";

            var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("Id", id);

            var reader = await command.ExecuteReaderAsync();

            MovieGet movie = null;

            if (await reader.ReadAsync())
            {
                movie = new MovieGet
                {
                    MovieId = reader.GetGuid(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Genre = reader.GetString(reader.GetOrdinal("Genre")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                    Language = reader.GetString(reader.GetOrdinal("Language")),
                    CoverUrl = reader.GetString(reader.GetOrdinal("CoverUrl")),
                    TrailerUrl = reader.GetString(reader.GetOrdinal("TrailerUrl")),
                    ActorNames = reader.IsDBNull(reader.GetOrdinal("ActorNames")) ? new List<string>() : reader.GetString(reader.GetOrdinal("ActorNames")).Split(", ").ToList(),
                };
            }

            connection.Close();

            return movie;
        }

        public async Task<IEnumerable<MovieGet>> GetFilteredMoviesAsync(MovieFiltering filtering, MovieSorting sorting, MoviePaging paging)
        {
            var queryBuilder = new StringBuilder(@"SELECT m.*, string_agg(a.""Name"", ', ') AS ""ActorNames"", g.""Name"" AS ""Genre"", l.""Name"" AS ""Language""
                                      FROM ""Movie"" m
                                      LEFT JOIN ""MovieActor"" ma ON m.""Id"" = ma.""MovieId""
                                      LEFT JOIN ""Actor"" a ON ma.""ActorId"" = a.""Id""
                                      LEFT JOIN ""Genre"" g ON m.""GenreId"" = g.""Id""
                                      LEFT JOIN ""Language"" l ON m.""LanguageId"" = l.""Id""
                                      WHERE 1 = 1");

            if (filtering.MovieId != null)
            {
                queryBuilder.Append(" AND m.\"Id\" = @MovieId");
            }

            if (filtering.GenreId != null)
            {
                queryBuilder.Append(" AND m.\"GenreId\" = @GenreId");
            }

            if (filtering.LanguageId != null)
            {
                queryBuilder.Append(" AND m.\"LanguageId\" = @LanguageId");
            }
            
            if (!string.IsNullOrEmpty(filtering.SearchTerm))
            {
                queryBuilder.Append(" AND m.\"Title\" ILIKE '%' || @SearchTerm || '%'");
            }

            queryBuilder.Append(" GROUP BY m.\"Id\", m.\"Title\", m.\"Description\", m.\"Duration\", m.\"LanguageId\", m.\"CoverUrl\", m.\"TrailerUrl\", g.\"Name\", l.\"Name\"");
            
            queryBuilder.Append($" ORDER BY \"{sorting.SortBy}\" {sorting.SortOrder}");

            if (paging.PageSize > 0) 
            {
                queryBuilder.Append($" OFFSET {paging.PageSize * (paging.PageNumber - 1)} LIMIT {paging.PageSize};");
            }
            else
            {
                queryBuilder.Append(";"); 
            }

            await using var connection = new NpgsqlConnection(_connectionString);
            var movies = new List<MovieGet>();

            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(queryBuilder.ToString(), connection);

            if (filtering.MovieId != null)
            {
                command.Parameters.AddWithValue("@MovieId", filtering.MovieId);
            }

            if (filtering.GenreId != null)
            {
                command.Parameters.AddWithValue("@GenreId", filtering.GenreId);
            }

            if (filtering.LanguageId != null)
            {
                command.Parameters.AddWithValue("@LanguageId", filtering.LanguageId);
            }
            
            if (!string.IsNullOrEmpty(filtering.SearchTerm))
            {
                command.Parameters.AddWithValue("@SearchTerm", filtering.SearchTerm);
            }
            
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var movie = new MovieGet
                {
                    MovieId = reader.GetGuid(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Genre = reader.GetString(reader.GetOrdinal("Genre")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                    Language = reader.IsDBNull(reader.GetOrdinal("Language")) ? null : reader.GetString(reader.GetOrdinal("Language")),
                    CoverUrl = reader.IsDBNull(reader.GetOrdinal("CoverUrl")) ? null : reader.GetString(reader.GetOrdinal("CoverUrl")),
                    TrailerUrl = reader.IsDBNull(reader.GetOrdinal("TrailerUrl")) ? null : reader.GetString(reader.GetOrdinal("TrailerUrl")),
                    ActorNames = reader.IsDBNull(reader.GetOrdinal("ActorNames")) ? null : reader.GetString(reader.GetOrdinal("ActorNames")).Split(", ").ToList(),
                };

                movies.Add(movie);
            }
            return movies;
        }

        public async Task<bool> MovieExistsAsync(string title)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"SELECT COUNT(*) FROM ""Movie"" WHERE ""Title"" = @Title";
            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Title", title);

            var count = (long)await command.ExecuteScalarAsync();
            return count > 0;
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = @"UPDATE ""Movie"" SET ""Title"" = @Title, ""Description"" = @Description, ""Duration"" = @Duration, ""LanguageId"" = @LanguageId, 
                                    ""GenreId"" = @GenreId, ""CoverUrl"" = @CoverUrl, ""TrailerUrl"" = @TrailerUrl, ""IsActive"" = @IsActive, ""DateUpdated"" = @DateUpdated, ""UpdatedByUserId"" = @UpdatedByUserId 
                                    WHERE ""Id"" = @Id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", movie.Id);
                    command.Parameters.AddWithValue("@Title", movie.Title);
                    command.Parameters.AddWithValue("@Description", movie.Description);
                    command.Parameters.AddWithValue("@Duration", movie.Duration);
                    command.Parameters.AddWithValue("@LanguageId", movie.LanguageId);
                    command.Parameters.AddWithValue("@GenreId", movie.GenreId);
                    command.Parameters.AddWithValue("@CoverUrl", movie.CoverUrl);
                    command.Parameters.AddWithValue("@TrailerUrl", movie.TrailerUrl);
                    command.Parameters.AddWithValue("@IsActive", movie.IsActive);
                    command.Parameters.AddWithValue("@DateUpdated", movie.DateUpdated);
                    command.Parameters.AddWithValue("@UpdatedByUserId", movie.UpdatedByUserId);

                    Console.WriteLine("Executing SQL: " + command.CommandText);
                    foreach (NpgsqlParameter param in command.Parameters)
                    {
                        Console.WriteLine($"{param.ParameterName}: {param.Value}");
                    }

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Rows affected: " + rowsAffected);

                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("No rows were updated. Check if the ID is correct.");
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteMovieAsync(Guid id)
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var deleteMovieActorCommandText = @"DELETE FROM ""MovieActor"" WHERE ""MovieId"" = @MovieId";
            var deleteMovieActorCommand = new NpgsqlCommand(deleteMovieActorCommandText, connection);
            deleteMovieActorCommand.Parameters.AddWithValue("@MovieId", id);
            await deleteMovieActorCommand.ExecuteNonQueryAsync();

            var deleteMovieCommandText = @"DELETE FROM ""Movie"" WHERE ""Id"" = @Id";
            var deleteMovieCommand = new NpgsqlCommand(deleteMovieCommandText, connection);
            deleteMovieCommand.Parameters.AddWithValue("@Id", id);
            await deleteMovieCommand.ExecuteNonQueryAsync();
        }

        public async Task DeleteActorFromMovie(Guid movieId, Guid actorId)
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var deleteMovieActorCommandText = @"DELETE FROM ""MovieActor"" WHERE ""MovieId"" = @MovieId AND ""ActorId"" = @ActorId";
            ;
            var deleteMovieActorCommand = new NpgsqlCommand(deleteMovieActorCommandText, connection);
            deleteMovieActorCommand.Parameters.AddWithValue("@MovieId", movieId);
            deleteMovieActorCommand.Parameters.AddWithValue("@ActorId", actorId);
            await deleteMovieActorCommand.ExecuteNonQueryAsync();

        }
    }
}
