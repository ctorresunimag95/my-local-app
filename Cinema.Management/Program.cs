using Cinema.Management.Movies.Publish;
using Cinema.Management.Otel;
using Cinema.Management.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAzureClients(bus =>
{
    bus.AddServiceBusClient(builder.Configuration.GetConnectionString("service-bus"));
});

builder.Services.AddScoped<PublishMovieHandler>();

builder.Services.AddOtel(builder.Environment);

builder.Services.AddPersistence(builder.Configuration);

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

await app.CreateDbIfNotExistsAsync();

app.Run();