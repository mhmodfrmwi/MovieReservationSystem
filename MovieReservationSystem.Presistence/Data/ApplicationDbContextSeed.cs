using MovieReservationSystem.Domain.Entities.CinemaModule;
using MovieReservationSystem.Domain.Entities.MovieModule;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using MovieReservationSystem.Domain.Entities.BookingModule;
using MovieReservationSystem.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.Presistence.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Genres.Any())
            {
                var genres = new List<Genre>
                {
                    new Genre { Name = "Action" },
                    new Genre { Name = "Comedy" },
                    new Genre { Name = "Drama" },
                    new Genre { Name = "Fantasy" },
                    new Genre { Name = "Horror" },
                    new Genre { Name = "Mystery" },
                    new Genre { Name = "Romance" },
                    new Genre { Name = "Thriller" },
                    new Genre { Name = "Western" }
                };

                context.Genres.AddRange(genres);
                await context.SaveChangesAsync();
            }

            if (!context.Cinemas.Any())
            {
                var cinemas = new List<Cinema>
                {
                    new Cinema { Name = "Cinema City", Location = "New York" },
                    new Cinema { Name = "Royal Cinema", Location = "London" },
                    new Cinema { Name = "Grand Cinema", Location = "Paris" }
                };

                context.Cinemas.AddRange(cinemas);
                await context.SaveChangesAsync();
            }

            if (!context.Halls.Any())
            {
                var cinema = context.Cinemas.FirstOrDefault();
                if(cinema != null)
                {
                    var halls = new List<Hall>
                    {
                        new Hall { Name = "Hall 1", Capacity = 100, CinemaId = cinema.Id },
                        new Hall { Name = "Hall 2", Capacity = 150, CinemaId = cinema.Id }
                    };
                    context.Halls.AddRange(halls);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Seats.Any())
            {
                var hall = context.Halls.FirstOrDefault();
                if(hall != null)
                {
                    var seats = new List<Seat>
                    {
                        new Seat { Row = "A", Number = 1, HallId = hall.Id },
                        new Seat { Row = "A", Number = 2, HallId = hall.Id },
                        new Seat { Row = "B", Number = 1, HallId = hall.Id }
                    };
                    context.Seats.AddRange(seats);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Movies.Any())
            {
                var movies = new List<Movie>
                {
                    new Movie
                    {
                        Title = "Inception",
                        Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                        ReleaseDate = new DateTime(2010, 7, 16),
                        DurationInMinutes = 148,
                        Language = "English",
                        PosterUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRRyuWmayVBvqjd1MxTKpRgauq2cCtUzb7Q9QvaFTkAuxAU_EYMoCE3wBuJeftxIzf0grreIw&s=10",
                        TrailerUrl = "https://youtu.be/YoHD9XEInc0?si=bI3zvec86qv3mEIE"
                    },
                    new Movie
                    {
                        Title = "The Dark Knight",
                        Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                        ReleaseDate = new DateTime(2008, 7, 18),
                        DurationInMinutes = 152,
                        Language = "English",
                        PosterUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTfE_qrYMBZ_JB8om-34WGaZARhpX26yWRttqIDvn4_7l--UzX8mxKcPrc59IcvTpEA_G8gPA&s=10",
                        TrailerUrl = "https://youtu.be/LDG9bisJEaI?si=xd_u4e5EVVCjQGEN"
                    }
                };

                context.Movies.AddRange(movies);
                await context.SaveChangesAsync();
            }

            if (!context.Showtimes.Any())
            {
                var movie = context.Movies.FirstOrDefault();
                var hall = context.Halls.FirstOrDefault();
                if(movie != null && hall != null)
                {
                    var showtimes = new List<Showtime>
                    {
                        new Showtime { StartTime = DateTime.UtcNow.AddDays(1), EndTime = DateTime.UtcNow.AddDays(1).AddMinutes(movie.DurationInMinutes), BasePrice = 10.5m, MovieId = movie.Id, HallId = hall.Id },
                        new Showtime { StartTime = DateTime.UtcNow.AddDays(2), EndTime = DateTime.UtcNow.AddDays(2).AddMinutes(movie.DurationInMinutes), BasePrice = 12.0m, MovieId = movie.Id, HallId = hall.Id }
                    };
                    context.Showtimes.AddRange(showtimes);
                    await context.SaveChangesAsync();
                }
            }
            
            // Note: Users should ideally be seeded using UserManager. Bookings, Tickets and Payments depend on users.
            // As this is a basic seed, we can just seed the data that doesn't strictly require an Identity user, or skip bookings.
        }
    }
}
