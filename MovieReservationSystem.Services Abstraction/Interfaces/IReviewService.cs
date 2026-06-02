using MovieReservationSystem.Domain.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Services_Abstraction.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(CreateReviewDto dto, string userId, string userName);
        Task<IEnumerable<ReviewDto>> GetMovieReviewsAsync(int movieId);
    }
}
