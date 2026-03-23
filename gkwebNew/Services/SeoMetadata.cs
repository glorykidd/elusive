namespace gkwebNew.Services;

public record SeoPageData(
    string Title,
    string Description,
    string CanonicalUrl,
    string OgType = "website",
    string? OgImage = null
);

public static class SeoDefaults
{
    public const string SiteName = "GloryKidd Technologies";
    public const string BaseUrl = "https://www.glorykidd.com";
    public const string DefaultOgImage = "https://www.glorykidd.com/images/gk-logo-square.svg";
}
