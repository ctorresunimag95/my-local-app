using System.Diagnostics;
using Cinema.Reservation.Models;
using ZiggyCreatures.Caching.Fusion;

namespace Cinema.Reservation.Movies.MoviePublished;

internal class MovieCreatedHandler
{
    private readonly IFusionCache _cache;

    public MovieCreatedHandler(IFusionCache cache)
    {
        _cache = cache;
    }

    public async Task HandleAsync(MovieCreatedEvent @event, CancellationToken cancellationToken)
    {
        using var activty = ApplicationDiagnostics.ActivitySource.StartActivity("MovieCreatedReceiver");
        
        await _cache.GetOrSetAsync($"movies-{@event.Id}", async _ =>
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            
            var movie = new Movie(@event.Id, @event.Name, @event.Description, @event.Genre);

            return movie;
        }, options: new FusionCacheEntryOptions
        {
            DistributedCacheDuration = TimeSpan.FromHours(12)
        },token: cancellationToken);

        activty?.SetStatus(ActivityStatusCode.Ok);
    }
}