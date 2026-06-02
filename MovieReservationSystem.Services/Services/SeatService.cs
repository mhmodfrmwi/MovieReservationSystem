using MovieReservationSystem.Domain.DTOs.SeatDTOs;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class SeatService(IUnitOfWork unitOfWork) : ISeatService
    {
        public async Task<string> GenerateSeatsForHallAsync(GenerateSeatsDto dto)
        {
            var hall = await unitOfWork.Repository<Hall>().GetByIdAsync(dto.HallId);
            if (hall == null)
            {
                throw new Exception($"Hall with id {dto.HallId} not found.");
            }
            int seatsPerRow = 10;
            char rowLetter = 'A';

            for (int i = 0; i < dto.Capacity; i++)
            {
                if (i > 0 && i % seatsPerRow == 0)
                {
                    rowLetter++;
                }

                int seatNumberInRow = (i % seatsPerRow) + 1;
                string fullSeatNumber = $"{rowLetter}{seatNumberInRow}";

                var seat = new Seat
                {
                    Number = fullSeatNumber,
                    HallId = dto.HallId,
                    Row = rowLetter.ToString()
                };

                await unitOfWork.Repository<Seat>().AddAsync(seat);
            }
            await unitOfWork.CompleteAsync();
            return $"Successfully generated {dto.Capacity} seats for hall with id {dto.HallId}.";
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsByHallIdAsync(int hallId)
        {
            var seats = await unitOfWork.Repository<Seat>().GetAllAsync();
            return seats.Where(s => s.HallId == hallId)
                    .Select(s => new SeatDto(s.Id, s.Code, true, s.HallId));
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsForShowtimeAsync(int showtimeId)
        {
            var showtime = await unitOfWork.Repository<Showtime>().GetByIdAsync(showtimeId);
            if (showtime == null) throw new Exception("Showtime not found!");

            var allSeatsInHall = await unitOfWork.Repository<Seat>().GetAllAsync();
            var hallSeats = allSeatsInHall.Where(s => s.HallId == showtime.HallId).ToList();

            var allBookings = await unitOfWork.Repository<Booking>().GetAllAsync();
            var showtimeBookingIds = allBookings.Where(b => b.ShowtimeId == showtimeId)
                                                .Select(b => b.Id)
                                                .ToList();

            var allTickets = await unitOfWork.Repository<Ticket>().GetAllAsync();
            var reservedSeatIds = allTickets.Where(t => showtimeBookingIds.Contains(t.BookingId))
                                            .Select(t => t.SeatId)
                                            .ToList();

            return hallSeats.Select(seat => new SeatDto(
                seat.Id,
                seat.Number,
                !reservedSeatIds.Contains(seat.Id),
                seat.HallId
            ));
        }
    }
}
