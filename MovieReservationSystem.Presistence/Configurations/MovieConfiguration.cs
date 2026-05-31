using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieReservationSystem.Domain.Entities.MovieModule;

namespace MovieReservationSystem.Presistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {

        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Description).HasMaxLength(1000);
            builder.Property(m => m.ReleaseDate).IsRequired();
            builder.Property(m => m.PosterUrl).HasMaxLength(500);
            builder.Property(m => m.DurationInMinutes).IsRequired();
            builder.Property(m => m.TrailerUrl).HasMaxLength(500);
            builder.Property(m => m.Language).HasMaxLength(50);
            builder.HasMany(m => m.Genres)
                   .WithMany(g => g.Movies)
                   .UsingEntity(static j => j.ToTable("MovieGenres"));
        }
    }
}
