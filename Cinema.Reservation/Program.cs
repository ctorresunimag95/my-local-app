using Cinema.Reservation.Bus;
using Cinema.Reservation.Cache;
using Cinema.Reservation.Movies;
using Cinema.Reservation.Movies.MoviePublished;
using Cinema.Reservation.Persistence;
using Cinema.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCache(builder.Configuration);

builder.Services.AddSingleton<BusProcessor>();

builder.Services.AddScoped<MovieCreatedHandler>();

//builder.Services.AddOtel(builder.Environment);
builder.AddServiceDefaults();

builder.Services
    .AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
            ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
        });
builder.Services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();

builder.Services.AddDbContext<ReservationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("database")));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.MapMoviesEndpoints();

await app.StartBusProcessorAsync();

app.Run();