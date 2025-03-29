using Cinema.Reservation.Bus;
using Cinema.Reservation.Cache;
using Cinema.Reservation.Models;
using Cinema.Reservation.Movies.Publish;
using Cinema.Reservation.Otel;
using Scalar.AspNetCore;
using ZiggyCreatures.Caching.Fusion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCache(builder.Configuration);

builder.Services.AddSingleton<BusProcessor>();

builder.Services.AddScoped<MovieCreatedHandler>();

builder.Services.AddOtel(builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("movies/{id:guid}", async (Guid id
    , IFusionCache cache
    , CancellationToken cancellationToken) =>
{
    var movie = await cache.GetOrDefaultAsync($"movies-{id}", defaultValue: default(Movie),
        token: cancellationToken);

    return Results.Ok(movie);
});

await app.StartBusProcessorAsync();

app.Run();