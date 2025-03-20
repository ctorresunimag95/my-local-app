using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Cinema.Reservation.Movies.Publish;

namespace Cinema.Reservation.Bus;

public class BusProcessor : IAsyncDisposable
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ILogger<BusProcessor> _logger;
    private readonly IServiceProvider _serviceProvider;

    public BusProcessor(ILogger<BusProcessor> logger,
        IConfiguration config, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        _serviceBusClient = new ServiceBusClient(config["ServiceBusConnectionString"]);
    }

    public async Task StartProcessorAsync()
    {
        var processor = _serviceBusClient.CreateProcessor("movie.management.topic",
            "movie.management.schedule.subscription", new ServiceBusProcessorOptions
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
            });

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        
        await processor.StartProcessingAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs arg)
    {
        var body = arg.Message.Body.ToString();
        _logger.LogInformation("Received message: {Body}", body);
        
        var @event = JsonSerializer.Deserialize<MovieCreatedEvent>(body);
        
        var handler = _serviceProvider.GetRequiredService<MovieCreatedHandler>();
        
        await handler.HandleAsync(@event!, arg.CancellationToken);

        await arg.CompleteMessageAsync(arg.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "Message handler processing error");
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceBusClient.DisposeAsync();
    }
}