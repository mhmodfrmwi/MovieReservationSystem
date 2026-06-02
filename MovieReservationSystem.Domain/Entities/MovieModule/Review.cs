namespace MovieReservationSystem.Domain.Entities.MovieModule
{
    public class Review : BaseEntity<int>
    {
        public int MovieId { get; set; }
        public string AppUserId { get; set; } = string.Empty;

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Movie Movie { get; set; } = null!;
    }
}
