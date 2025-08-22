using System.Diagnostics;
using Cinema.Management.Otel;

namespace Cinema.Management;

public static class ApplicationDiagnostics
{
    public static readonly ActivitySource ActivitySource = new(OtelConstants.AppName, "1.0.0", [
        new KeyValuePair<string, object?>("service.namespace", "cinema")
    ]);
}