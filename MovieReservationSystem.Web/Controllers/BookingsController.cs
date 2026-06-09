using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.DTOs.BookingDTOs;
using MovieReservationSystem.Services_Abstraction.Interfaces;
using System.Security.Claims;

namespace MovieReservationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController(IBookingService bookingService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await bookingService.GetBookingByIdAsync(id);
            return booking is null ? NotFound() : Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto createBookingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID could not be determined from the token.");

            try
            {
                var booking = await bookingService.CreateBookingAsync(userId, createBookingDto);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-bookings")]
        public async Task<ActionResult<IEnumerable<MyBookingDto>>> GetMyBookings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID could not be determined from the token.");

            var bookings = await bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID could not be determined from the token.");

            try
            {
                var booking = await bookingService.CancelBookingAsync(id, userId);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
