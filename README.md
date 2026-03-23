# Elusive

The GloryKidd Technologies company website — a Blazor Web App with Static Server-Side Rendering (SSR) for SEO-friendly, fully-rendered HTML. Showcases the company's software development, consulting services, and client portfolio.

## Application Overview

Elusive serves as the public-facing website for GloryKidd Technologies LLC. The site includes:

- **Home** — Company mission, introduction video, and service highlights
- **Portfolio** — Client testimonials and project showcases
- **Software & Consulting** — Custom software development, security services, workflow optimization, and managed infrastructure offerings
- **About Us** — Company overview with an embedded contact form

## Architecture Overview

The solution (`gkweb.new.sln`) uses a server + class library architecture for Static SSR:

| Project | Type | Purpose |
|---------|------|---------|
| `gkwebNew.Server/` | ASP.NET Core Web App | Server host — renders pages to HTML, serves static assets |
| `gkwebNew/` | Razor Class Library | Pages, layouts, components, and static assets |
| `apiObjects/` | .NET Class Library | Shared API data models (`gkweb.api.types`) |
| `gkwebNew.Tests/` | bUnit Test Project | Unit tests for Razor components |

Pages are rendered on the server as complete HTML — search engines and crawlers receive fully-rendered content on first response with no JavaScript/WASM dependency.

```
gkwebNew.Server/
  Program.cs              # Server entry point and service registration
  Components/
    App.razor             # Root HTML document (head, body, scripts)
    Routes.razor          # Blazor router configuration
  wwwroot/
    sitemap.xml           # SEO sitemap
    robots.txt            # Crawler rules
  web.config              # IIS hosting configuration

gkwebNew/
  Components/
    SeoHead.razor         # Reusable SEO meta tags component
  Services/
    SeoMetadata.cs        # SEO data models and defaults
  Layout/
    MainLayout.razor      # Page shell and footer
    NavMenu.razor         # Top navigation bar
  Pages/
    Home.razor            # /
    About.razor           # /about
    Portfolio.razor       # /portfolio
    Software.razor        # /software
  wwwroot/                # CSS, images, favicon

apiObjects/
  models/
    Staff.cs              # Staff data model
```

## SEO Features

- **Static SSR** — Full HTML rendered server-side; no empty shell for crawlers
- **Per-page meta tags** — Title, description, canonical URL via `SeoHead` component
- **Open Graph tags** — Optimized for social media sharing (Facebook, LinkedIn)
- **Twitter Cards** — Summary large image cards for Twitter/X
- **JSON-LD structured data** — Organization and ProfessionalService schemas
- **sitemap.xml** — All pages listed for search engine discovery
- **robots.txt** — Crawler directives with sitemap reference
- **Semantic HTML** — Proper `<h1>` headings, `<footer>`, `<img>` with `alt` attributes, lazy-loaded iframes

## Tech Stack

- **.NET 10** — Target framework
- **Blazor Web App (Static SSR)** — Server-rendered pages for SEO
- **Blazor Bootstrap 3.5** — UI component library (Card, Icon, etc.)
- **Bootstrap 5.3** — CSS framework (loaded via CDN)
- **IIS** — Production hosting via ASP.NET Core Module v2

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Build & Run

```bash
# Restore dependencies
dotnet restore gkweb.new.sln

# Build the solution
dotnet build gkweb.new.sln

# Run the app
dotnet run --project gkwebNew.Server/gkwebNew.Server.csproj

# Run tests
dotnet test gkwebNew.Tests/gkwebNew.Tests.csproj
```

The app will be available at **http://localhost:5194** (or **https://localhost:7117**).

## Deployment

The app deploys to a self-hosted Windows runner with IIS:

1. `dotnet publish gkwebNew.Server -c Release -o C:/www-root/glorykidd.com`
2. IIS serves as a reverse proxy to the ASP.NET Core Kestrel process
3. `web.config` configures the ASP.NET Core Module v2 for in-process hosting

CI/CD is handled via GitHub Actions workflows:
- `gkes-develop.yml` — Builds on pushes to `develop`
- `gkes.yml` — Builds, tests, and deploys on pushes to `main`

## Contributing

1. Fork the repository
2. Create a feature branch from `develop` (`git checkout -b feature/your-feature`)
3. Make your changes
4. Ensure the project builds cleanly with `dotnet build gkweb.new.sln`
5. Run tests with `dotnet test gkwebNew.Tests/gkwebNew.Tests.csproj`
6. Commit your changes with a clear, descriptive message
7. Push your branch and open a Pull Request against `develop`

### Guidelines

- Use **Blazor Bootstrap** components rather than raw Bootstrap HTML when a component exists
- Keep pages as Razor components in `gkwebNew/Pages/`
- Use the `SeoHead` component on every page with appropriate `SeoPageData`
- Place shared data models in the `apiObjects/` project under the `gkweb.api.types.models` namespace
- Use `<img>` tags with `alt` attributes (not `<image>`)

## License

This project is licensed under the MIT License — see [LICENSE](LICENSE) for details.

Copyright (c) 2026 GloryKidd Technologies
