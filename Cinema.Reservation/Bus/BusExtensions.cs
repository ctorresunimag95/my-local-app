namespace Cinema.Reservation.Bus;

internal static class BusExtensions
{
    public static async Task StartBusProcessorAsync(this IApplicationBuilder app)
    {
        var processor = app.ApplicationServices.GetRequiredService<BusProcessor>();
        await processor.StartProcessorAsync();
    }
}