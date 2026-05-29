using gkweb.api.types.models;
using gkwebNew.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace gkwebNew.Tests.Services;

public class EmailServiceTests
{
    private static ContactSubmission MakeSubmission(string? phone = "555-1234") => new()
    {
        Id = 7,
        Name = "Jane Doe",
        Email = "jane@example.com",
        Phone = phone,
        Message = "I'd love to learn more about your services.",
        SubmittedAt = new DateTime(2026, 5, 1, 14, 0, 0, DateTimeKind.Utc)
    };

    private static EmailService MakeService(Dictionary<string, string?>? settings = null)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(settings ?? [])
            .Build();
        return new EmailService(config, NullLogger<EmailService>.Instance);
    }

    // --- BuildAdminNotificationHtml ---

    [Fact]
    public void BuildAdminNotificationHtml_ContainsName()
    {
        var html = EmailService.BuildAdminNotificationHtml(MakeSubmission());
        Assert.Contains("Jane Doe", html);
    }

    [Fact]
    public void BuildAdminNotificationHtml_ContainsEmail()
    {
        var html = EmailService.BuildAdminNotificationHtml(MakeSubmission());
        Assert.Contains("jane@example.com", html);
    }

    [Fact]
    public void BuildAdminNotificationHtml_ContainsPhone()
    {
        var html = EmailService.BuildAdminNotificationHtml(MakeSubmission());
        Assert.Contains("555-1234", html);
    }

    [Fact]
    public void BuildAdminNotificationHtml_ContainsMessage()
    {
        var html = EmailService.BuildAdminNotificationHtml(MakeSubmission());
        Assert.Contains("love to learn more about your services", html);
    }

    [Fact]
    public void BuildAdminNotificationHtml_ContainsAdminLink()
    {
        var html = EmailService.BuildAdminNotificationHtml(MakeSubmission());
        Assert.Contains("/admin/contacts/7", html);
    }

    [Fact]
    public void BuildAdminNotificationHtml_ShowsDashForNullPhone()
    {
        var html = EmailService.BuildAdminNotificationHtml(MakeSubmission(phone: null));
        Assert.Contains("—", html);
    }

    [Fact]
    public void BuildAdminNotificationHtml_HtmlEncodesSpecialChars()
    {
        var submission = MakeSubmission();
        submission.Name = "<script>alert('xss')</script>";
        var html = EmailService.BuildAdminNotificationHtml(submission);
        Assert.DoesNotContain("<script>", html);
        Assert.Contains("&lt;script&gt;", html);
    }

    [Fact]
    public void BuildAdminNotificationHtml_IsValidHtmlStructure()
    {
        var html = EmailService.BuildAdminNotificationHtml(MakeSubmission());
        Assert.StartsWith("<!DOCTYPE html>", html.TrimStart());
        Assert.Contains("</html>", html);
    }

    // --- SendAdminNotificationAsync (skips gracefully when not configured) ---

    [Fact]
    public async Task SendAdminNotificationAsync_SkipsWhenNoAdminEmailConfigured()
    {
        var svc = MakeService();
        // No SMTP config — should return without throwing
        await svc.SendAdminNotificationAsync(MakeSubmission());
    }

    [Fact]
    public async Task SendAdminNotificationAsync_SkipsWhenAdminEmailIsWhitespace()
    {
        var svc = MakeService(new Dictionary<string, string?>
        {
            ["Email:AdminNotificationAddress"] = "   "
        });
        await svc.SendAdminNotificationAsync(MakeSubmission());
    }
}
