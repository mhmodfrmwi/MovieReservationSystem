namespace MovieReservationSystem.Domain.DTOs.ShowtimeDTOs
{
    public record CreateShowtimeDto(DateTime StartTime, decimal TicketPrice, int MovieId, int HallId);
}
