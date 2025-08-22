using System.Diagnostics;
using Cinema.Reservation.Models;
using Cinema.Reservation.Persistence;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
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
        var context = Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current), 
            new Dictionary<string, string[]>(), (headers, key) => headers.TryGetValue(key, out var value) ? value : null);
        
        Baggage.Current = context.Baggage;
        
        //Activity.Current = null;
        
        using var activity = ApplicationDiagnostics.ActivitySource.StartActivity("MovieCreatedReceiver", 
            ActivityKind.Server,
            new ActivityContext(),
            links: [new(context.ActivityContext)]);
        
        await _movieRepository.AddAsync(new Movie(@event.Id, @event.Name, @event.Description, @event.Genre, @event.PosterUri),
            cancellationToken);

        await _cache.RemoveByTagAsync("movies");

        activity?.SetStatus(ActivityStatusCode.Ok);
    }
}