namespace MovieReservationSystem.Domain.DTOs.ShowtimeDTOs
{
    public record ShowtimeDto(int Id, DateTime StartTime, decimal TicketPrice, int MovieId, int HallId);
}
