using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Cinema.Management.Otel;

internal static class OtelExtensions
{
    public static IServiceCollection AddOtel(this IServiceCollection services,
        IHostEnvironment hostingEnvironment)
    {
        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(OtelConstants.AppName,
                serviceVersion: "1.0.0",
                serviceNamespace: "Cinema")
            .AddTelemetrySdk()
            .AddAttributes(new Dictionary<string, object>
            {
                ["environment"] =
                    hostingEnvironment.EnvironmentName.ToLowerInvariant()
            });

        services
            .AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(OtelConstants.AppName))
            .WithLogging(logging => { logging.SetResourceBuilder(resourceBuilder); }, options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.ParseStateValues = true;
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(OtelConstants.AppName);
                tracing.SetResourceBuilder(resourceBuilder);

                tracing.AddSource("Azure.*");

                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(resourceBuilder);

                metrics.AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            }).UseOtlpExporter();

        return services;
    }
}