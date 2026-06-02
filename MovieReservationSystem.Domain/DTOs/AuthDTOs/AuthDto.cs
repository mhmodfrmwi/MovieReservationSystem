namespace MovieReservationSystem.Domain.DTOs.AuthDTOs
{
    public record AuthDto(
    string Message,
    bool IsAuthenticated,
    string Username,
    string Email,
    string Token,
    DateTime ExpiresOn
);
}
