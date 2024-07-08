using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema.Model;

namespace Cinema.Repository.Common
{
    public interface ISeatReservedRepository
    {
        Task AddSeatReservationAsync(SeatReserved reservation);
        Task<List<SeatReserved>> GetAllSeatReservationsAsync();
        Task<SeatReserved> GetSeatReservationByIdAsync(Guid id);
        Task UpdateSeatReservationAsync(SeatReserved reservation);
        Task DeleteSeatReservationAsync(Guid id);
    }
}
