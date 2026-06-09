using MovieReservationSystem.Domain.DTOs.ReviewDTOs;
using MovieReservationSystem.Domain.Entities.IdentityModule;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Interfaces;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class ReviewService(IUnitOfWork unitOfWork) : IReviewService
    {
        public async Task AddReviewAsync(CreateReviewDto dto, string userId, string userName)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var movie = await unitOfWork.Repository<Movie>().GetByIdAsync(dto.MovieId)
                ?? throw new Exception("Movie not found.");

            var existingReviews = await unitOfWork.Repository<Review>().GetAllAsync();
            if (existingReviews.Any(r => r.MovieId == dto.MovieId && r.AppUserId == userId))
                throw new Exception("You have already reviewed this movie.");

            var review = new Review
            {
                MovieId = dto.MovieId,
                AppUserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await unitOfWork.Repository<Review>().AddAsync(review);
            await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetMovieReviewsAsync(int movieId)
        {
            var allReviews = await unitOfWork.Repository<Review>().GetAllAsync();
            var users = await unitOfWork.Repository<AppUser>().GetAllAsync();
            var userLookup = users.ToDictionary(u => u.Id, u => u.UserName ?? "User");

            return allReviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UserName = userLookup.GetValueOrDefault(r.AppUserId, "User")
                }).ToList();
        }

        public async Task<bool> UpdateReviewAsync(int reviewId, string userId, UpdateReviewDto dto, bool isAdmin)
        {
            var review = await unitOfWork.Repository<Review>().GetByIdAsync(reviewId);
            if (review is null) return false;

            if (!isAdmin && review.AppUserId != userId)
                throw new Exception("You can only update your own reviews.");

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            unitOfWork.Repository<Review>().Update(review);
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, string userId, bool isAdmin)
        {
            var review = await unitOfWork.Repository<Review>().GetByIdAsync(reviewId);
            if (review is null) return false;

            if (!isAdmin && review.AppUserId != userId)
                throw new Exception("You can only delete your own reviews.");

            unitOfWork.Repository<Review>().Delete(review);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
