using Microsoft.Extensions.Hosting;
using System.Diagnostics.Metrics;

namespace Cinema.Management.Otel;

internal sealed class ManagementMeter
{
    private readonly Counter<long> _movieCreatedCounter;
    public const string MeterName = "cinema.management.meters";

    public ManagementMeter(IMeterFactory meterFactory, IHostEnvironment hostEnvironment)
    {
        var meter = meterFactory.Create(MeterName);

        _movieCreatedCounter = meter.CreateCounter<long>(name: "cinema.management.movie.created"
            , description: "Counts the number of movies created"
            , unit: "amount of movies"
            , tags: new List<KeyValuePair<string, object>>
            {
                new("Environment", hostEnvironment.EnvironmentName)
            }!);
    }

    public void MovieCreated(string genre, int releasedYear) => _movieCreatedCounter.Add(1
        , new KeyValuePair<string, object?>("Genre", genre)
        , new KeyValuePair<string, object?>("ReleasedYear", releasedYear));
}
