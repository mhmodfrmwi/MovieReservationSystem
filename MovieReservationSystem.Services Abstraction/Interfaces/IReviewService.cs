using MovieReservationSystem.Domain.DTOs.ReviewDTOs;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(CreateReviewDto dto, string userId, string userName);
        Task<IEnumerable<ReviewDto>> GetMovieReviewsAsync(int movieId);
        Task<bool> UpdateReviewAsync(int reviewId, string userId, UpdateReviewDto dto, bool isAdmin);
        Task<bool> DeleteReviewAsync(int reviewId, string userId, bool isAdmin);
    }
}
