# Password Validator API

A .NET 9 Web API for validating passwords and storing validation attempts in SQL Server.

## Features

-   Password validation with multiple rules
-   Secure password hashing (SHA256)
-   SQL Server database storage
-   RESTful API endpoints
-   CORS enabled for frontend integration

## Prerequisites

-   .NET 9 SDK
-   Docker (for SQL Server)
-   VS Code with C# extension (recommended)

## Getting Started

### 1. Start SQL Server (Docker)

```bash
docker run -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=YourPassword123!" \
   -p 1434:1433 --name sqlserver-password-validator \
   -d mcr.microsoft.com/azure-sql-edge
```

### 2. Configure User Secrets

```bash
cd PasswordValidatorAPI
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1434;Database=PasswordValidatorDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True;"
```

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Run Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the API

```bash
dotnet run
```

The API will start on `https://localhost:5001` or `http://localhost:5000`.

## Project Structure

```
PasswordValidatorAPI/
├── Controllers/
│   └── PasswordController.cs    # API endpoints
├── Data/
│   └── AppDbContext.cs          # Entity Framework DbContext
├── Models/
│   └── ValidationAttempt.cs     # Database model
├── Dtos/
│   └── PasswordRequestDto.cs    # Data transfer objects
├── Services/
│   └── PasswordService.cs       # Password validation logic
├── Migrations/                  # EF Core migrations
└── Program.cs                   # Application entry point
```

## API Endpoints

### POST `/api/password/validate`

Validates a password and stores the attempt in the database.

**Request Body:**

```json
{
    "password": "MyPassword123!"
}
```

**Response:**

```json
{
    "isValid": true,
    "errors": []
}
```

Or if invalid:

```json
{
    "isValid": false,
    "errors": ["Password must be at least 8 characters long", "Password must contain at least one special character"]
}
```

## Password Validation Rules

-   Minimum 8 characters
-   At least one uppercase letter
-   At least one lowercase letter
-   At least one number
-   At least one special character

## Database Schema

### ValidationAttempts Table

| Column           | Type          | Description                        |
| ---------------- | ------------- | ---------------------------------- |
| Id               | int           | Primary key (auto-increment)       |
| PasswordHash     | nvarchar(max) | SHA256 hash of password            |
| IsValid          | bit           | Whether password passed validation |
| ValidationErrors | nvarchar(max) | Comma-separated error messages     |
| Timestamp        | datetime2     | When validation occurred (UTC)     |

## Technologies Used

-   **ASP.NET Core 9.0** - Web API framework
-   **Entity Framework Core 9.0** - ORM
-   **SQL Server (Azure SQL Edge)** - Database
-   **Docker** - Container platform
-   **SHA256** - Password hashing

## Security Notes

-   Passwords are hashed using SHA256 before storage
-   User secrets are used for local development (connection strings)
-   CORS is configured to allow requests from `http://localhost:3000` (React app)

## Common Commands

```bash
# Run the API
dotnet run

# Build the project
dotnet build

# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# List user secrets
dotnet user-secrets list

# Stop SQL Server container
docker stop sqlserver-password-validator

# Start SQL Server container
docker start sqlserver-password-validator
```

## Troubleshooting

### Cannot connect to database

-   Ensure Docker container is running: `docker ps`
-   Verify connection string in user secrets: `dotnet user-secrets list`
-   Check SQL Server password matches between Docker and user secrets

### Migration errors

-   Delete the `Migrations` folder and `obj` folder
-   Run `dotnet ef migrations add InitialCreate` again
-   Run `dotnet ef database update`

### CORS errors from frontend

-   Ensure `app.UseCors("AllowReactApp")` is in `Program.cs`
-   Verify React app is running on `http://localhost:3000`

## License

This project is for educational purposes.
