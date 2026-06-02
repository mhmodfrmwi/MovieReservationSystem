using MovieReservationSystem.Domain.DTOs.BookingDTOs;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.Enums;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class BookingService(IUnitOfWork unitOfWork) : IBookingService
    {
        public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto)
        {
            try
            {
                var showtime = await unitOfWork.Repository<Showtime>().GetByIdAsync(createBookingDto.ShowtimeId);
                if (showtime == null)
                {
                    throw new Exception("Showtime not found");
                }
                var existingBookings = await unitOfWork.Repository<Booking>().GetAllAsync();
                var showtimeBookings = existingBookings.Where(b => b.ShowtimeId == createBookingDto.ShowtimeId);
                var existingTickets = await unitOfWork.Repository<Ticket>().GetAllAsync();
                var bookedSeatIds = existingTickets.Where(t => showtimeBookings.Any(b => b.Id == t.BookingId)).Select(t => t.SeatId).ToHashSet();
                var isAnySeatAlreadyBooked = createBookingDto.SeatIds.Any(seatId => bookedSeatIds.Contains(seatId));
                if (isAnySeatAlreadyBooked)
                {
                    throw new Exception("One or more selected seats are already booked");
                }
                decimal totalPrice = showtime.TicketPrice * createBookingDto.SeatIds.Count;
                var booking = new Booking
                {
                    UserId = userId,
                    ShowtimeId = createBookingDto.ShowtimeId,
                    BookingDate = DateTime.UtcNow,
                    TotalAmount = totalPrice,
                    Status = BookingStatus.Confirmed
                };
                await unitOfWork.Repository<Booking>().AddAsync(booking);
                await unitOfWork.CompleteAsync();
                for (var seatId = 0; seatId < createBookingDto.SeatIds.Count; seatId++)
                {
                    var ticket = new Ticket
                    {
                        BookingId = booking.Id,
                        SeatId = createBookingDto.SeatIds[seatId],
                        Price = showtime.TicketPrice
                    };
                    await unitOfWork.Repository<Ticket>().AddAsync(ticket);
                }
                await unitOfWork.CompleteAsync();
                return new BookingDto
                (
                     booking.Id,
                     booking.BookingDate,
                     booking.TotalAmount,
                     booking.Status.ToString(),
                     booking.ShowtimeId,
                     booking.UserId,
                     createBookingDto.SeatIds
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the booking: {ex.Message}");
                throw;
            }

        }

        public async Task<BookingDto> GetBookingByIdAsync(int bookingId)
        {
            var booking = await unitOfWork.Repository<Booking>().GetByIdAsync(bookingId);
            if (booking == null)
            {
                return null;
            }
            return new BookingDto
            (
                 booking.Id,
                 booking.BookingDate,
                 booking.TotalAmount,
                 booking.Status.ToString(),
                 booking.ShowtimeId,
                 booking.UserId,
                 booking.Tickets.Select(t => t.SeatId).ToList()
            );
        }
        public async Task<IEnumerable<MyBookingDto>> GetUserBookingsAsync(string userId)
        {
            var allBookings = await unitOfWork.Repository<Booking>().GetAllAsync();
            var userBookings = allBookings.Where(b => b.UserId == userId).ToList();

            var allTickets = await unitOfWork.Repository<Ticket>().GetAllAsync();

            var result = new List<MyBookingDto>();

            foreach (var booking in userBookings)
            {
                var seatIds = allTickets.Where(t => t.BookingId == booking.Id)
                                        .Select(t => t.SeatId)
                                        .ToList();

                result.Add(new MyBookingDto(
                    booking.Id,
                    booking.BookingDate,
                    booking.TotalAmount,
                    booking.ShowtimeId,
                    seatIds
                ));
            }

            return result.OrderByDescending(b => b.BookingDate);
        }
    }
}
