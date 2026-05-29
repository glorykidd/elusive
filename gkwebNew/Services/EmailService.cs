using gkweb.api.types.models;
using gkwebNew.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace gkwebNew.Services;

public class EmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendAdminNotificationAsync(ContactSubmission submission)
    {
        var adminEmail = _config["Email:AdminNotificationAddress"];
        if (string.IsNullOrWhiteSpace(adminEmail))
            return;

        var subject = $"New Contact Submission: {submission.Name}";
        var body = BuildAdminNotificationHtml(submission);

        await SendAsync(adminEmail, "GloryKidd Admin", subject, body);
    }

    private async Task SendAsync(string toAddress, string toName, string subject, string htmlBody)
    {
        var fromAddress = _config["Email:FromAddress"];
        var fromName = _config["Email:FromName"] ?? "GloryKidd Technologies";
        var host = _config["Email:SmtpHost"];
        var port = int.TryParse(_config["Email:SmtpPort"], out var p) ? p : 587;
        var username = _config["Email:Username"];
        var password = _config["Email:Password"];

        if (string.IsNullOrWhiteSpace(fromAddress) || string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Email not configured — skipping send to {To}", toAddress);
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromAddress));
        message.To.Add(new MailboxAddress(toName, toAddress));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", toAddress);
        }
    }

    internal static string BuildAdminNotificationHtml(ContactSubmission submission)
    {
        var phone = string.IsNullOrWhiteSpace(submission.Phone) ? "—" : submission.Phone;
        var submittedAt = submission.SubmittedAt.ToLocalTime().ToString("f");

        return $"""
            <!DOCTYPE html>
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; color: #333;">
              <div style="background: #333333; padding: 20px; text-align: center;">
                <h1 style="color: #6fff00; margin: 0; font-size: 20px;">New Contact Submission</h1>
              </div>
              <div style="padding: 24px; background: #f9f9f9;">
                <table style="width: 100%; border-collapse: collapse;">
                  <tr><td style="padding: 8px 0; font-weight: bold; width: 120px;">Name</td><td style="padding: 8px 0;">{Encode(submission.Name)}</td></tr>
                  <tr><td style="padding: 8px 0; font-weight: bold;">Email</td><td style="padding: 8px 0;"><a href="mailto:{Encode(submission.Email)}">{Encode(submission.Email)}</a></td></tr>
                  <tr><td style="padding: 8px 0; font-weight: bold;">Phone</td><td style="padding: 8px 0;">{Encode(phone)}</td></tr>
                  <tr><td style="padding: 8px 0; font-weight: bold; vertical-align: top;">Message</td><td style="padding: 8px 0; white-space: pre-wrap;">{Encode(submission.Message)}</td></tr>
                  <tr><td style="padding: 8px 0; font-weight: bold;">Submitted</td><td style="padding: 8px 0;">{submittedAt}</td></tr>
                </table>
              </div>
              <div style="padding: 16px; text-align: center; background: #eeeeee; font-size: 12px; color: #666;">
                GloryKidd Technologies &middot; <a href="{SeoDefaults.BaseUrl}/admin/contacts/{submission.Id}">View in Admin</a>
              </div>
            </body>
            </html>
            """;
    }

    private static string Encode(string? value) =>
        System.Net.WebUtility.HtmlEncode(value ?? "");
}
