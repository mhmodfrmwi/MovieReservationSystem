using Microsoft.AspNetCore.Identity;
using MovieReservationSystem.Domain.Entities.IdentityModule;

namespace MovieReservationSystem.Presistence.Data
{
    public class ApplicationDbContextSeed
    {
        public static Task SeedAsync(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
            => ComprehensiveDataSeeder.SeedAsync(context, roleManager, userManager);
    }
}
