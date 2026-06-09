using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Domain.DTOs.HallDTOs
{
    public record UpdateHallDto(
        [property: Required][property: MaxLength(100)] string Name,
        [property: Range(1, 10000)] int Capacity,
        int CinemaId
    );
}
