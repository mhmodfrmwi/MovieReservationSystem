using MovieReservationSystem.Domain.Entities.ShowtimeModule;

namespace MovieReservationSystem.Domain.Entities.CinemaModule
{
    public class Hall : BaseEntity<int>
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int CinemaId { get; set; }

        // Navigation Properties
        public Cinema Cinema { get; set; }
        public IList<Seat> Seats { get; set; } = new List<Seat>();
        public IList<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
