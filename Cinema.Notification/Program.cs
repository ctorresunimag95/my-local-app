using Cinema.Notification.Bus;
using Cinema.Notification.Mail;
using Cinema.Notification.Movies.MoviePublished;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddServiceDefaults();

builder.Services.AddSingleton<BusProcessor>();

builder.Services.AddMailService();

builder.Services.AddScoped<MovieCreatedHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapDefaultEndpoints();

await app.StartBusProcessorAsync();

app.Run();