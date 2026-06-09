using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Domain.DTOs.CinemaDTOs
{
    public record UpdateCinemaDto(
        [property: Required][property: MaxLength(200)] string Name,
        [property: Required][property: MaxLength(300)] string Location
    );
}
