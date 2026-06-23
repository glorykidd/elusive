using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace gkwebNew.Services;

public class RecaptchaService(IConfiguration configuration, ILogger<RecaptchaService> logger)
{
    private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(10) };
    private const string VerifyUrl = "https://www.google.com/recaptcha/api/siteverify";

    public string SiteKey => configuration["Recaptcha:SiteKey"] ?? string.Empty;

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(configuration["Recaptcha:SiteKey"]) &&
        !string.IsNullOrWhiteSpace(configuration["Recaptcha:SecretKey"]) &&
        !(configuration["Recaptcha:SecretKey"]?.Contains("REPLACE_IN_PRODUCTION") ?? false);

    public async Task<bool> VerifyAsync(string token)
    {
        if (!IsConfigured)
        {
            logger.LogWarning("reCAPTCHA is not configured — skipping verification.");
            return true;
        }

        if (string.IsNullOrWhiteSpace(token))
            return false;

        var secretKey = configuration["Recaptcha:SecretKey"]!;

        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("secret", secretKey),
            new KeyValuePair<string, string>("response", token)
        ]);

        try
        {
            var response = await _http.PostAsync(VerifyUrl, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("success").GetBoolean();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "reCAPTCHA verification request failed");
            return false;
        }
    }
}
