using Cinema.Model;

namespace Cinema.Service.Common;

public interface ITicketService
{
    Task<Guid> AddTicketAsync(Ticket ticket);
}