using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema.Model;

namespace Cinema.Repository.Common
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetAllSeatsAsync();
        Task<Seat> GetSeatByIdAsync(Guid id);
        Task<List<Seat>> GetSeatsByProjectionIdAsync(Guid projectionId);
        Task<List<SeatReserved>> GetReservedSeatsByProjectionIdAsync(Guid projectionId);
        Task AddSeatAsync(Seat seat);
        Task AddReservedSeatAsync(SeatReserved seatReserved);

        Task UpdateSeatAsync(Seat seat);
        Task DeleteSeatAsync(Guid id);
    }
}