using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema.Model;

namespace Cinema.Service.Common
{
    public interface ISeatReservedService
    {
        Task AddSeatReservationAsync(SeatReserved reservation);
        Task<List<SeatReserved>> GetAllSeatReservationsAsync();
        Task<SeatReserved> GetSeatReservationByIdAsync(Guid id);
        Task UpdateSeatReservationAsync(SeatReserved reservation);
        Task DeleteSeatReservationAsync(Guid id);
    }
}
