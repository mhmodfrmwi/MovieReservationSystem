using MovieReservationSystem.Domain.Entities.Enums;

namespace MovieReservationSystem.Domain.DTOs.BookingDTOs
{
    public record CreateBookingDto(
        int ShowtimeId,
        List<int> SeatIds,
        PaymentMethod PaymentMethod = PaymentMethod.Visa
    );
}
