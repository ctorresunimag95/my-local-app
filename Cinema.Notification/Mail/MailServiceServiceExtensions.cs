using System.Net.Mail;

namespace Cinema.Notification.Mail;

public static class MailServiceServiceExtensions
{
    public static IServiceCollection AddMailService(this IServiceCollection services)
    {
        services.AddSingleton(_ =>
        new SmtpClient(
         host: Environment.GetEnvironmentVariable("MAILPIT_HOST") ?? "localhost",
            port: int.TryParse(
            Environment.GetEnvironmentVariable("MAILPIT_PORT"), out var p) ? p : 1025));
        return services.AddScoped<IEmailService, EmailService>();
    }
}