namespace MovieReservationSystem.Domain.DTOs.AuthDTOs
{
    public record UserProfileDto(
        string Id,
        string Username,
        string Email,
        string FirstName,
        string LastName
    );
}
