using Cinema.Reservation.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Reservation.Persistence;

public interface IMovieRepository
{
    Task AddAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<Movie?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Movie>> GetAllAsync(CancellationToken cancellationToken = default);
}

internal sealed class MovieRepository : IMovieRepository
{
    private readonly ReservationContext _context;

    public MovieRepository(ReservationContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await _context.Movies.AddAsync(movie, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Movie?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return movie;
    }
    
    public async Task<IReadOnlyCollection<Movie>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var movies = await _context.Movies.ToListAsync(cancellationToken);
        
        return movies.AsReadOnly();
    }
}