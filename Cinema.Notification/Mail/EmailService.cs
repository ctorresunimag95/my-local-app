using System.Net.Mail;

namespace Cinema.Notification.Mail;

public sealed class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<EmailService> _logger;

    public EmailService(SmtpClient smtpClient, ILogger<EmailService> logger)
    {
        _smtpClient = smtpClient;
        _logger = logger;
    }

    public async Task SendAsync(string receiver, string subject, string body, CancellationToken cancellationToken = default)
    {
        using var message = new MailMessage(from: "no-reply@cinema.com"
            , to: receiver
            , subject: subject
            , body: body);
        await _smtpClient.SendMailAsync(message, cancellationToken);
    }
}