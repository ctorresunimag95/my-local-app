using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Cinema.Management.Models;
using Cinema.Management.Persistence;

namespace Cinema.Management.Movies.PublishMovie;

internal class PublishMovieHandler
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly MovieRepository _movieRepository;

    public PublishMovieHandler(ServiceBusClient serviceBusClient, MovieRepository movieRepository)
    {
        _serviceBusClient = serviceBusClient;
        _movieRepository = movieRepository;
    }

    public async Task<Movie> HandleAsync(MovieDto movieDto, CancellationToken token = default)
    {
        var movie = new Movie(movieDto.Name, movieDto.Description, movieDto.Genre, movieDto.PosterUri,
            movieDto.ReleaseDate);
        await _movieRepository.AddAsync(movie, token);

        // Publish event
        var movieCreatedEvent = new MovieCreatedEvent(movie.Id, movieDto.Name, movieDto.Description,
            movieDto.Genre, movieDto.PosterUri);

        var sender = _serviceBusClient.CreateSender("movie.management.topic");

        var message = new ServiceBusMessage(JsonSerializer.Serialize(movieCreatedEvent));

        await sender.SendMessageAsync(message, token);

        return movie;
    }
}