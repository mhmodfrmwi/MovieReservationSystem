using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieReservationSystem.Domain.Entities.BookingModule;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieReservationSystem.Presistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasOne(t => t.Booking)
                   .WithMany(b => b.Tickets)
                   .HasForeignKey(t => t.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(t => t.Seat)
                   .WithMany()
                   .HasForeignKey(t => t.SeatId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
