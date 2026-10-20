# RaceDay API - Event Management System

## Overview

RaceDay is a full-stack web-based event management system designed specifically for the South African road running, walking, and cycling community. This RESTful API backend provides the foundation for managing events, participants, enrolments, and results.

The platform supports two distinct user roles:
- **Organiser**: Can create, edit, and delete events, manage event categories, capture participant results, and view all event enrolments
- **Participant**: Can create an account, browse events, enter events by selecting a category, view their own enrolments, and track their personal results

## Features

- User authentication with role-based access control
- Password hashing using BCrypt
- Session-based authentication
- Full CRUD operations for events (Organiser only)
- Category management for events (Organiser only)
- Event enrolment system (Participant only)
- Results capture and tracking
- Swagger/OpenAPI documentation
- Comprehensive unit testing
- GitHub Actions CI/CD pipeline

## Technology Stack

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core (Code-First approach)
- SQL Server (LocalDB for development)
- BCrypt.Net for password hashing
- Swagger/OpenAPI for API documentation
- xUnit for unit testing
- GitHub Actions for CI/CD

## Prerequisites

Before running this project, ensure you have the following installed:

- .NET 8.0 SDK ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- SQL Server LocalDB (included with Visual Studio) or SQL Server Express
- Visual Studio 2022 or VS Code with C# extension
- Git

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <your-repository-url>
cd RaceDayAPI
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Configure Database Connection

The default connection string in `appsettings.json` uses SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RaceDayDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

If you're using a different SQL Server instance, update the connection string accordingly.

### 4. Create and Apply Database Migrations

Since we're using the Code-First approach, you need to create the initial migration and apply it to the database:

```bash
# Navigate to the API project
cd RaceDayAPI/RaceDayAPI

# Add initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

If you don't have the EF Core tools installed globally:

```bash
dotnet tool install --global dotnet-ef
```

### 5. Run the Application

```bash
dotnet run
```

The API will start on `https://localhost:5001` (or a similar port).

### 6. Access Swagger Documentation

Once the application is running, open your browser and navigate to:

```
https://localhost:5001/swagger
```

This will display the Swagger UI where you can test all API endpoints.

## Running Unit Tests

To run the unit tests:

```bash
# From the solution root
dotnet test

# Or run tests with detailed output
dotnet test --verbosity normal
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login with email and password
- `POST /api/auth/logout` - Logout current user

### User Profile
- `GET /api/users/profile` - Get current user's profile
- `PUT /api/users/profile` - Update current user's profile

### Events
- `GET /api/events` - Get all events
- `GET /api/events/{id}` - Get specific event
- `POST /api/events` - Create new event (Organiser only)
- `PUT /api/events/{id}` - Update event (Organiser only)
- `DELETE /api/events/{id}` - Delete event (Organiser only)

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/event/{eventId}` - Get categories for specific event
- `POST /api/categories` - Create new category (Organiser only)

### Enrolments
- `POST /api/enrolments` - Enrol in an event (Participant only)
- `GET /api/enrolments/my-enrolments` - Get current user's enrolments
- `GET /api/enrolments/event/{eventId}` - Get enrolments for event (Organiser only)

### Results
- `POST /api/results` - Capture result (Organiser only)
- `GET /api/results/my-results` - Get current user's results
- `GET /api/results/event/{eventId}` - Get results for event (Organiser only)

## CI/CD Pipeline

The project uses GitHub Actions for continuous integration and deployment. The workflow:

1. Checks out the code
2. Sets up .NET 8.0 SDK
3. Restores NuGet dependencies
4. Builds the solution in Release mode
5. Runs all unit tests
6. Publishes build artifacts on success

The workflow runs on every push to `main` or `develop` branches, and on all pull requests.

### CI/CD Build Status

![CI/CD Build Status](https://img.shields.io/badge/build-passing-brightgreen)

## Video Presentation

A video demonstration of the API, including code structure explanation, endpoint testing, authentication flow, and unit tests, will be submitted as an unlisted YouTube video. The link will be added here once the video is uploaded.

## Project Structure

```
RaceDayAPI/
├── .github/
│   └── workflows/
│       └── ci-cd.yml          # GitHub Actions workflow
├── RaceDayAPI/
│   ├── Controllers/            # API controllers
│   ├── Data/                   # DbContext and database configuration
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Middleware/             # Custom middleware (authentication)
│   ├── Models/                 # Database entities
│   ├── Services/               # Business logic services
│   ├── Program.cs              # Application entry point
│   └── appsettings.json        # Configuration
├── RaceDayAPI.Tests/           # Unit test project
│   ├── AuthTests.cs            # Authentication tests
│   ├── EventTests.cs           # Event management tests
│   ├── RoleAccessTests.cs      # Role-based access tests
│   └── EnrolmentTests.cs       # Enrolment tests
└── README.md                   # This file
```

## Database Schema

The system uses the following entities:

- **Users**: Stores user information including role (Organiser/Participant)
- **Events**: Event details with organiser relationship
- **Categories**: Event categories (age groups, distances)
- **Enrolments**: Links participants to events with selected category
- **Results**: Captures finish times and positions for participants

## Security Features

- Passwords are hashed using BCrypt before storage
- Session-based authentication maintains user state
- Role-based access control enforced at controller level
- Protected endpoints require authentication
- Organiser-specific operations verify ownership

## Testing

The project includes comprehensive unit tests covering:

- User registration and login
- Event CRUD operations
- Role-based access control
- Enrolment creation and validation
- Database constraints and relationships

All tests use in-memory database for isolation and speed.

## Future Enhancements

- JWT token authentication (alternative to sessions)
- Refresh token mechanism
- Email verification for registration
- Password reset functionality
- Event image upload (Azure Blob Storage - Part 3)
- Rate limiting
- API versioning
- Advanced filtering and pagination

## References

See the Reference List section below for academic sources used in this project.

## License

This project is submitted as part of the PROG6212 Portfolio of Evidence at The Independent Institute of Education (Pty) Ltd 2026.

## Contact

For questions or issues, please contact via the repository's issue tracker.

---

## Reference List (Harvard Style)

1.  Microsoft, 2024. *Entity Framework Core Documentation*. Available at: https://learn.microsoft.com/en-us/ef/core/ [Accessed 20 October 2026].

2.  Microsoft, 2024. *ASP.NET Core Web API Documentation*. Available at: https://learn.microsoft.com/en-us/aspnet/core/web-api/ [Accessed 20 October 2026].

3.  Microsoft, 2024. *Authentication and Authorization in ASP.NET Core*. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/ [Accessed 20 October 2026].

4.  Swashbuckle.AspNetCore, 2024. *Swagger Implementation for ASP.NET Core*. Available at: https://github.com/domaindrivendev/Swashbuckle.AspNetCore [Accessed 20 October 2026].

5.  BCrypt.Net-Next, 2024. *BCrypt Password Hashing Library*. Available at: https://github.com/BcryptNet/bcrypt.net [Accessed 20 October 2026].

6.  xUnit, 2024. *xUnit.net Testing Framework*. Available at: https://xunit.net/ [Accessed 20 October 2026].

7.  GitHub Actions, 2024. *Automating CI/CD with GitHub Actions*. Available at: https://docs.github.com/en/actions [Accessed 20 October 2026].

8.  The Independent Institute of Education (Pty) Ltd, 2026. *PROG6212 Portfolio of Evidence - Part 2*.
