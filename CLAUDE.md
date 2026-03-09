# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview
- GloryKidd Technologies company website — a client-side Blazor WebAssembly SPA
- Public-facing marketing site with pages for Home, About, Portfolio, and Software & Consulting Services
- About page embeds a Google Form for contact/inquiries
- `apiObjects/` library defines shared data models intended for API integration (currently `Staff` entity)

## Architecture & Patterns
- **Solution:** `gkweb.new.sln` contains two projects
  - `gkwebNew/` — Blazor WASM app (the website)
  - `apiObjects/` — Class library (`gkweb.api.types`) for shared API data models
- **Routing:** `App.razor` configures the Blazor router; `MainLayout` is the default layout for all pages
- **Layout:** `Layout/MainLayout.razor` provides the page shell and footer; `Layout/NavMenu.razor` handles top navbar with Bootstrap collapse toggle
- **Pages:** Each page is a routable Razor component in `Pages/` — `Home.razor` (`/`), `About.razor` (`/about`), `Portfolio.razor` (`/portfolio`), `Software.razor` (`/software`)
- **Static assets:** `wwwroot/` contains `index.html` (host page that loads the Blazor WASM runtime), CSS, images, and vendored Bootstrap
- **Global imports:** `_Imports.razor` centralizes `@using` directives for all Razor components

## Stack Best Practices
- Target framework is **.NET 10** with nullable reference types and implicit usings enabled
- UI components come from **Blazor Bootstrap** (`BlazorBootstrap` namespace) — use `Card`, `CardBody`, `CardTitle`, `CardText`, `Icon`, etc. rather than raw Bootstrap HTML
- Bootstrap 5.3 is loaded via CDN in `index.html`; vendored copy also exists in `wwwroot/lib/bootstrap/`
- Chart.js and SortableJS are included via CDN for Blazor Bootstrap component support
- Services are registered in `Program.cs` using the standard `WebAssemblyHostBuilder` pattern
- App is purely client-side WASM — no server-side rendering or prerendering

## Anti-Patterns
- Do not add server-side rendering concerns; this is a client-only WASM app
- Do not duplicate Bootstrap CSS references — CDN is already loaded in `index.html`
- Do not use raw HTML Bootstrap markup when a Blazor Bootstrap component exists for the same purpose

## Data Models
- `Staff` (`apiObjects/models/Staff.cs`) — Id (int), Title, Summary, ImageUrl (all nullable strings except Id)
- Models live in the `gkweb.api.types.models` namespace

## Security & Configuration
- Launch profiles in `gkwebNew/Properties/launchSettings.json` define HTTP (port 5194) and HTTPS (port 7117) endpoints
- `ASPNETCORE_ENVIRONMENT` is set to `Development` in launch profiles
- `HttpClient` is registered with the app's base address as its BaseAddress

## Commands & Scripts

```bash
# Restore dependencies
dotnet restore gkweb.new.sln

# Build the solution
dotnet build gkweb.new.sln

# Run the Blazor WASM app (http://localhost:5194 or https://localhost:7117)
dotnet run --project gkwebNew/gkwebNew.csproj
```
