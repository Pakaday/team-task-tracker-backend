# Team Task Tracker — Backend API

A RESTful ASP.NET Core Web API for managing users and tasks, built to practice core backend development patterns: DTO separation, EF Core with PostgreSQL, data validation, and correct HTTP semantics.

## What This Is

This project focuses on getting the fundamentals right for a real-world CRUD API — not just making endpoints that work, but making them work correctly. That means proper DTO design to control what data flows in and out, validation at the API boundary, duplicate detection at both the application and database layer, and returning the right HTTP status codes for every outcome.

## Architecture

```
HTTP Request
     │
     ▼
Controller (validates input via DTO + ModelState)
     │
     ▼
AppDbContext (EF Core)
     │
     ▼
PostgreSQL
```

## Project Structure

```
├── Controllers/
│   ├── TaskItemsController.cs   # CRUD endpoints for tasks
│   └── UsersController.cs       # CRUD endpoints for users
├── DTO/
│   ├── CreateTaskItemDTO.cs     # Input shape for creating a task
│   ├── ReadTaskItemDTO.cs       # Output shape for returning a task
│   ├── CreateUserDTO.cs         # Input shape for creating a user
│   └── ReadUserDTO.cs           # Output shape for returning a user
├── Models/
│   ├── TaskItem.cs              # Domain model with TaskStatus enum
│   └── User.cs                  # Domain model
├── Data/
│   └── AppDbContext.cs          # EF Core context with index and enum config
└── Migrations/                  # EF Core migration history
```

## Key Design Decisions

**DTO separation** — Create and Read DTOs are kept distinct from domain models. This prevents over-posting (callers can't set fields like `CreatedAt` or `Id` directly) and gives full control over what the API exposes. Input validation lives on the Create DTOs, not the domain models.

**Dual-layer duplicate prevention** — Email uniqueness is enforced at two levels: the application checks for an existing email before inserting (`AnyAsync`) and returns a 409 Conflict if found, and the database enforces a unique index on the email column. The app-layer check gives a meaningful error response; the DB constraint is the safety net.

**Enum stored as string** — `TaskStatus` is stored as a string in PostgreSQL rather than an integer. This makes the database human-readable and prevents silent bugs when enum values are reordered.

**EF Core Migrations** — Schema changes are tracked through migrations rather than recreating the database, reflecting how production schema management actually works.

**PostgreSQL** — Uses Npgsql as the EF Core provider rather than SQL Server, chosen to get hands-on with a different database engine.

## API Endpoints

### Tasks

| Method | Route | Description | Response |
|--------|-------|-------------|----------|
| GET | `/api/taskitems` | Get all tasks | 200 |
| GET | `/api/taskitems/{id}` | Get task by ID | 200 / 404 |
| POST | `/api/taskitems` | Create a task | 201 / 400 |

### Users

| Method | Route | Description | Response |
|--------|-------|-------------|----------|
| GET | `/api/users` | Get all users | 200 |
| GET | `/api/users/{id}` | Get user by ID | 200 / 404 |
| POST | `/api/users` | Create a user | 201 / 400 / 409 |

## Task Model

```json
{
  "title": "Fix login bug",
  "description": "Button unresponsive on mobile",
  "dueDate": "2026-05-01",
  "status": "New"
}
```

**Status values:** `New`, `InProgress`, `Completed`

## Running Locally

**Prerequisites:** .NET 8 SDK, PostgreSQL running locally

**1. Update the connection string** in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=tasktracker;Username=postgres;Password=yourpassword"
  }
}
```

**2. Apply migrations:**

```bash
dotnet ef database update
```

**3. Run the API:**

```bash
dotnet run
```

Swagger UI is available at `https://localhost:{port}/swagger`.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 9
- PostgreSQL (Npgsql provider)

## What's Next

This is a work in progress. Planned additions:

- **User-Task relationship** — assign tasks to users with a foreign key and navigation properties
- **PUT / PATCH / DELETE** endpoints for full CRUD
- **Filtering and sorting** on the task list (by status, due date, assigned user)
- **Read DTOs on all GET endpoints** — currently GET returns the domain model directly; this should be consistent with the POST pattern
- **Service layer** — move business logic out of controllers into a service class
- **Unit tests** — test the business logic in isolation

## What I Learned

- Why DTOs matter: separating API shape from domain model gives you control over validation, versioning, and what data gets exposed
- How EF Core migrations track schema changes incrementally — closer to how production database changes are managed
- The difference between app-level and database-level constraint enforcement, and why you need both
- How to return proper HTTP semantics from an ASP.NET Core controller — `CreatedAtAction` for 201, `Conflict` for 409, `NotFound` for 404
