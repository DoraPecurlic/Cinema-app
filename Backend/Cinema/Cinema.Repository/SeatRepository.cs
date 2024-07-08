using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;

namespace Cinema.Repository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly string _connectionString;

        public SeatRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Seat>> GetAllSeatsAsync()
        {
            var seats = new List<Seat>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = "SELECT * FROM \"Seat\"";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var seat = new Seat
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                SeatNumber = reader.IsDBNull(reader.GetOrdinal("SeatNumber")) ? -1 : reader.GetInt32(reader.GetOrdinal("SeatNumber")),
                                HallId = reader.GetGuid(reader.GetOrdinal("HallId")),
                            };
                            seats.Add(seat);
                        }
                    }
                }
            }

            return seats;
        }

        public async Task<Seat> GetSeatByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(@"SELECT * FROM ""Seat"" WHERE ""Id"" = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Seat
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                SeatNumber = reader.IsDBNull(reader.GetOrdinal("SeatNumber")) ? -1 : reader.GetInt32(reader.GetOrdinal("SeatNumber")),
                                HallId = reader.GetGuid(reader.GetOrdinal("HallId")),
                            };
                        }
                    }
                }
            }
            return null;
        }
        
        public async Task<List<Seat>> GetSeatsByProjectionIdAsync(Guid projectionId)
    {
        var seats = new List<Seat>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var commandText = @"
                SELECT s.""Id"" AS ""SeatId"", s.""SeatNumber"", s.""RowLetter"", h.""Id"" AS ""HallId"", h.""HallNumber""
                FROM ""Projection"" p
                JOIN ""ProjectionHall"" ph ON p.""Id"" = ph.""ProjectionId""
                JOIN ""Hall"" h ON ph.""HallId"" = h.""Id""
                JOIN ""Seat"" s ON s.""HallId"" = h.""Id""
                WHERE p.""Id"" = @ProjectionId";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@ProjectionId", projectionId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var seat = new Seat
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("SeatId")),
                            SeatNumber = reader.IsDBNull(reader.GetOrdinal("SeatNumber")) ? -1 : reader.GetInt32(reader.GetOrdinal("SeatNumber")),
                            RowLetter = reader.GetString(reader.GetOrdinal("RowLetter")),
                            HallId = reader.GetGuid(reader.GetOrdinal("HallId")),
                            HallNumber = reader.GetInt32(reader.GetOrdinal("HallNumber"))
                        };
                        seats.Add(seat);
                    }
                }
            }
        }

        return seats;
    }

    public async Task<List<SeatReserved>> GetReservedSeatsByProjectionIdAsync(Guid projectionId)
    {
        var reservedSeats = new List<SeatReserved>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var commandText = @"
                SELECT sr.""Id"" AS ""ReservationId"", 
                       sr.""TicketId"", 
                       sr.""ProjectionId"", 
                       sr.""SeatId"", 
                       sr.""IsActive"", 
                       sr.""DateCreated"", 
                       sr.""DateUpdated"", 
                       sr.""CreatedByUserId"", 
                       sr.""UpdatedByUserId"",
                       s.""SeatNumber"", 
                       s.""RowLetter"", 
                       h.""Id"" AS ""HallId"", 
                       h.""HallNumber""
                FROM ""SeatReserved"" sr
                JOIN ""Seat"" s ON sr.""SeatId"" = s.""Id""
                JOIN ""Hall"" h ON s.""HallId"" = h.""Id""
                WHERE sr.""ProjectionId"" = @ProjectionId";
            using (var command = new NpgsqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@ProjectionId", projectionId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var seatReserved = new SeatReserved
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("ReservationId")),
                            TicketId = reader.GetGuid(reader.GetOrdinal("TicketId")),
                            ProjectionId = reader.GetGuid(reader.GetOrdinal("ProjectionId")),
                            SeatId = reader.GetGuid(reader.GetOrdinal("SeatId")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                            DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                            CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                            UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                            SeatNumber = reader.GetInt32(reader.GetOrdinal("SeatNumber")),
                            RowLetter = reader.GetString(reader.GetOrdinal("RowLetter")),
                            HallId = reader.GetGuid(reader.GetOrdinal("HallId")),
                            HallNumber = reader.GetInt32(reader.GetOrdinal("HallNumber"))
                        };
                        reservedSeats.Add(seatReserved);
                    }
                }
            }
        }

        return reservedSeats;
    }
        
        public async Task AddSeatAsync(Seat seat)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = @"INSERT INTO ""Seat"" (""Id"", ""SeatNumber"", ""HallId"") 
                                    VALUES (@Id, @SeatNumber, @HallId)";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", seat.Id);
                    command.Parameters.AddWithValue("@SeatNumber", (object?)seat.SeatNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HallId", seat.HallId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task AddReservedSeatAsync(SeatReserved seatReserved)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"
                INSERT INTO ""SeatReserved"" 
                    (""Id"", ""TicketId"", ""ProjectionId"", ""SeatId"", ""IsActive"", 
                    ""DateCreated"", ""DateUpdated"", ""CreatedByUserId"", ""UpdatedByUserId"")
                VALUES (@Id, @TicketId, @ProjectionId, @SeatId, @IsActive, 
                    @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId);";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", seatReserved.Id);
            command.Parameters.AddWithValue("@TicketId", seatReserved.TicketId);
            command.Parameters.AddWithValue("@ProjectionId", seatReserved.ProjectionId);
            command.Parameters.AddWithValue("@SeatId", seatReserved.SeatId);
            command.Parameters.AddWithValue("@IsActive", seatReserved.IsActive);
            command.Parameters.AddWithValue("@DateCreated", seatReserved.DateCreated);
            command.Parameters.AddWithValue("@DateUpdated", seatReserved.DateUpdated);
            command.Parameters.AddWithValue("@CreatedByUserId", seatReserved.CreatedByUserId);
            command.Parameters.AddWithValue("@UpdatedByUserId", seatReserved.UpdatedByUserId);

            await command.ExecuteNonQueryAsync();
            
        }
        public async Task UpdateSeatAsync(Seat seat)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = @"UPDATE ""Seat"" SET ""SeatNumber"" = @SeatNumber, ""HallId"" = @HallId WHERE ""Id"" = @Id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", seat.Id);
                    command.Parameters.AddWithValue("@SeatNumber", (object?)seat.SeatNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HallId", seat.HallId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteSeatAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = @"DELETE FROM ""Seat"" WHERE ""Id"" = @Id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
