using EcomMakeUp.Servies;
using System.Net.Mail;
using System.Net;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        // Getting SMTP settings from configuration
        var smtpHost = _configuration["Smtp:Host"];
        var smtpPort = int.Parse(_configuration["Smtp:Port"]);
        var smtpUsername = _configuration["Smtp:Username"];
        var smtpPassword = _configuration["Smtp:Password"];
        var fromEmail = _configuration["Smtp:FromEmail"];

        var smtpClient = new SmtpClient(smtpHost)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
