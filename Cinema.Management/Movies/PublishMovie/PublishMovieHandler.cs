using System.Diagnostics;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Cinema.Management.Models;
using Cinema.Management.Otel;
using Cinema.Management.Persistence;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Cinema.Management.Movies.PublishMovie;

internal class PublishMovieHandler
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly MovieRepository _movieRepository;
    private readonly ManagementMeter _managementMeter;

    public PublishMovieHandler(ServiceBusClient serviceBusClient
        , MovieRepository movieRepository
        , ManagementMeter managementMeter)
    {
        _serviceBusClient = serviceBusClient;
        _movieRepository = movieRepository;
        _managementMeter = managementMeter;
    }

    public async Task<Movie> HandleAsync(MovieDto movieDto, CancellationToken token = default)
    {
        var context = Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current), 
            new Dictionary<string, string[]>(), (headers, key) => headers.TryGetValue(key, out var value) ? value : null);
        
        Baggage.Current = context.Baggage;
        
        using var _ = ApplicationDiagnostics.ActivitySource.StartActivity("PublishMovie", 
            ActivityKind.Server,
            new ActivityContext(),
            links: [new(context.ActivityContext)]);
        
        var movie = new Movie(movieDto.Name, movieDto.Description, movieDto.Genre, movieDto.PosterUri,
            movieDto.ReleaseDate);
        await _movieRepository.AddAsync(movie, token);

        // Publish event
        var movieCreatedEvent = new MovieCreatedEvent(movie.Id, movieDto.Name, movieDto.Description,
            movieDto.Genre, movieDto.PosterUri);

        var sender = _serviceBusClient.CreateSender("movie.management.topic");

        var message = new ServiceBusMessage(JsonSerializer.Serialize(movieCreatedEvent));

        await sender.SendMessageAsync(message, token);

        _managementMeter.MovieCreated(movieDto.Genre, movieDto.ReleaseDate.Year);

        return movie;
    }
}