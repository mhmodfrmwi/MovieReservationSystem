namespace MovieReservationSystem.Domain.Entities.CinemaModule
{
    public class Seat : BaseEntity<int>
    {
        public string Row { get; set; }
        public string Number { get; set; }
        public string Code => $"{Row}{Number}";

        public int HallId { get; set; }

        // Navigation Properties
        public Hall Hall { get; set; }
    }
}
