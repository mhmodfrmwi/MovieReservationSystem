using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MovieReservationSystem.Services_Abstraction.Interfaces;

namespace MovieReservationSystem.Services.Services
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(
                configuration["EmailSettings:SenderName"],
                configuration["EmailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = message };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                configuration["EmailSettings:SmtpServer"],
                int.Parse(configuration["EmailSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                configuration["EmailSettings:SenderEmail"],
                configuration["EmailSettings:Password"]);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
