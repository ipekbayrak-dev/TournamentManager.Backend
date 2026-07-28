# TournamentManager — Backend

A double-elimination tournament management system built as a graduation project. Modeled after the Dota 2 professional tournament format — supports full bracket progression, team registration, player management, prize pool allocation, and Stripe-ready payments.

## Features

- **Double-elimination bracket** — upper bracket, lower bracket, grand final
- **JWT authentication** with refresh tokens and role-based access (Admin, Captain, Player)
- **Full CRUD** for Tournaments, Teams, Players, Matches, Prize Pools, Prize Allocations, Tournament Entries, and Payments
- **Soft delete** via global EF Core query filters — nothing is permanently removed
- **FluentValidation** on all critical create endpoints
- **Scalar UI** for interactive API documentation
- **DB seeding** — admin account and roles created automatically on first run

## Tech Stack

| Layer | Technology |
|---|---|
| API | ASP.NET Core Web API (.NET 10) |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| Auth | ASP.NET Core Identity + JWT |
| Validation | FluentValidation |
| API Docs | Scalar (OpenAPI 3.1) |
| Architecture | 4-layer (Domain / Application / Infrastructure / Api) |

## Project Structure

```
TournamentManager.Backend/
├── Domain/
│   ├── Entities/          # ApplicationUser, Team, Player, Tournament, Match,
│   │                      #   TournamentEntry, Prize, PrizeAllocation, Payment
│   ├── Enums/             # BracketType, DotaPosition, DotaRegion,
│   │                      #   MatchStatus, TournamentStatus, PaymentStatus
│   └── Common/            # BaseEntity (Id, CreatedAt, UpdatedAt, DeletedAt)
│
├── Application/
│   ├── Dtos/              # Request/Response DTOs per entity
│   ├── Features/          # Service implementations
│   ├── Interfaces/        # IRepository<T>, IService contracts
│   ├── Validators/        # FluentValidation validators
│   └── Common/            # Result<T>, Result, Roles, ValidationExtensions
│
├── Infrastructure/
│   ├── Persistence/
│   │   ├── Common/        # EFRepositoryBase<T> (virtual ApplyIncludes hook)
│   │   ├── Configurations/# Fluent API entity configurations
│   │   └── Repositories/  # TeamRepository, PrizeRepository, TournamentRepository, ...
│   ├── Common/            # AppDbContext, Paginate<T>
│   └── Services/          # TokenService, infrastructure service implementations
│
└── Api/
    ├── Controllers/       # AuthController + 8 CRUD controllers
    ├── DbSeeder.cs        # Seeds roles and default admin on startup
    ├── BearerSecuritySchemeTransformer.cs
    └── Program.cs
```

## Architecture Notes

- **Result\<T\> pattern** — all services return `Result<T>` or `Result`. Controllers check `IsSuccess`, then `Data is null` for 404, then return 200.
- **ApplyIncludes hook** — `EFRepositoryBase<T>` exposes `protected virtual IQueryable<T> ApplyIncludes(IQueryable<T>)`. Repositories that need eager loading (Team → Players, Prize → Allocations, Tournament → Matches + Entries) override this single method.
- **Soft delete** — `DeletedAt` timestamp on `BaseEntity`, filtered globally via `HasQueryFilter`. Pass `withDeleted: true` to bypass.
- **Roles** — `Admin` can write everything. `Player` can read and register. `Captain` reserved for future team management features.

## API Endpoints

| Group | Endpoints |
|---|---|
| Auth | POST /register, POST /login, POST /refresh |
| Tournament | GET all, GET by id, POST, PUT, DELETE |
| Team | GET all, GET by id, POST, PUT, DELETE |
| Player | GET by team, GET by id, POST, PUT, DELETE |
| Match | GET by tournament, GET by id, POST, PUT, DELETE |
| TournamentEntry | GET by tournament, GET by id, POST, PUT, DELETE |
| Prize | GET by tournament, GET by id, POST, PUT, DELETE |
| PrizeAllocation | GET by id, POST, PUT, DELETE |
| Payment | GET by id, POST, PUT, DELETE |

All endpoints except Auth require a valid JWT Bearer token. Write operations (POST/PUT/DELETE) require the `Admin` role.

## Prerequisites

- .NET 10 SDK
- SQL Server
- EF Core tools: `dotnet tool install --global dotnet-ef`

## Getting Started

**1. Configure the connection string** in `Api/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TournamentManager;..."
}
```

**2. Apply migrations** from the solution root:
```
dotnet ef migrations add InitialCreate --project TournamentManager.Backend/Infrastructure --startup-project TournamentManager.Backend/Api

dotnet ef database update --project TournamentManager.Backend/Infrastructure --startup-project TournamentManager.Backend/Api
```

**3. Run the API:**
```
dotnet run --project TournamentManager.Backend/Api
```

**4. Open Scalar UI:**
```
http://localhost:5147/scalar/v1
```

The default admin account (`hello@ipekbayrak.dev`) and all roles are seeded automatically on first run.

## Default Credentials

| Role | Email | Password |
|---|---|---|
| Admin | hello@ipekbayrak.dev | Admin1234$ |

## License

MIT — see [LICENSE](LICENSE)
