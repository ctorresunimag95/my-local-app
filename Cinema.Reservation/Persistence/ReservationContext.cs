using Cinema.Reservation.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Reservation.Persistence;

public class ReservationContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    
    public ReservationContext(DbContextOptions<ReservationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReservationContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}