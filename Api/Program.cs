using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TournamentManager.Api;
using TournamentManager.Application;
using TournamentManager.Application.Validators;
using TournamentManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// TODO: replace AllowAll with specific frontend URL once frontend is ready
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<SignUpRequestValidator>();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

await DbSeeder.SeedAsync(app);

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Tournament Manager API</title>
  <style>
    :root {
      --purple: #6a0dad;
      --purple-light: #8b2fc9;
      --purple-dark: #4a007a;
      --gold: #c9a84c;
      --gold-light: #e0c068;
      --bg: #0f0a1a;
      --surface: #1a1030;
      --surface-2: #241640;
      --text: #f0eaf8;
      --muted: #9e8fbf;
    }
    *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
    body {
      background: var(--bg);
      color: var(--text);
      font-family: 'Segoe UI', system-ui, sans-serif;
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 2rem;
    }
    .card {
      background: var(--surface);
      border: 1px solid var(--purple-dark);
      border-radius: 1rem;
      padding: 3rem 2.5rem;
      max-width: 520px;
      width: 100%;
      text-align: center;
    }
    .icon { font-size: 3.5rem; margin-bottom: 1rem; }
    h1 {
      font-size: 1.75rem;
      font-weight: 800;
      margin-bottom: 0.5rem;
    }
    h1 span {
      background: linear-gradient(135deg, var(--gold), var(--gold-light));
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
      background-clip: text;
    }
    .subtitle {
      color: var(--muted);
      font-size: 0.95rem;
      margin-bottom: 2rem;
      line-height: 1.6;
    }
    .divider {
      height: 1px;
      background: linear-gradient(90deg, transparent, var(--gold), transparent);
      opacity: 0.4;
      margin: 1.75rem 0;
    }
    .badge-row {
      display: flex;
      justify-content: center;
      gap: 0.75rem;
      flex-wrap: wrap;
      margin-bottom: 2rem;
    }
    .badge {
      background: var(--surface-2);
      border: 1px solid var(--purple-dark);
      color: var(--muted);
      font-size: 0.75rem;
      padding: 0.3rem 0.75rem;
      border-radius: 999px;
    }
    .btn {
      display: inline-block;
      background: linear-gradient(135deg, var(--purple), var(--purple-light));
      color: #fff;
      text-decoration: none;
      font-weight: 600;
      padding: 0.75rem 2rem;
      border-radius: 0.5rem;
      font-size: 0.95rem;
      transition: opacity 0.15s;
    }
    .btn:hover { opacity: 0.85; }
    .footer {
      margin-top: 2rem;
      color: var(--muted);
      font-size: 0.8rem;
    }
    .footer a { color: var(--gold); text-decoration: none; }
    .footer a:hover { text-decoration: underline; }
  </style>
</head>
<body>
  <div class="card">
    <div class="icon">⚔</div>
    <h1>Tournament Manager <span>API</span></h1>
    <p class="subtitle">
      REST API for Dota 2 double-elimination tournaments.<br />
      Manage teams, players, brackets, matches, and prize pools.
    </p>
    <div class="badge-row">
      <span class="badge">.NET 10</span>
      <span class="badge">ASP.NET Core</span>
      <span class="badge">JWT Auth</span>
      <span class="badge">EF Core</span>
      <span class="badge">Double Elimination</span>
    </div>
    <div class="divider"></div>
    <a href="/scalar/v1" class="btn">View API Docs →</a>
    <div class="footer">
      <p style="margin-top:1rem">Built by <a href="https://ipekbayrak.dev">İpek Bayrak</a></p>
    </div>
  </div>
</body>
</html>
""", "text/html"));


app.Run();