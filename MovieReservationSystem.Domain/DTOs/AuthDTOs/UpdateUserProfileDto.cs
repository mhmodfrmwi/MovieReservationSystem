using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Domain.DTOs.AuthDTOs
{
    public record UpdateUserProfileDto(
        [property: Required][property: MaxLength(100)] string FirstName,
        [property: Required][property: MaxLength(100)] string LastName
    );
}
