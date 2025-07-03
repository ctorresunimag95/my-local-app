using System.Diagnostics;
using Cinema.Reservation.Models;
using Cinema.Reservation.Persistence;
using ZiggyCreatures.Caching.Fusion;

namespace Cinema.Reservation.Movies.MoviePublished;

internal class MovieCreatedHandler
{
    private readonly IFusionCache _cache;
    private readonly IMovieRepository _movieRepository;

    public MovieCreatedHandler(IFusionCache cache, IMovieRepository movieRepository)
    {
        _cache = cache;
        _movieRepository = movieRepository;
    }

    public async Task HandleAsync(MovieCreatedEvent @event, CancellationToken cancellationToken)
    {
        using var activty = ApplicationDiagnostics.ActivitySource.StartActivity("MovieCreatedReceiver");
        
        await _movieRepository.AddAsync(new Movie(@event.Id, @event.Name, @event.Description, @event.Genre, @event.PosterUri),
            cancellationToken);

        await _cache.RemoveByTagAsync("movies");

        activty?.SetStatus(ActivityStatusCode.Ok);
    }
}