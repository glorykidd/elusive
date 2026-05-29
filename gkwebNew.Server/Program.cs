using gkwebNew.Data;
using gkwebNew.Server.Components;
using gkwebNew.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContactDb")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.AccessDeniedPath = "/admin/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<EmailService>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapPost("/admin/do-login", async (HttpContext ctx, IConfiguration config) =>
{
    var form = await ctx.Request.ReadFormAsync();
    var username = form["username"].ToString();
    var password = form["password"].ToString();

    var configUsername = config["AdminAuth:Username"] ?? string.Empty;
    var configPassword = config["AdminAuth:Password"] ?? string.Empty;

    var usernameMatch = CryptographicOperations.FixedTimeEquals(
        System.Text.Encoding.UTF8.GetBytes(username),
        System.Text.Encoding.UTF8.GetBytes(configUsername));
    var passwordMatch = CryptographicOperations.FixedTimeEquals(
        System.Text.Encoding.UTF8.GetBytes(password),
        System.Text.Encoding.UTF8.GetBytes(configPassword));

    if (!usernameMatch || !passwordMatch)
        return Results.Redirect("/admin/login?error=1");

    var claims = new List<Claim>
    {
        new(ClaimTypes.Name, username),
        new(ClaimTypes.Role, "Admin")
    };
    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

    return Results.Redirect("/admin");
});

app.MapGet("/admin/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
}).RequireAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(gkwebNew.Pages.Home).Assembly);

app.Run();
