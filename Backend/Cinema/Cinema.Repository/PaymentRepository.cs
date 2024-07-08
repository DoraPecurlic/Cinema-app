using System;
using System.Threading.Tasks;
using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;

namespace Cinema.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"INSERT INTO ""Payment"" 
                                (""Id"", ""TotalPrice"", ""PaymentDate"", ""IsActive"", ""DateCreated"", ""DateUpdated"", ""CreatedByUserId"", ""UpdatedByUserId"") 
                                VALUES (@Id, @TotalPrice, @PaymentDate, @IsActive, @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId);";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", payment.Id);
            command.Parameters.AddWithValue("@TotalPrice", payment.TotalPrice);
            command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
            command.Parameters.AddWithValue("@IsActive", payment.IsActive);
            command.Parameters.AddWithValue("@DateCreated", payment.DateCreated);
            command.Parameters.AddWithValue("@DateUpdated", payment.DateUpdated);
            command.Parameters.AddWithValue("@CreatedByUserId", payment.CreatedByUserId);
            command.Parameters.AddWithValue("@UpdatedByUserId", payment.UpdatedByUserId);

            await command.ExecuteNonQueryAsync();
        }
        
        
        public async Task<List<GetPayment>> GetAllPaymentsAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"SELECT ""Id"", ""TotalPrice"", ""PaymentDate""
                                FROM ""Payment"";";

            await using var command = new NpgsqlCommand(commandText, connection);
            var payments = new List<GetPayment>();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var payment = new GetPayment
                {
                    Id = reader.GetGuid(0),
                    TotalPrice = reader.GetDecimal(1),
                    PaymentDate = reader.GetDateTime(2)
                };
                payments.Add(payment);
            }

            return payments;
        }

        public async Task<List<GetPayment>> GetPaymentsByUserAsync(Guid userId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"SELECT ""Id"", ""TotalPrice"", ""PaymentDate""
                                FROM ""Payment""
                                WHERE ""CreatedByUserId"" = @userId;";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@userId", userId);
            var payments = new List<GetPayment>();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var payment = new GetPayment
                {
                    Id = reader.GetGuid(0),
                    TotalPrice = reader.GetDecimal(1),
                    PaymentDate = reader.GetDateTime(2)
                };
                payments.Add(payment);
            }

            return payments;
        }
    }
}
