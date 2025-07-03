using Cinema.Reservation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Reservation.Persistence;

public class MovieEntityTypeConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.Description);
        
        builder.Property(x => x.Genre)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.PosterUrl)
            .IsRequired();
    }
}