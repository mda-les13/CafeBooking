using CafeBooking.BusinessLogic.DTOs;
using CafeBooking.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeBooking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto dto)
        {
            var reservation = await _reservationService.CreateReservationAsync(dto);
            return CreatedAtAction(nameof(CreateReservation), new { id = reservation.Id }, reservation);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReservationByPhone([FromQuery] string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest("Номер телефона не указан");

            await _reservationService.DeleteReservationByPhoneAsync(phoneNumber);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveReservations()
        {
            var activeReservations = await _reservationService.GetActiveReservationsAsync(DateTime.Now);
            return Ok(activeReservations);
        }
    }
}
