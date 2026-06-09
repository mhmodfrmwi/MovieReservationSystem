namespace MovieReservationSystem.Domain.DTOs.BookingDTOs
{
    public record PaymentDto(
        int Id,
        decimal Amount,
        DateTime PaymentDate,
        string Method,
        string Status,
        int BookingId
    );
}
