using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Domain.DTOs.BookingDTOs;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.Enums;
using MovieReservationSystem.Domain.Entities.IdentityModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class BookingService(IUnitOfWork unitOfWork, IEmailService emailService) : IBookingService
    {
        public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto)
        {
            var showtime = await unitOfWork.Repository<Showtime>().GetByIdAsync(createBookingDto.ShowtimeId)
                ?? throw new Exception("Showtime not found");

            var user = await unitOfWork.Repository<AppUser>().GetByIdAsync(userId)
                ?? throw new Exception("User not found");

            var bookedSeatIds = await GetBookedSeatIdsForShowtimeAsync(createBookingDto.ShowtimeId);
            if (createBookingDto.SeatIds.Any(bookedSeatIds.Contains))
                throw new Exception("One or more selected seats are already booked");

            var totalPrice = showtime.TicketPrice * createBookingDto.SeatIds.Count;

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

            foreach (var seatId in createBookingDto.SeatIds)
            {
                await unitOfWork.Repository<Ticket>().AddAsync(new Ticket
                {
                    BookingId = booking.Id,
                    SeatId = seatId,
                    Price = showtime.TicketPrice
                });
            }

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = totalPrice,
                PaymentDate = DateTime.UtcNow,
                Method = createBookingDto.PaymentMethod,
                Status = PaymentStatus.Success
            };
            await unitOfWork.Repository<Payment>().AddAsync(payment);
            await unitOfWork.CompleteAsync();

            try
            {
                var emailBody = $@"
                    <h1>تم تأكيد حجزك بنجاح!</h1>
                    <p>رقم الحجز: {booking.Id}</p>
                    <p>إجمالي المبلغ: {booking.TotalAmount} EGP</p>
                    <p>نتمنى لك مشاهدة ممتعة!</p>";
                await emailService.SendEmailAsync(user.Email!, "تأكيد حجز تذكرة سينما", emailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Booking confirmed but email failed: {ex.Message}");
            }

            return await MapToBookingDtoAsync(booking.Id, createBookingDto.SeatIds);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int bookingId)
        {
            var booking = await unitOfWork.Repository<Booking>()
                .GetQueryable()
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking is null) return null;

            var seatIds = await GetSeatIdsForBookingAsync(bookingId);
            return MapToDto(booking, seatIds);
        }

        public async Task<IEnumerable<MyBookingDto>> GetUserBookingsAsync(string userId)
        {
            var userBookings = await unitOfWork.Repository<Booking>()
                .GetQueryable()
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            var result = new List<MyBookingDto>();
            foreach (var booking in userBookings)
            {
                var seatIds = await GetSeatIdsForBookingAsync(booking.Id);
                result.Add(new MyBookingDto(
                    booking.Id,
                    booking.BookingDate,
                    booking.TotalAmount,
                    booking.ShowtimeId,
                    seatIds
                ));
            }

            return result;
        }

        public async Task<BookingDto> CancelBookingAsync(int bookingId, string userId)
        {
            var booking = await unitOfWork.Repository<Booking>()
                .GetQueryable()
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId)
                ?? throw new Exception("Booking not found");

            if (booking.UserId != userId)
                throw new Exception("You can only cancel your own bookings.");

            if (booking.Status == BookingStatus.Canceled)
                throw new Exception("Booking is already canceled.");

            booking.Status = BookingStatus.Canceled;
            if (booking.Payment is not null)
                booking.Payment.Status = PaymentStatus.Failed;

            unitOfWork.Repository<Booking>().Update(booking);
            await unitOfWork.CompleteAsync();

            var seatIds = await GetSeatIdsForBookingAsync(bookingId);
            return MapToDto(booking, seatIds);
        }

        private async Task<BookingDto> MapToBookingDtoAsync(int bookingId, List<int> seatIds)
        {
            var booking = await unitOfWork.Repository<Booking>()
                .GetQueryable()
                .Include(b => b.Payment)
                .FirstAsync(b => b.Id == bookingId);

            return MapToDto(booking, seatIds);
        }

        private async Task<HashSet<int>> GetBookedSeatIdsForShowtimeAsync(int showtimeId)
        {
            var activeBookingIds = await unitOfWork.Repository<Booking>()
                .GetQueryable()
                .Where(b => b.ShowtimeId == showtimeId && b.Status != BookingStatus.Canceled)
                .Select(b => b.Id)
                .ToListAsync();

            var tickets = await unitOfWork.Repository<Ticket>().GetAllAsync();
            return tickets.Where(t => activeBookingIds.Contains(t.BookingId))
                .Select(t => t.SeatId)
                .ToHashSet();
        }

        private async Task<List<int>> GetSeatIdsForBookingAsync(int bookingId)
        {
            var tickets = await unitOfWork.Repository<Ticket>().GetAllAsync();
            return tickets.Where(t => t.BookingId == bookingId).Select(t => t.SeatId).ToList();
        }

        private static BookingDto MapToDto(Booking booking, List<int> seatIds)
        {
            PaymentDto? paymentDto = null;
            if (booking.Payment is not null)
            {
                paymentDto = new PaymentDto(
                    booking.Payment.Id,
                    booking.Payment.Amount,
                    booking.Payment.PaymentDate,
                    booking.Payment.Method.ToString(),
                    booking.Payment.Status.ToString(),
                    booking.Payment.BookingId
                );
            }

            return new BookingDto(
                booking.Id,
                booking.BookingDate,
                booking.TotalAmount,
                booking.Status.ToString(),
                booking.ShowtimeId,
                booking.UserId,
                seatIds,
                paymentDto
            );
        }
    }
}
