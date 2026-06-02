namespace MovieReservationSystem.Domain.DTOs.SeatDTOs
{
    public record SeatDto(int Id, string SeatNumber, bool IsAvailable, int HallId);
}
