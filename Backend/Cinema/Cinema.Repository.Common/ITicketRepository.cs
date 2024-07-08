using Cinema.Model;

namespace Cinema.Repository.Common;

public interface ITicketRepository
{
    Task AddTicketAsync(Ticket ticket);
}