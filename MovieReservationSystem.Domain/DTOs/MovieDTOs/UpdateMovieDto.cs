using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Domain.DTOs.MovieDTOs
{
    public record UpdateMovieDto(
        [property: Required][property: MaxLength(200)] string Title,
        [property: MaxLength(1000)] string Description,
        [property: Range(1, 600)] int DurationInMinutes,
        [property: MaxLength(500)] string PosterUrl,
        [property: MaxLength(50)] string Language,
        DateTime ReleaseDate,
        [property: MaxLength(500)] string TrailerUrl,
        List<int>? GenreIds = null
    );
}
