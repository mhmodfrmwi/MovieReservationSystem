using MovieReservationSystem.Domain.DTOs.ReviewDTOs;
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

            var movie = await unitOfWork.Repository<Movie>().GetByIdAsync(dto.MovieId);
            if (movie == null)
                throw new Exception("Movie not found.");

            var existingReviews = await unitOfWork.Repository<Review>().GetAllAsync();
            bool alreadyReviewed = existingReviews.Any(r => r.MovieId == dto.MovieId && r.AppUserId == userId);
            if (alreadyReviewed)
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

            return allReviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UserName = "User"
                }).ToList();
        }
    }
}
