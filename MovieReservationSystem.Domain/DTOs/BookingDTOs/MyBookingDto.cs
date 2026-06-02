namespace MovieReservationSystem.Domain.DTOs.BookingDTOs
{
    public record MyBookingDto(
    int BookingId,
    DateTime BookingDate,
    decimal TotalPrice,
    int ShowtimeId,
    List<int> SeatIds
    );
}
