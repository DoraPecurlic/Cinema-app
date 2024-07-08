using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema.Model;
using Cinema.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeatReservedController : Controller
    {
        private readonly ISeatReservedService _seatReservedService;

        public SeatReservedController(ISeatReservedService seatReservedService)
        {
            _seatReservedService = seatReservedService ?? throw new ArgumentNullException(nameof(seatReservedService));
        }

        [HttpPost]
        public async Task<IActionResult> AddSeatReservationAsync([FromBody] SeatReserved reservation)
        {
            await _seatReservedService.AddSeatReservationAsync(reservation);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeatReservationsAsync()
        {
            var reservations = await _seatReservedService.GetAllSeatReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatReservationByIdAsync(Guid id)
        {
            var reservation = await _seatReservedService.GetSeatReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeatReservationAsync(Guid id, [FromBody] SeatReserved reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest("Reservation ID mismatch");
            }

            try
            {
                await _seatReservedService.UpdateSeatReservationAsync(reservation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeatReservationAsync(Guid id)
        {
            try
            {
                await _seatReservedService.DeleteSeatReservationAsync(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
