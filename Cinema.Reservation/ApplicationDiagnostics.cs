﻿using System.Diagnostics;
using Cinema.Reservation.Otel;

namespace Cinema.Reservation;

public static class ApplicationDiagnostics
{
    public static readonly ActivitySource ActivitySource = new(OtelConstants.AppName, "1.0.0", [
        new KeyValuePair<string, object?>("service.namespace", "cinema")
    ]);
}