using Microsoft.AspNetCore.Identity;
using MovieReservationSystem.Domain.Entities.BookingModule;
namespace MovieReservationSystem.Domain.Entities.IdentityModule
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
