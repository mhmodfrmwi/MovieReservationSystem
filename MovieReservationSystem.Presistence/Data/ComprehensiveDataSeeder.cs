using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.Domain.Constants;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.Enums;
using MovieReservationSystem.Domain.Entities.IdentityModule;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Presistence.Data.Seed;

namespace MovieReservationSystem.Presistence.Data
{
    internal static class ComprehensiveDataSeeder
    {
        private const int TargetMovieCount = 50;
        private const int TargetBookingCount = 150;
        private const int TargetReviewCount = 200;

        private static readonly string[] DemoUserPassword = ["User123!"];

        private static readonly (string First, string Last, string Email)[] DemoUsers =
        [
            ("Ahmed", "Hassan", "ahmed.hassan@demo.com"),
            ("Sara", "Mahmoud", "sara.mahmoud@demo.com"),
            ("Omar", "Khalil", "omar.khalil@demo.com"),
            ("Fatma", "Ali", "fatma.ali@demo.com"),
            ("Youssef", "Ibrahim", "youssef.ibrahim@demo.com"),
            ("Nour", "Adel", "nour.adel@demo.com"),
            ("Karim", "Mostafa", "karim.mostafa@demo.com"),
            ("Mariam", "Farouk", "mariam.farouk@demo.com"),
            ("Hana", "Saeed", "hana.saeed@demo.com"),
            ("Tarek", "Nabil", "tarek.nabil@demo.com"),
            ("Layla", "Hamza", "layla.hamza@demo.com"),
            ("Mohamed", "Salah", "mohamed.salah@demo.com"),
            ("Dina", "Rashad", "dina.rashad@demo.com"),
            ("Amr", "Waleed", "amr.waleed@demo.com"),
            ("Salma", "Hesham", "salma.hesham@demo.com"),
            ("James", "Wilson", "james.wilson@demo.com"),
            ("Emily", "Brown", "emily.brown@demo.com"),
            ("Lucas", "Martin", "lucas.martin@demo.com"),
            ("Sophie", "Dubois", "sophie.dubois@demo.com"),
            ("Ali", "Rahman", "ali.rahman@demo.com")
        ];

        private static readonly string[] ReviewComments =
        [
            "Absolutely stunning! One of the best films I've seen this year.",
            "Great cinematography and a powerful story. Highly recommended.",
            "Good movie but the pacing was a bit slow in the middle.",
            "Amazing performances from the entire cast.",
            "Worth every penny of the ticket. Would watch again!",
            "The soundtrack alone makes this worth watching.",
            "Not bad, but I expected more based on the hype.",
            "Perfect date night movie. We loved it!",
            "Edge-of-your-seat experience from start to finish.",
            "A masterpiece. Already planning to see it again.",
            "Solid entertainment with a few surprising twists.",
            "The visual effects were incredible on the big screen.",
            "Emotional and thought-provoking. Bring tissues!",
            "Fun, light-hearted, and exactly what I needed.",
            "One of those movies that stays with you for days."
        ];

        internal static async Task SeedAsync(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedAdminUserAsync(userManager);
            await SeedDemoUsersAsync(userManager);

            await SeedGenresAsync(context);
            await SeedCinemasAndHallsAsync(context);
            await RefreshMoviePosterUrlsAsync(context);
            await SeedMoviesAsync(context);
            await SeedShowtimesAsync(context);
            await SeedBookingsAsync(context);
            await SeedReviewsAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in new[] { Roles.Admin, Roles.User })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            const string adminEmail = "admin@movie.com";
            if (await userManager.FindByEmailAsync(adminEmail) is not null)
                return;

            var admin = new AppUser
            {
                UserName = "admin",
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin"
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Roles.Admin);
                await userManager.AddToRoleAsync(admin, Roles.User);
            }
        }

        private static async Task SeedDemoUsersAsync(UserManager<AppUser> userManager)
        {
            foreach (var (first, last, email) in DemoUsers)
            {
                if (await userManager.FindByEmailAsync(email) is not null)
                    continue;

                var user = new AppUser
                {
                    UserName = email.Split('@')[0],
                    Email = email,
                    FirstName = first,
                    LastName = last,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, DemoUserPassword[0]);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, Roles.User);
            }
        }

        private static async Task SeedGenresAsync(ApplicationDbContext context)
        {
            var genreNames = new[]
            {
                "Action", "Comedy", "Drama", "Fantasy", "Horror", "Mystery", "Romance",
                "Thriller", "Western", "Sci-Fi", "Animation", "Adventure", "Crime",
                "Documentary", "Family", "History", "Music"
            };

            var existing = await context.Genres.Select(g => g.Name).ToListAsync();
            var toAdd = genreNames
                .Where(name => !existing.Contains(name))
                .Select(name => new Genre { Name = name })
                .ToList();

            if (toAdd.Count > 0)
            {
                context.Genres.AddRange(toAdd);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedCinemasAndHallsAsync(ApplicationDbContext context)
        {
            var existingCinemaNames = await context.Cinemas.Select(c => c.Name).ToListAsync();

            foreach (var cinemaEntry in CinemaSeedCatalog.All)
            {
                if (existingCinemaNames.Contains(cinemaEntry.Name))
                    continue;

                var cinema = new Cinema
                {
                    Name = cinemaEntry.Name,
                    Location = cinemaEntry.Location
                };
                context.Cinemas.Add(cinema);
                await context.SaveChangesAsync();

                foreach (var hallEntry in cinemaEntry.Halls)
                {
                    var hall = new Hall
                    {
                        Name = hallEntry.Name,
                        Capacity = hallEntry.Capacity,
                        CinemaId = cinema.Id
                    };
                    context.Halls.Add(hall);
                    await context.SaveChangesAsync();

                    var seats = GenerateSeats(hall.Id, hallEntry.Capacity, hallEntry.SeatsPerRow);
                    context.Seats.AddRange(seats);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static List<Seat> GenerateSeats(int hallId, int capacity, int seatsPerRow)
        {
            var seats = new List<Seat>(capacity);
            var rowLetter = 'A';

            for (var i = 0; i < capacity; i++)
            {
                if (i > 0 && i % seatsPerRow == 0)
                    rowLetter++;

                var seatNumberInRow = (i % seatsPerRow) + 1;
                seats.Add(new Seat
                {
                    HallId = hallId,
                    Row = rowLetter.ToString(),
                    Number = seatNumberInRow.ToString()
                });
            }

            return seats;
        }

        private static async Task RefreshMoviePosterUrlsAsync(ApplicationDbContext context)
        {
            var movies = await context.Movies.ToListAsync();
            var updated = false;

            foreach (var movie in movies)
            {
                if (!MoviePosterUrls.ByTitle.TryGetValue(movie.Title, out var posterUrl))
                    continue;

                if (movie.PosterUrl == posterUrl)
                    continue;

                movie.PosterUrl = posterUrl;
                updated = true;
            }

            if (updated)
                await context.SaveChangesAsync();
        }

        private static async Task SeedMoviesAsync(ApplicationDbContext context)
        {
            if (await context.Movies.CountAsync() >= TargetMovieCount)
                return;

            var genreLookup = await context.Genres.ToDictionaryAsync(g => g.Name, g => g);
            var existingTitles = await context.Movies.Select(m => m.Title).ToListAsync();

            foreach (var entry in MovieSeedCatalog.All)
            {
                if (existingTitles.Contains(entry.Title))
                    continue;

                if (await context.Movies.CountAsync() >= TargetMovieCount)
                    break;

                var movie = new Movie
                {
                    Title = entry.Title,
                    Description = entry.Description,
                    ReleaseDate = entry.ReleaseDate,
                    DurationInMinutes = entry.DurationMinutes,
                    Language = entry.Language,
                    PosterUrl = entry.PosterUrl,
                    TrailerUrl = entry.TrailerYoutubeId
                };

                foreach (var genreName in entry.Genres.Distinct())
                {
                    if (genreLookup.TryGetValue(genreName, out var genre))
                        movie.Genres.Add(genre);
                }

                context.Movies.Add(movie);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedShowtimesAsync(ApplicationDbContext context)
        {
            if (await context.Showtimes.CountAsync() >= 400)
                return;

            var movies = await context.Movies.ToListAsync();
            var halls = await context.Halls.ToListAsync();
            if (movies.Count == 0 || halls.Count == 0)
                return;

            var random = new Random(42);
            var showtimes = new List<Showtime>();
            var slotHours = new[] { 10, 13, 16, 19, 22 };

            foreach (var movie in movies)
            {
                var showtimeCount = random.Next(6, 11);
                for (var i = 0; i < showtimeCount; i++)
                {
                    var hall = halls[random.Next(halls.Count)];
                    var dayOffset = random.Next(0, 21);
                    var hour = slotHours[random.Next(slotHours.Length)];
                    var start = DateTime.UtcNow.Date.AddDays(dayOffset).AddHours(hour);
                    if (start < DateTime.UtcNow.AddHours(-2))
                        start = start.AddDays(7);

                    var basePrice = hall.Name.Contains("IMAX", StringComparison.OrdinalIgnoreCase) ? 18m
                        : hall.Name.Contains("VIP", StringComparison.OrdinalIgnoreCase) ||
                          hall.Name.Contains("Gold", StringComparison.OrdinalIgnoreCase) ||
                          hall.Name.Contains("Platinum", StringComparison.OrdinalIgnoreCase) ? 22m
                        : hall.Name.Contains("4DX", StringComparison.OrdinalIgnoreCase) ? 20m
                        : 12m;

                    var priceVariation = (decimal)(random.NextDouble() * 4 - 2);
                    var ticketPrice = Math.Max(8m, Math.Round(basePrice + priceVariation, 1));

                    showtimes.Add(new Showtime
                    {
                        MovieId = movie.Id,
                        HallId = hall.Id,
                        StartTime = start,
                        EndTime = start.AddMinutes(movie.DurationInMinutes + 15),
                        TicketPrice = ticketPrice
                    });
                }
            }

            context.Showtimes.AddRange(showtimes);
            await context.SaveChangesAsync();
        }

        private static async Task SeedBookingsAsync(ApplicationDbContext context)
        {
            if (await context.Bookings.CountAsync() >= TargetBookingCount)
                return;

            var users = await context.Users.ToListAsync();
            var showtimes = await context.Showtimes
                .OrderBy(s => s.StartTime)
                .Take(200)
                .ToListAsync();

            if (users.Count == 0 || showtimes.Count == 0)
                return;

            var random = new Random(123);
            var paymentMethods = Enum.GetValues<PaymentMethod>();
            var bookedSeatsByShowtime = new Dictionary<int, HashSet<int>>();

            var existingTickets = await context.Tickets
                .Select(t => new { t.BookingId, t.SeatId })
                .ToListAsync();
            var bookingShowtimeMap = await context.Bookings
                .Select(b => new { b.Id, b.ShowtimeId })
                .ToListAsync();

            foreach (var ticket in existingTickets)
            {
                var showtimeId = bookingShowtimeMap.FirstOrDefault(b => b.Id == ticket.BookingId)?.ShowtimeId;
                if (showtimeId is null) continue;
                if (!bookedSeatsByShowtime.ContainsKey(showtimeId.Value))
                    bookedSeatsByShowtime[showtimeId.Value] = [];
                bookedSeatsByShowtime[showtimeId.Value].Add(ticket.SeatId);
            }

            var bookingsToCreate = TargetBookingCount - await context.Bookings.CountAsync();
            var created = 0;

            foreach (var showtime in showtimes)
            {
                if (created >= bookingsToCreate)
                    break;

                if (!bookedSeatsByShowtime.ContainsKey(showtime.Id))
                    bookedSeatsByShowtime[showtime.Id] = [];

                var hallSeats = await context.Seats
                    .Where(s => s.HallId == showtime.HallId)
                    .Select(s => s.Id)
                    .ToListAsync();

                var availableSeats = hallSeats
                    .Where(id => !bookedSeatsByShowtime[showtime.Id].Contains(id))
                    .ToList();

                if (availableSeats.Count < 2)
                    continue;

                var bookingsForShowtime = random.Next(1, 4);
                for (var b = 0; b < bookingsForShowtime && created < bookingsToCreate; b++)
                {
                    var seatCount = random.Next(1, Math.Min(5, availableSeats.Count));
                    var selectedSeats = availableSeats.OrderBy(_ => random.Next()).Take(seatCount).ToList();
                    if (selectedSeats.Count == 0)
                        break;

                    foreach (var seatId in selectedSeats)
                    {
                        availableSeats.Remove(seatId);
                        bookedSeatsByShowtime[showtime.Id].Add(seatId);
                    }

                    var user = users[random.Next(users.Count)];
                    var isCanceled = random.Next(100) < 8;
                    var total = showtime.TicketPrice * selectedSeats.Count;
                    var bookingDate = showtime.StartTime.AddDays(-random.Next(1, 8));
                    if (bookingDate > DateTime.UtcNow)
                        bookingDate = DateTime.UtcNow.AddHours(-random.Next(2, 48));

                    var booking = new Booking
                    {
                        UserId = user.Id,
                        ShowtimeId = showtime.Id,
                        BookingDate = bookingDate,
                        TotalAmount = total,
                        Status = isCanceled ? BookingStatus.Canceled : BookingStatus.Confirmed
                    };
                    context.Bookings.Add(booking);
                    await context.SaveChangesAsync();

                    foreach (var seatId in selectedSeats)
                    {
                        context.Tickets.Add(new Ticket
                        {
                            BookingId = booking.Id,
                            SeatId = seatId,
                            Price = showtime.TicketPrice
                        });
                    }

                    context.Payments.Add(new Payment
                    {
                        BookingId = booking.Id,
                        Amount = total,
                        PaymentDate = bookingDate,
                        Method = paymentMethods[random.Next(paymentMethods.Length)],
                        Status = isCanceled ? PaymentStatus.Failed : PaymentStatus.Success
                    });

                    await context.SaveChangesAsync();
                    created++;
                }
            }
        }

        private static async Task SeedReviewsAsync(ApplicationDbContext context)
        {
            if (await context.Set<Review>().CountAsync() >= TargetReviewCount)
                return;

            var users = await context.Users.ToListAsync();
            var movies = await context.Movies.ToListAsync();
            if (users.Count == 0 || movies.Count == 0)
                return;

            var random = new Random(456);
            var existingPairs = await context.Set<Review>()
                .Select(r => new { r.MovieId, r.AppUserId })
                .ToListAsync();
            var existingSet = existingPairs
                .Select(p => $"{p.MovieId}:{p.AppUserId}")
                .ToHashSet();

            var reviews = new List<Review>();
            var target = TargetReviewCount - existingPairs.Count;

            foreach (var movie in movies)
            {
                var reviewCount = random.Next(3, 8);
                var shuffledUsers = users.OrderBy(_ => random.Next()).Take(reviewCount);

                foreach (var user in shuffledUsers)
                {
                    if (reviews.Count >= target)
                        break;

                    var key = $"{movie.Id}:{user.Id}";
                    if (existingSet.Contains(key))
                        continue;

                    existingSet.Add(key);
                    reviews.Add(new Review
                    {
                        MovieId = movie.Id,
                        AppUserId = user.Id,
                        Rating = random.Next(3, 6),
                        Comment = ReviewComments[random.Next(ReviewComments.Length)],
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 180))
                    });
                }
            }

            if (reviews.Count > 0)
            {
                context.Set<Review>().AddRange(reviews);
                await context.SaveChangesAsync();
            }
        }
    }
}
