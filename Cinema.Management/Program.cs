using Cinema.Management.Movies.PublishMovie;
using Cinema.Management.Movies.PullMovieData;
using Cinema.Management.Otel;
using Cinema.Management.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Scalar.AspNetCore;
using MovieDto = Cinema.Management.Movies.PublishMovie.MovieDto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAzureClients(bus =>
{
    bus.AddServiceBusClient(builder.Configuration.GetConnectionString("serviceBus"));
});

builder.Services.AddScoped<PublishMovieHandler>();

//builder.Services.AddOtel(builder.Environment);
builder.AddServiceDefaults();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddHttpClient<OmdbService>(client =>
{
    client.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>("omdb:apiUrl")!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.MapPost("/movies/publish", async ([FromBody] MovieDto movieDto,
    [FromServices] PublishMovieHandler handler,
    CancellationToken cancellationToken) =>
{
    var movie = await handler.HandleAsync(movieDto, cancellationToken);

    return Results.Ok(movie);
});

app.MapGet("/movies/search",
    async Task<Results<Ok<MovieResponseDto>, NotFound>> ([FromQuery] string title, OmdbService omdbService,
        CancellationToken cancellationToken) =>
    {
        var movie = await omdbService.GetMovieAsync(title, cancellationToken);

        if (movie is null) return TypedResults.NotFound();

        return TypedResults.Ok(movie);
    });

await app.CreateDbIfNotExistsAsync();

app.Run();