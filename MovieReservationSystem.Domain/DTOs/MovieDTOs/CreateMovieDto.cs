namespace MovieReservationSystem.Domain.DTOs.MovieDTOs
{
    public record CreateMovieDto(
        string Title,
        string Description,
        decimal TicketPrice,
        int DurationInMinutes,
        string PosterUrl,
        string Language,
        DateTime ReleaseDate,
        string TrailerUrl
    );
}
