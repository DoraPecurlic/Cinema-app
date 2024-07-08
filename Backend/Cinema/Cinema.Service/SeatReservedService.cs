using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;

namespace Cinema.Service
{
    public class SeatReservedService : ISeatReservedService
    {
        private readonly ISeatReservedRepository _seatReservedRepository;

        public SeatReservedService(ISeatReservedRepository seatReservedRepository)
        {
            _seatReservedRepository = seatReservedRepository ?? throw new ArgumentNullException(nameof(seatReservedRepository));
        }

        public async Task AddSeatReservationAsync(SeatReserved reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            await _seatReservedRepository.AddSeatReservationAsync(reservation);
        }

        public async Task<List<SeatReserved>> GetAllSeatReservationsAsync()
        {
            return await _seatReservedRepository.GetAllSeatReservationsAsync();
        }

        public async Task<SeatReserved> GetSeatReservationByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid reservation ID", nameof(id));
            }

            return await _seatReservedRepository.GetSeatReservationByIdAsync(id);
        }

        public async Task UpdateSeatReservationAsync(SeatReserved reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            await _seatReservedRepository.UpdateSeatReservationAsync(reservation);
        }

        public async Task DeleteSeatReservationAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid reservation ID", nameof(id));
            }

            await _seatReservedRepository.DeleteSeatReservationAsync(id);
        }
    }
}
