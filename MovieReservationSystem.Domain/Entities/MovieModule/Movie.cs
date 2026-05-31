using MovieReservationSystem.Domain.Entities.ShowtimeModule;

namespace MovieReservationSystem.Domain.Entities.MovieModule
{
    public class Movie : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public int DurationInMinutes { get; set; }
        public string TrailerUrl { get; set; }

        public string Language { get; set; }

        // Navigation properties
        public IList<Genre> Genres { get; set; }=new List<Genre>();
        public IList<Showtime> Showtimes { get; set; }= new List<Showtime>();
    }
}
