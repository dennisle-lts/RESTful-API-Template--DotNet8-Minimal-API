# RESTful API Template - .NET 8 Minimal API

A production-ready RESTful API template built with .NET 8 Minimal API, featuring basic user endpoint, global exception handling, and MySQL integration with Dapper.

## Features
- **Minimal API** - Lightweight, high-performance endpoint routing
- **Global Exception Handler** - Centralized error handling with RFC 7807 Problem Details
- **Repository Pattern** - Abstracted data access layer for testability
- **Service Layer** - Business logic encapsulation
- **Dapper ORM** - Lightweight, performant micro-ORM for MySQL
- **Swagger/OpenAPI** - Auto-generated API documentation
- **ULID** - Universally Unique Lexicographically Sortable Identifiers
- **Async/Await** - Non-blocking I/O operations throughout
- **Dependency Injection** - Built-in DI container with proper service lifetimes

## Architecture

The project follows a clean, layered architecture:

```
┌─────────────────────────────────────┐
│      Endpoints (Presentation)       │  ← HTTP requests/responses
├─────────────────────────────────────┤
│       Services (Business Logic)     │  ← Validation, business rules
├─────────────────────────────────────┤
│       Repositories (Data Access)    │  ← Database queries
├─────────────────────────────────────┤
│      Infrastructure (Database)      │  ← MySQL connections
└─────────────────────────────────────┘
```

### Design Patterns

- **Repository Pattern** - Abstracts data access logic
- **Factory Pattern** - Database connection factory
- **DTO Pattern** - Separates domain models from API contracts
- **Dependency Injection** - Constructor-based DI throughout
- **Extension Methods** - Clean domain-to-DTO mapping

## Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL 5.7+ or compatible database server

### Configuration

1. Update `appsettings.Development.json` with your database connection:
```json
{
  "ConnectionStrings": {
    "MySQL": "server=localhost;port=3306;database=database_name;user=your_user;password=your_password"
  }
}
```

### Running the Application

```bash
cd RestAPI
dotnet restore
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5001
- Swagger UI: https://localhost:7106/swagger

## Project Structure

```
RestAPI/
├── Contracts/              # Request/Response DTOs
│   ├── Requests/
│   └── Responses/
├── Domain/                 # Core business entities
├── Endpoints/              # Minimal API endpoint definitions
│   └── User/
├── Infrastructure/         # Database connections
│   └── Database/
├── Mapping/                # Domain-to-DTO mappings
├── Middleware/             # Custom middleware (exception handler)
├── Repositories/           # Data access layer
│   ├── Interfaces/
│   └── Implementations/
├── Services/               # Business logic layer
│   ├── Interfaces/
│   └── Implementations/
└── Program.cs              # Application entry point
```

## Error Handling

The API uses a global exception handler that returns standardized RFC 7807 Problem Details responses:

| Exception Type | HTTP Status | Description |
|----------------|-------------|-------------|
| `UnauthorizedAccessException` | 401 | Account disabled or unauthorized |
| `ArgumentException` | 404 | Resource not found |
| `InvalidOperationException` | 400 | Bad request |
| Other exceptions | 500 | Internal server error |

All errors include:
- `type` - URI reference to error type
- `title` - Short, human-readable summary
- `status` - HTTP status code
- `detail` - Detailed error message
- `instance` - URI reference to specific occurrence

## Technologies

| Package | Version | Purpose |
|---------|---------|---------|
| .NET | 8.0 | Runtime framework |
| Dapper | 2.1.66 | Lightweight ORM for SQL queries |
| MySql.Data | 9.5.0 | MySQL database driver |
| Swashbuckle.AspNetCore | 6.6.2 | Swagger/OpenAPI documentation |
| Ulid | 1.4.1 | ULID generation for IDs |

## Development

### Adding a New Endpoint

1. Create request/response DTOs in `Contracts/`
2. Create endpoint in `Endpoints/[Feature]/`
3. Register endpoint mapping in `Program.cs`

Example:
```csharp
app.MapGroup("/api/feature")
   .MapFeatureEndpoints()
   .WithTags("Feature");
```

### Adding a New Service

1. Define interface in `Services/Interfaces/`
2. Implement in `Services/Implementations/`
3. Register in `Program.cs`:
```csharp
builder.Services.AddScoped<IYourService, YourService>();
```

### Adding a New Repository

1. Define interface in `Repositories/Interfaces/`
2. Implement in `Repositories/Implementations/`
3. Register in `Program.cs`:
```csharp
builder.Services.AddScoped<IYourRepository, YourRepository>();
```

## Configuration
### Logging

The application uses .NET's built-in logging. Configure log levels in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## Security Considerations

- SQL injection prevention via Dapper parameterized queries
- HTTPS redirection enabled
- Password stored as hash (never exposed in responses)
- Account status validation (`IsActive` flag)
- Sensitive fields excluded from DTOs

## License

MIT
