namespace MovieReservationSystem.Domain.DTOs.BookingDTOs
{
    public record BookingDto
    (
        int BookingId,
        DateTime BookingDate,
        decimal TotalPrice,
        string Status,
        int ShowtimeId,
        string UserId,
        List<int> ReservedSeatIds,
        PaymentDto? Payment
    );
}
