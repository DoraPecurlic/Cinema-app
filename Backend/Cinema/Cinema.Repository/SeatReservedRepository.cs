using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;

namespace Cinema.Repository
{
    public class SeatReservedRepository : ISeatReservedRepository
    {
        private readonly string _connectionString;

        public SeatReservedRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddSeatReservationAsync(SeatReserved reservation)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = @"INSERT INTO ""SeatReserved"" (""Id"", ""TicketId"", ""ProjectionId"", ""SeatId"", ""IsActive"", ""DateCreated"", ""DateUpdated"", ""CreatedByUserId"", ""UpdatedByUserId"") 
                                    VALUES (@Id, @TicketId, @ProjectionId, @SeatId, @IsActive, @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId)";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", reservation.Id);
                    command.Parameters.AddWithValue("@TicketId", reservation.TicketId);
                    command.Parameters.AddWithValue("@ProjectionId", reservation.ProjectionId);
                    command.Parameters.AddWithValue("@SeatId", reservation.SeatId);
                    command.Parameters.AddWithValue("@IsActive", reservation.IsActive);
                    command.Parameters.AddWithValue("@DateCreated", reservation.DateCreated);
                    command.Parameters.AddWithValue("@DateUpdated", reservation.DateUpdated);
                    command.Parameters.AddWithValue("@CreatedByUserId", reservation.CreatedByUserId);
                    command.Parameters.AddWithValue("@UpdatedByUserId", reservation.UpdatedByUserId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<SeatReserved>> GetAllSeatReservationsAsync()
        {
            var reservations = new List<SeatReserved>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = "SELECT * FROM \"SeatReserved\"";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var reservation = new SeatReserved
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                TicketId = reader.GetGuid(reader.GetOrdinal("TicketId")),
                                ProjectionId = reader.GetGuid(reader.GetOrdinal("ProjectionId")),
                                SeatId = reader.GetGuid(reader.GetOrdinal("SeatId")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.IsDBNull(reader.GetOrdinal("DateUpdated")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                                UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                            };
                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }

        public async Task<SeatReserved> GetSeatReservationByIdAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(@"SELECT * FROM ""SeatReserved"" WHERE ""Id"" = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new SeatReserved
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                TicketId = reader.GetGuid(reader.GetOrdinal("TicketId")),
                                ProjectionId = reader.GetGuid(reader.GetOrdinal("ProjectionId")),
                                SeatId = reader.GetGuid(reader.GetOrdinal("SeatId")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.IsDBNull(reader.GetOrdinal("DateUpdated")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                                UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task UpdateSeatReservationAsync(SeatReserved reservation)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = @"UPDATE ""SeatReserved"" SET ""TicketId"" = @TicketId, ""ProjectionId"" = @ProjectionId, ""SeatId"" = @SeatId, ""IsActive"" = @IsActive, 
                                    ""DateUpdated"" = @DateUpdated, ""UpdatedByUserId"" = @UpdatedByUserId WHERE ""Id"" = @Id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", reservation.Id);
                    command.Parameters.AddWithValue("@TicketId", reservation.TicketId);
                    command.Parameters.AddWithValue("@ProjectionId", reservation.ProjectionId);
                    command.Parameters.AddWithValue("@SeatId", reservation.SeatId);
                    command.Parameters.AddWithValue("@IsActive", reservation.IsActive);
                    command.Parameters.AddWithValue("@DateUpdated", reservation.DateUpdated);
                    command.Parameters.AddWithValue("@UpdatedByUserId", reservation.UpdatedByUserId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteSeatReservationAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var commandText = @"DELETE FROM ""SeatReserved"" WHERE ""Id"" = @Id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
