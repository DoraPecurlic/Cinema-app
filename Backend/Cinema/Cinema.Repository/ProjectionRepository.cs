using Npgsql;
using Cinema.Model;
using Cinema.Repository.Common;

namespace Cinema.Repository
{
    public class ProjectionRepository : IProjectionRepository
    {
        private readonly string connectionString;
        public ProjectionRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Projection>> GetAllProjectionsAsync()
        {
            var projections = new List<Projection>();

            await using var connection = new NpgsqlConnection(connectionString);
            var commandText = @"
                SELECT p.*, m.""Title""
                FROM ""Projection"" p
                JOIN ""Movie"" m ON p.""MovieId"" = m.""Id"";";

            await using var command = new NpgsqlCommand(commandText, connection);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var projection = new Projection
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")).Date,
                    Time = reader.GetTimeSpan(reader.GetOrdinal("Time")),
                    MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                    Movie = new Movie
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("MovieId")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title"))
                    }
                };
                projections.Add(projection);
            }

            return projections;
        }

        public async Task<Projection?> GetProjectionByIdAsync(Guid id)
        {
            await using var connection = new NpgsqlConnection(connectionString);
            var commandText = @"
                SELECT p.*, m.""Title""
                FROM ""Projection"" p
                JOIN ""Movie"" m ON p.""MovieId"" = m.""Id""
                WHERE p.""Id"" = @Id;";
            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var projection = new Projection
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")).Date,
                    Time = reader.GetTimeSpan(reader.GetOrdinal("Time")),
                    MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                    Movie = new Movie
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("MovieId")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title"))
                    }
                };

                return projection;
            }

            return null;
        }
        
        public async Task<IEnumerable<Projection>> GetProjectionsByMovieIdAsync(Guid id)
        {
            await using var connection = new NpgsqlConnection(connectionString);
            var commandText = @"
                SELECT p.*, m.""Title""
                FROM ""Projection"" p
                JOIN ""Movie"" m ON p.""MovieId"" = m.""Id""
                WHERE p.""MovieId"" = @Id;
            ";
            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            var projections = new List<Projection>();

            while (await reader.ReadAsync())
            {
                var projection = new Projection
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")).Date,
                    Time = reader.GetTimeSpan(reader.GetOrdinal("Time")),
                    MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                    Movie = new Movie
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("MovieId")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title"))
                    }
                };

                projections.Add(projection);
            }

            return projections;
        }

        public async Task AddProjectionAsync(Projection projection)
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var commandText = "INSERT INTO \"Projection\" (\"Id\", \"Date\", \"Time\", \"MovieId\", \"UserId\", \"IsActive\", \"DateCreated\", \"DateUpdated\", \"CreatedByUserId\", \"UpdatedByUserId\") " +
                                  "VALUES (@Id, @Date, @Time, @MovieId, @UserId, @IsActive, @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId);";

                await using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", projection.Id);
                command.Parameters.AddWithValue("@Date", projection.Date);
                command.Parameters.AddWithValue("@Time", projection.Time);
                command.Parameters.AddWithValue("@MovieId", projection.MovieId);
                command.Parameters.AddWithValue("@UserId", projection.UserId);
                command.Parameters.AddWithValue("@IsActive", projection.IsActive);
                command.Parameters.AddWithValue("@DateCreated", projection.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", projection.DateUpdated);
                command.Parameters.AddWithValue("@CreatedByUserId", projection.CreatedByUserId);
                command.Parameters.AddWithValue("@UpdatedByUserId", projection.UpdatedByUserId);

                await command.ExecuteNonQueryAsync();

                foreach (var projectionHall in projection.ProjectionHalls)
                {
                    projectionHall.Id = Guid.NewGuid();
                    projectionHall.ProjectionId = projection.Id;

                    var projectionHallCommandText = "INSERT INTO \"ProjectionHall\" (\"Id\", \"ProjectionId\", \"HallId\") " +
                                                    "VALUES (@Id, @ProjectionId, @HallId);";

                    await using var projectionHallCommand = new NpgsqlCommand(projectionHallCommandText, connection);
                    projectionHallCommand.Parameters.AddWithValue("@Id", projectionHall.Id);
                    projectionHallCommand.Parameters.AddWithValue("@ProjectionId", projectionHall.ProjectionId);
                    projectionHallCommand.Parameters.AddWithValue("@HallId", projectionHall.HallId);

                    await projectionHallCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateProjectionAsync(Projection projection)
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var commandText = "UPDATE \"Projection\" SET \"Date\" = @Date, \"Time\" = @Time, \"MovieId\" = @MovieId, \"UserId\" = @UserId, " +
                                  "\"IsActive\" = @IsActive, \"DateUpdated\" = @DateUpdated, \"UpdatedByUserId\" = @UpdatedByUserId WHERE \"Id\" = @Id;";
                await using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@Id", projection.Id);
                command.Parameters.AddWithValue("@Date", projection.Date);
                command.Parameters.AddWithValue("@Time", projection.Time);
                command.Parameters.AddWithValue("@MovieId", projection.MovieId);
                command.Parameters.AddWithValue("@UserId", projection.UserId);
                command.Parameters.AddWithValue("@IsActive", projection.IsActive);
                command.Parameters.AddWithValue("@DateUpdated", projection.DateUpdated);
                command.Parameters.AddWithValue("@UpdatedByUserId", projection.UpdatedByUserId);

                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteProjectionAsync(Guid id)
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var deleteProjectionHallsCommandText = "DELETE FROM \"ProjectionHall\" WHERE \"ProjectionId\" = @ProjectionId;";
                await using var deleteProjectionHallsCommand = new NpgsqlCommand(deleteProjectionHallsCommandText, connection);
                deleteProjectionHallsCommand.Parameters.AddWithValue("@ProjectionId", id);
                await deleteProjectionHallsCommand.ExecuteNonQueryAsync();

                var deleteProjectionCommandText = "DELETE FROM \"Projection\" WHERE \"Id\" = @Id;";
                await using var deleteProjectionCommand = new NpgsqlCommand(deleteProjectionCommandText, connection);
                deleteProjectionCommand.Parameters.AddWithValue("@Id", id);
                await deleteProjectionCommand.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
