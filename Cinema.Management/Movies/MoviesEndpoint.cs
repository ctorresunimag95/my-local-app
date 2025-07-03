using Cinema.Management.Models;
using Cinema.Management.Movies.PublishMovie;
using Cinema.Management.Movies.PullMovieData;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Management.Movies;

public static class MoviesEndpoint
{
    public static void MapMoviesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("movies");

        group.MapPost("/publish", PublishMovieAsync);

        group.MapGet("/search", SearchMovieAsync);
    }

    private static async Task<Results<Ok<Movie>, ProblemHttpResult>> PublishMovieAsync([FromBody] MovieDto movieDto,
        [FromServices] PublishMovieHandler handler,
        CancellationToken cancellationToken)
    {
        var movie = await handler.HandleAsync(movieDto, cancellationToken);

        return TypedResults.Ok(movie);
    }

    private static async Task<Results<Ok<MovieResponseDto>, NotFound>> SearchMovieAsync([FromQuery] string title,
        OmdbService omdbService,
        CancellationToken cancellationToken)
    {
        var movie = await omdbService.GetMovieAsync(title, cancellationToken);

        if (movie is null) return TypedResults.NotFound();

        return TypedResults.Ok(movie);
    }
}