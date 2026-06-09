using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Domain.DTOs.ShowtimeDTOs
{
    public record UpdateShowtimeDto(
        DateTime StartTime,
        [property: Range(0.01, 10000)] decimal TicketPrice,
        int MovieId,
        int HallId
    );
}
