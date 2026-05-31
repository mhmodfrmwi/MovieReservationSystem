using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieReservationSystem.Domain.Entities.BookingModule;

namespace MovieReservationSystem.Presistence.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.BookingDate).IsRequired();
            builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(b => b.Status).IsRequired();
            builder.HasOne(b => b.Payment)
               .WithOne(p => p.Booking)
               .HasForeignKey<Payment>(p => p.BookingId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
