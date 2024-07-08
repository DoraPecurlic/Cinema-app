using Npgsql;
using Cinema.Model;
using Cinema.Repository.Common;

namespace Cinema.Repository
{
    public class ActorRepository: IActorRepository
    {
        private readonly string _connectionString;
        public ActorRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task AddActorAsync(Actor actor)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = "INSERT INTO \"Actor\" (\"Id\", \"Name\", \"IsActive\", \"DateCreated\", \"DateUpdated\", \"CreatedByUserId\", \"UpdatedByUserId\") " +
                              "VALUES (@Id, @Name, @IsActive, @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId);";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", actor.Id);
            command.Parameters.AddWithValue("@Name", actor.Name);
            command.Parameters.AddWithValue("@IsActive", actor.IsActive);
            command.Parameters.AddWithValue("@DateCreated", actor.DateCreated);
            command.Parameters.AddWithValue("@DateUpdated", actor.DateUpdated);
            command.Parameters.AddWithValue("@CreatedByUserId", actor.CreatedByUserId);
            command.Parameters.AddWithValue("@UpdatedByUserId", actor.UpdatedByUserId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Actor>> GetAllActorsAsync()
        {
            var actors = new List<Actor>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            var commandText = "SELECT * FROM \"Actor\";";

            await using var command = new NpgsqlCommand(commandText, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var actor = new Actor
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId"))
                };
                actors.Add(actor);
            }

            return actors;
        }

        public async Task<Actor?> GetActorAsync(Guid id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var commandText = "SELECT * FROM \"Actor\" WHERE \"Id\" = @Id;";
            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Actor
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId"))
                };
            }

            return null;
        }
        
        public async Task<List<Actor>> GetActorsByNameAsync(List<string> actorNames)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"
                SELECT * FROM ""Actor"" WHERE ""Name"" = ANY(@Names);";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Names", actorNames);

            await using var reader = await command.ExecuteReaderAsync();

            var actors = new List<Actor>();
            while (await reader.ReadAsync())
            {
                actors.Add(new Actor
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated"))
                });
            }

            return actors;
        }
        
        public async Task<Actor?> GetActorByNameAsync(string actorName)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"
            SELECT * FROM ""Actor"" 
            WHERE ""Name"" = @Name
            LIMIT 1;";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Name", actorName);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Actor
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                    CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                    UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId"))
                };
            }

            return null;
        }

        public async Task UpdateActorAsync(Actor actor)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var commandText = "UPDATE \"Actor\" SET \"Name\" = @Name, \"IsActive\" = @IsActive, \"DateUpdated\" = @DateUpdated, \"UpdatedByUserId\" = @UpdatedByUserId " +
                              "WHERE \"Id\" = @Id;";
            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", actor.Id);
            command.Parameters.AddWithValue("@Name", actor.Name);
            command.Parameters.AddWithValue("@IsActive", actor.IsActive);
            command.Parameters.AddWithValue("@DateUpdated", actor.DateUpdated);
            command.Parameters.AddWithValue("@UpdatedByUserId", actor.UpdatedByUserId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteActorAsync(Guid id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var commandText = "DELETE FROM \"Actor\" WHERE \"Id\" = @Id;";
            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
