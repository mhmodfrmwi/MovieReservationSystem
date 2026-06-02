namespace MovieReservationSystem.Domain.DTOs.AuthDTOs
{
    public record RegisterDto(string Username, string Email, string Password, string FirstName, string LastName);
}
