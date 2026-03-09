# Elusive

The GloryKidd Technologies company website — a client-side Blazor WebAssembly single-page application showcasing the company's software development, consulting services, and client portfolio.

## Application Overview

Elusive serves as the public-facing website for GloryKidd Technologies LLC. The site includes:

- **Home** — Company mission, introduction video, and service highlights
- **Portfolio** — Client testimonials and project showcases
- **Software & Consulting** — Custom software development, security services, workflow optimization, and managed infrastructure offerings
- **About Us** — Company overview with an embedded contact form

## Architecture Overview

The solution (`gkweb.new.sln`) is composed of two projects:

| Project | Type | Purpose |
|---------|------|---------|
| `gkwebNew/` | Blazor WebAssembly App | The website front-end |
| `apiObjects/` | .NET Class Library | Shared API data models (`gkweb.api.types`) |

The Blazor WASM app is purely client-side — there is no server-side rendering. The `wwwroot/index.html` host page loads the Blazor runtime, and all routing is handled client-side via `App.razor` and `MainLayout`.

```
gkwebNew/
  Program.cs          # App entry point and service registration
  App.razor           # Client-side router
  Layout/
    MainLayout.razor   # Page shell and footer
    NavMenu.razor      # Top navigation bar
  Pages/
    Home.razor         # /
    About.razor        # /about
    Portfolio.razor    # /portfolio
    Software.razor     # /software
  wwwroot/             # Static assets, index.html, CSS, images

apiObjects/
  models/
    Staff.cs           # Staff data model
```

## Tech Stack

- **.NET 10** — Target framework
- **Blazor WebAssembly** — Client-side SPA framework
- **Blazor Bootstrap 3.5** — UI component library (Card, Icon, etc.)
- **Bootstrap 5.3** — CSS framework (loaded via CDN)
- **Chart.js / SortableJS** — Included via CDN for Blazor Bootstrap component support

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
dotnet run --project gkwebNew/gkwebNew.csproj
```

The app will be available at **http://localhost:5194** (or **https://localhost:7117**).

## Contributing

1. Fork the repository
2. Create a feature branch from `main` (`git checkout -b feature/your-feature`)
3. Make your changes
4. Ensure the project builds cleanly with `dotnet build gkweb.new.sln`
5. Commit your changes with a clear, descriptive message
6. Push your branch and open a Pull Request against `main`

### Guidelines

- Use **Blazor Bootstrap** components rather than raw Bootstrap HTML when a component exists
- Keep pages as Razor components in `gkwebNew/Pages/`
- Place shared data models in the `apiObjects/` project under the `gkweb.api.types.models` namespace
- Maintain the client-side WASM architecture — do not introduce server-side rendering

## License

This project is licensed under the MIT License — see [LICENSE](LICENSE) for details.

Copyright (c) 2026 GloryKidd Technologies
