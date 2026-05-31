using Microsoft.AspNetCore.Identity;
namespace MovieReservationSystem.Domain.Entities.IdentityModule
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }
}
