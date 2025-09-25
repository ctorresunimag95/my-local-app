using Cinema.Management.Movies;
using Cinema.Management.Movies.PublishMovie;
using Cinema.Management.Movies.PullMovieData;
using Cinema.Management.Otel;
using Cinema.Management.Persistence;
using Cinema.ServiceDefaults;
using Microsoft.Extensions.Azure;
using Scalar.AspNetCore;

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
builder.Services.AddMetrics();
builder.Services.AddSingleton<ManagementMeter>();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics => metrics
            .AddMeter(ManagementMeter.MeterName));

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddHttpClient<OmdbService>(client =>
{
    client.BaseAddress =
        new Uri(builder.Configuration.GetValue<string>("omdb:apiUrl")!);
});

builder.Services
    .AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
            ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
        });
builder.Services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.MapMoviesEndpoints();

await app.CreateDbIfNotExistsAsync();

app.Run();