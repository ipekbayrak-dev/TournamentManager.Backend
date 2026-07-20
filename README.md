# TournamentManager

A double-elimination tournament management system built as a graduation project. Supports bracket progression, team registration, payments, and prize allocation — modeled after the Dota 2 tournament format.

## Tech Stack

- **.NET 10** — ASP.NET Core Web API (backend) + ASP.NET Core MVC (frontend)
- **Entity Framework Core 10** — SQL Server, Fluent API, soft delete, global query filters
- **ASP.NET Core Identity** — user management with roles (Admin, Captain, Player)
- **JWT** — token-based authentication
- **4-layer architecture** — Domain, Application, Infrastructure, Api

## Project Structure

```
TournamentManager/
├── TournamentManager.Backend/
│   ├── Domain/          # Entities, enums, interfaces
│   ├── Application/     # Services, DTOs, repository contracts
│   ├── Infrastructure/  # EF Core, Identity, repository implementations
│   └── Api/             # Controllers, Program.cs
└── TournamentManager.Frontend/
    └── (ASP.NET Core MVC)
```

## Prerequisites

- .NET 10 SDK
- SQL Server
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`

## Getting Started

1. Update the connection string in `Api/appsettings.json`
2. Apply migrations:
   ```
   dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api
   dotnet ef database update --project Infrastructure --startup-project Api
   ```
3. Run the API:
   ```
   dotnet run --project Api
   ```

## License

MIT — see [LICENSE](LICENSE)
