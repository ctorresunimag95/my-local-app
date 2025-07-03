using Cinema.Reservation.Models;
using Cinema.Reservation.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;

namespace Cinema.Reservation.Movies;

public static class MoviesEndpoints
{
    public static void MapMoviesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("movies");
        
        group.MapGet("{id:guid}", GetMovieByIdAsync);

        group.MapGet("", GetMoviesAsync);
    }

    private static async Task<Results<Ok<Movie>, NotFound>> GetMovieByIdAsync(Guid id
        , IFusionCache cache
        , IMovieRepository repository
        , CancellationToken cancellationToken)
    {
        var movie = await cache.GetOrSetAsync<Movie?>($"movies_{id}"
            , factory: async (context, token) =>
            {
                var movie = await repository.GetAsync(id, token);

                context.Options.Duration = TimeSpan.FromMinutes(30);
                context.Options.DistributedCacheDuration = TimeSpan.FromMinutes(60);

                return movie;
            }
            , tags: ["movies"]
            , token: cancellationToken);

        return movie is not null
            ? TypedResults.Ok(movie)
            : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<IReadOnlyCollection<Movie>>, ProblemHttpResult>> GetMoviesAsync(IFusionCache cache
        , IMovieRepository repository
        , CancellationToken cancellationToken)
    {
        var movies = await cache.GetOrSetAsync<IReadOnlyCollection<Movie>>("movies"
            , factory: async (context, token) =>
            {
                var movies = await repository.GetAllAsync(token);

                context.Options.Duration = TimeSpan.FromMinutes(30);
                context.Options.DistributedCacheDuration = TimeSpan.FromMinutes(60);

                return movies;
            }
            , tags: ["movies"]
            , token: cancellationToken);

        return TypedResults.Ok(movies);
    }
}