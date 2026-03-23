# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview
- GloryKidd Technologies company website — a Blazor Web App with Static SSR for SEO-friendly server-rendered HTML
- Public-facing marketing site with pages for Home, About, Portfolio, and Software & Consulting Services
- About page embeds a Google Form for contact/inquiries
- `apiObjects/` library defines shared data models intended for API integration (currently `Staff` entity)

## Architecture & Patterns
- **Solution:** `gkweb.new.sln` contains four projects
  - `gkwebNew.Server/` — ASP.NET Core server host (entry point, Static SSR rendering)
  - `gkwebNew/` — Razor class library (pages, layouts, components, static assets)
  - `apiObjects/` — Class library (`gkweb.api.types`) for shared API data models
  - `gkwebNew.Tests/` — bUnit test project
- **Root document:** `gkwebNew.Server/Components/App.razor` is the HTML shell (replaces the old `index.html`)
- **Routing:** `gkwebNew.Server/Components/Routes.razor` configures the Blazor router; `MainLayout` is the default layout
- **Layout:** `gkwebNew/Layout/MainLayout.razor` provides the page shell and footer; `gkwebNew/Layout/NavMenu.razor` handles top navbar
- **Pages:** Routable Razor components in `gkwebNew/Pages/` — `Home.razor` (`/`), `About.razor` (`/about`), `Portfolio.razor` (`/portfolio`), `Software.razor` (`/software`)
- **SEO:** `gkwebNew/Components/SeoHead.razor` renders per-page meta tags, Open Graph, Twitter Cards, and JSON-LD structured data via `<HeadContent>`
- **Static assets:** `gkwebNew/wwwroot/` contains CSS, images; referenced via `_content/gkwebNew/` paths in the server project
- **Global imports:** `_Imports.razor` centralizes `@using` directives for all Razor components

## Stack Best Practices
- Target framework is **.NET 10** with nullable reference types and implicit usings enabled
- **Static SSR** — pages render fully on the server for SEO; no WASM runtime downloaded
- UI components come from **Blazor Bootstrap** (`BlazorBootstrap` namespace) — use `Card`, `CardBody`, `CardTitle`, `CardText`, `Icon`, etc. rather than raw Bootstrap HTML
- Bootstrap 5.3 is loaded via CDN in `App.razor`
- Services are registered in `gkwebNew.Server/Program.cs` using `WebApplication.CreateBuilder`
- Each page uses the `SeoHead` component with `SeoPageData` for SEO metadata

## Anti-Patterns
- Do not duplicate Bootstrap CSS references — CDN is already loaded in `App.razor`
- Do not use raw HTML Bootstrap markup when a Blazor Bootstrap component exists for the same purpose
- Do not use `<image>` tags — use `<img>` with `alt` attributes

## Data Models
- `Staff` (`apiObjects/models/Staff.cs`) — Id (int), Title, Summary, ImageUrl (all nullable strings except Id)
- Models live in the `gkweb.api.types.models` namespace

## Security & Configuration
- Launch profiles in `gkwebNew.Server/Properties/launchSettings.json` define HTTP (port 5194) and HTTPS (port 7117) endpoints
- `ASPNETCORE_ENVIRONMENT` is set to `Development` in launch profiles
- IIS hosting configured via `gkwebNew.Server/web.config` (ASP.NET Core Module v2, InProcess)

## Commands & Scripts

```bash
# Restore dependencies
dotnet restore gkweb.new.sln

# Build the solution
dotnet build gkweb.new.sln

# Run the app (http://localhost:5194 or https://localhost:7117)
dotnet run --project gkwebNew.Server/gkwebNew.Server.csproj

# Run tests
dotnet test gkwebNew.Tests/gkwebNew.Tests.csproj
```
