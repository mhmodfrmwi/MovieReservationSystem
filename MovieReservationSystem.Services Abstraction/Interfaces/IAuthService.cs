using MovieReservationSystem.Domain.DTOs.AuthDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto dto);
        Task<AuthDto> LoginAsync(LoginDto dto);
        Task<UserProfileDto?> GetProfileAsync(string userId);
        Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
    }
}
