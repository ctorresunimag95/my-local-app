namespace Cinema.Notification.Mail;

public interface IEmailService
{
    Task SendAsync(string receiver, string subject, string body, CancellationToken cancellationToken = default);
}