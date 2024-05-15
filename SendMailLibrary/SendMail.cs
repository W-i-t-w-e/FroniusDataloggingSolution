using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Reflection;

namespace SendMailLibrary
{
    public class SendMail : IDisposable
    {
        private readonly string username;
        private readonly string password;
        private readonly string host;
        private readonly int port;
        private readonly string senderEmail;
        private readonly string recipientEmail;
        private readonly ILogger<SendMail> logger;
        private CancellationTokenSource cts;
        public SendMail(ILogger<SendMail> logger, IConfiguration configuration)
        {
            cts = new CancellationTokenSource();
            
            this.username = configuration.GetValue<string>("Mail:Username");
            this.password = configuration.GetValue<string>("Mail:Password");
            this.host = configuration.GetValue<string>("Mail:Host");
            this.port = configuration.GetValue<int>("Mail:Port");
            this.senderEmail = configuration.GetValue<string>("Mail:SenderAddress");
            this.recipientEmail = configuration.GetValue<string>("Mail:RecipientAddress");
            this.logger = logger;
        }
        public void Dispose()
        {
            logger.LogDebug("Disposing SendMail...");
            cts?.Cancel();
            cts?.Dispose();
        }

        public async Task SendEmailAsync(string subject, string text)
        {
            var message = CreateMessage(subject, text);

            using (var client = new SmtpClient())
            {
                logger.LogDebug($"Connecting to the host {host}:{port}...");
                await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable, cts.Token);
                logger.LogDebug($"Authenticate the sender {username}...");
                await client.AuthenticateAsync(username, password, cts.Token);
                logger.LogDebug($"Send the email...");
                await client.SendAsync(message, cts.Token);
                logger.LogDebug("Disconnecting from host...");
                await client.DisconnectAsync(true);
            }

        }

        private MimeMessage CreateMessage(string subject, string body)
        {
            logger.LogDebug("Getting application name...");
            var senderName = Assembly.GetEntryAssembly().GetName().Name;
            logger.LogDebug("Creating new message...");
            var message = new MimeMessage();
            logger.LogDebug($"Adding From name: {senderName} with address: {senderEmail}");
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            logger.LogDebug($"Adding To address: {recipientEmail}");
            message.To.Add(new MailboxAddress("Admin", recipientEmail));
            logger.LogDebug($"Adding Subject: {subject}");
            message.Subject = subject;
            logger.LogDebug($"Adding Body: {body}");
            message.Body = new TextPart("plain") { Text = body };
            return message;
        }
    }
}
