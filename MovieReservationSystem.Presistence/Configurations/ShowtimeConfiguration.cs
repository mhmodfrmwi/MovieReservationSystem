using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieReservationSystem.Domain.Entities.ShowtimeModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Presistence.Configurations
{
    public class ShowtimeConfiguration : IEntityTypeConfiguration<Showtime>
    {
        public void Configure(EntityTypeBuilder<Showtime> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.StartTime).IsRequired();
            builder.Property(s => s.EndTime).IsRequired();
            builder.Property(s => s.BasePrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.HasOne(s => s.Movie)
                   .WithMany(m => m.Showtimes)
                   .HasForeignKey(s => s.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(s => s.Hall)
                   .WithMany(h => h.Showtimes)
                   .HasForeignKey(s => s.HallId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.Bookings)
                   .WithOne(b => b.Showtime)
                   .HasForeignKey(b => b.ShowtimeId)
                   .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
