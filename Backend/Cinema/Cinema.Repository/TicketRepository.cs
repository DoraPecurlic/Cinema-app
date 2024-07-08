using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;

namespace Cinema.Repository;

public class TicketRepository: ITicketRepository
{
    private readonly string _connectionString;

    public TicketRepository(string connectionString)
    {
        this._connectionString = connectionString;
    }
    
    public async Task AddTicketAsync(Ticket ticket)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var commandText = @"INSERT INTO ""Ticket"" 
                (""Id"", ""Price"", ""PaymentId"", ""ProjectionId"", ""UserId"", ""IsActive"", ""DateCreated"", ""DateUpdated"", ""CreatedByUserId"", ""UpdatedByUserId"") 
                VALUES (@Id, @Price, @PaymentId, @ProjectionId, @UserId, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @CreatedByUserId, @UpdatedByUserId);";

        await using var command = new NpgsqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Id", ticket.Id);
        command.Parameters.AddWithValue("@Price", ticket.Price);
        command.Parameters.AddWithValue("@PaymentId", ticket.PaymentId);
        command.Parameters.AddWithValue("@ProjectionId", ticket.ProjectionId);
        command.Parameters.AddWithValue("@UserId", ticket.UserId);
        command.Parameters.AddWithValue("@CreatedByUserId", ticket.CreatedByUserId);
        command.Parameters.AddWithValue("@UpdatedByUserId", ticket.UpdatedByUserId);

        await command.ExecuteNonQueryAsync();
    }
    
    
}