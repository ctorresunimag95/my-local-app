using System.Diagnostics;
using Cinema.Notification.Mail;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Cinema.Notification.Movies.MoviePublished;

public sealed class MovieCreatedHandler
{
    private readonly IEmailService _emailService;
    private readonly ILogger<MovieCreatedHandler> _logger;

    public MovieCreatedHandler(IEmailService emailService, ILogger<MovieCreatedHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task HandleAsync(MovieCreatedEvent @event, CancellationToken cancellationToken)
    {
        var context = Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current), 
            new Dictionary<string, string[]>(), (headers, key) => headers.TryGetValue(key, out var value) ? value : null);
        
        Baggage.Current = context.Baggage;
        
        //Activity.Current = null;
        
        using var activity = ApplicationDiagnostics.ActivitySource.StartActivity("MovieCreatedNotificationSender", 
            ActivityKind.Server,
            new ActivityContext(),
            links: [new(context.ActivityContext)]);
        await _emailService.SendAsync(
            receiver: "subscibers.mail.group@cinema.com",
            subject: $"New movie published: {@event.Name}",
            body: $"A new movie with title {@event.Name} has been published. Check it out!",
            cancellationToken);
    }
}