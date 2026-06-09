namespace MovieReservationSystem.Domain.DTOs.MovieDTOs
{
    public record MovieDto(
        int Id,
        string Title,
        string Description,
        int DurationInMinutes,
        string PosterUrl,
        string Language,
        DateTime ReleaseDate,
        string TrailerUrl,
        IReadOnlyList<string> Genres
    );
}
