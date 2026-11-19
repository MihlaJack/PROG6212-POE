# RaceDay - Event Management System

## Overview

RaceDay is a comprehensive event management platform designed specifically for the South African road running, walking, and cycling community. This full-stack solution includes a RESTful API backend and an MVC web frontend, with Azure Blob Storage for image uploads and Docker containerization.

The platform supports two distinct user roles:
- **Organiser**: Can create, edit, and delete events, manage event categories, capture participant results, and view all event enrolments
- **Participant**: Can create an account, browse events, enter events by selecting a category, view their own enrolments, and track their personal results

## Features

### Backend API (Part 2)
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

### Frontend MVC (Part 3)
- ASP.NET Core MVC web application
- Role-based navigation and interfaces
- Session-based authentication
- Event management interface for Organisers
- Event browsing and enrolment for Participants
- Azure Blob Storage integration for image uploads
- Docker containerization
- Responsive Bootstrap 5 design
- Sporting theme with custom colors

### File Storage (Part 3)
- Azure Blob Storage integration
- Event banner image uploads
- Profile picture uploads
- File upload through API endpoints
- Public blob access for images

## Technology Stack

- .NET 8.0
- ASP.NET Core Web API (backend)
- ASP.NET Core MVC (frontend)
- Entity Framework Core (Code-First approach)
- SQL Server (LocalDB for development)
- BCrypt.Net for password hashing
- Azure Blob Storage for file uploads
- Swagger/OpenAPI for API documentation
- xUnit for unit testing
- GitHub Actions for CI/CD
- Docker for containerization
- Bootstrap 5 for UI

## Prerequisites

Before running this project, ensure you have the following installed:

- .NET 8.0 SDK ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- SQL Server LocalDB (included with Visual Studio) or SQL Server Express
- Visual Studio 2022 or VS Code with C# extension
- Docker Desktop (for containerization)
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

The default connection string in `RaceDayAPI/appsettings.json` uses SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RaceDayDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

If you're using a different SQL Server instance, update the connection string accordingly.

### 4. Configure Azure Blob Storage (Optional)

For file uploads, you need to configure Azure Blob Storage:

1. Create an Azure Storage Account
2. Get the connection string from Azure Portal
3. Update `RaceDayAPI/appsettings.json`:

```json
"ConnectionStrings": {
  "AzureStorage": "DefaultEndpointsProtocol=https;AccountName=youraccount;AccountKey=yourkey;EndpointSuffix=core.windows.net"
}
```

Create containers named "event-banners" and "profile-pictures" in your storage account.

### 5. Create and Apply Database Migrations

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

### 6. Run the API

```bash
cd RaceDayAPI/RaceDayAPI
dotnet run
```

The API will start on `https://localhost:5201` (or a similar port).

### 7. Run the MVC Application

```bash
cd RaceDayWeb
dotnet run
```

The MVC application will start on `https://localhost:5200` (or a similar port).

### 8. Access Swagger Documentation

Once the API is running, open your browser and navigate to:

```
https://localhost:5201/swagger
```

## Running with Docker

### Build Docker Images

```bash
# Build the API image
docker build -f RaceDayAPI/Dockerfile -t raceday-api .

# Build the MVC image
docker build -f RaceDayWeb/Dockerfile -t raceday-web .
```

### Run Docker Containers

```bash
# Run API container
docker run -p 5201:80 -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal;Database=RaceDayDB;Trusted_Connection=True;MultipleActiveResultSets=true" raceday-api

# Run MVC container
docker run -p 5200:80 -e "ApiBaseUrl=http://host.docker.internal:5201" raceday-web
```

## Project Structure

```
RaceDayAPI/
├── .github/
│   └── workflows/
│       └── ci-cd.yml          # GitHub Actions workflow
├── RaceDayAPI/                # RESTful API project
│   ├── Controllers/           # API controllers
│   ├── Data/                  # DbContext and database configuration
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Middleware/            # Custom middleware (authentication)
│   ├── Models/                # Database entities
│   ├── Services/              # Business logic services
│   ├── Properties/            # Project properties
│   ├── Program.cs             # Application entry point
│   ├── appsettings.json       # Configuration
│   └── Dockerfile             # API containerization
├── RaceDayAPI.Tests/          # Unit test project
│   ├── AuthTests.cs           # Authentication tests
│   ├── EventTests.cs          # Event management tests
│   ├── RoleAccessTests.cs     # Role-based access tests
│   ├── EnrolmentTests.cs      # Enrolment tests
│   └── RaceDayAPI.Tests.csproj
├── RaceDayWeb/                # MVC web application
│   ├── Controllers/           # MVC controllers
│   ├── Models/                # API models and DTOs
│   ├── Services/              # API client service
│   ├── Views/                 # Razor views
│   ├── wwwroot/               # Static files
│   ├── Program.cs             # Application entry point
│   ├── appsettings.json       # Configuration
│   └── Dockerfile             # MVC containerization
├── Dockerfile                 # Multi-project Dockerfile
├── README.md                  # This file
├── CHANGELOG.md               # Project changelog
├── LICENSE                    # MIT License
├── CONTRIBUTING.md            # Contributing guidelines
├── CODE_OF_CONDUCT.md         # Code of conduct
├── SECURITY.md                # Security policy
├── REFERENCES.md              # Academic references
├── PART3_COMPLETION_GUIDE.md  # Part 3 guide
├── PART3_STATUS.md            # Part 3 status report
├── SUBMISSION_NOTES.md        # Submission notes
└── RaceDayAPI.sln             # Solution file
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

### File Uploads
- `POST /api/files/upload/event-banner` - Upload event banner image
- `POST /api/files/upload/profile-picture` - Upload profile picture

## MVC Application Features

### Organiser Interface
- Dashboard with event statistics
- Event management (create, edit, delete)
- Category management
- View all event enrolments
- Capture race results
- Upload event banner images

### Participant Interface
- Browse upcoming events with filtering
- View detailed event information
- Enrol in events with category selection
- Track personal enrolments
- View race results
- Manage profile with picture upload

## CI/CD Pipeline

The project uses GitHub Actions for continuous integration and deployment. The workflow:

1. Checks out the code
2. Sets up .NET 8.0 SDK
3. Restores NuGet dependencies
4. Builds the solution in Release mode
5. Runs all unit tests
6. Publishes API build artifacts
7. Publishes MVC build artifacts

The workflow runs on every push to `main` or `develop` branches, and on all pull requests.

### CI/CD Build Status

![CI/CD Build Status](https://img.shields.io/badge/build-passing-brightgreen)

## Docker Containerization

The application is containerized using Docker with multi-stage builds:

- **API Container**: ASP.NET Core runtime
- **MVC Container**: ASP.NET Core MVC runtime
- **Multi-project Dockerfile**: Builds both projects

### Build Commands

```bash
docker build -f RaceDayAPI/Dockerfile -t raceday-api .
docker build -f RaceDayWeb/Dockerfile -t raceday-web .
```

### Run Commands

```bash
docker run -p 5201:80 -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal;Database=RaceDayDB;Trusted_Connection=True;MultipleActiveResultSets=true" raceday-api
docker run -p 5200:80 -e "ApiBaseUrl=http://host.docker.internal:5201" raceday-web
```

## Video Presentation

A video demonstration of the complete application, including:
- API endpoints and Swagger documentation
- MVC application interface
- Authentication flow
- Role-based access control
- Event management functionality
- Azure Blob Storage integration
- Docker containerization
- Unit tests

The video will be submitted as an unlisted YouTube video. The link will be added here once the video is uploaded.

## Database Schema

The system uses the following entities:

- **Users**: Stores user information including role (Organiser/Participant), profile picture URL
- **Events**: Event details with organiser relationship, banner image URL
- **Categories**: Event categories (age groups, distances)
- **Enrolments**: Links participants to events with selected category
- **Results**: Captures finish times and positions for participants

## Security Features

- Passwords are hashed using BCrypt before storage
- Session-based authentication maintains user state
- Role-based access control enforced at controller level
- Protected endpoints require authentication
- Organiser-specific operations verify ownership
- Azure Blob Storage uses public access for images
- CORS configured for cross-origin requests
- Docker containers use isolated environments

## Testing

The project includes comprehensive unit tests covering:

- User registration and login
- Event CRUD operations
- Role-based access control
- Enrolment creation and validation
- Database constraints and relationships
- Authentication service functionality

All tests use in-memory database for isolation and speed.

## GitHub Commits

- **Part 1**: 20 meaningful commits (ERD, API plan, SQL script)
- **Part 2**: 20 meaningful commits (API development, testing, CI/CD)
- **Part 3**: 20+ meaningful commits (MVC application, Azure integration, Docker)

## Future Enhancements

- JWT token authentication (alternative to sessions)
- Refresh token mechanism
- Email verification for registration
- Password reset functionality
- Advanced filtering and pagination
- Real-time notifications
- Mobile application
- Payment integration for paid events
- Weather API integration for race day forecasts

## References

See the Reference List section below for academic sources used in this project.

## License

This project is submitted as part of the PROG6212 Portfolio of Evidence at The Independent Institute of Education (Pty) Ltd 2026.

## Contact

For questions or issues, please contact via the repository's issue tracker.

---

## Reference List (Harvard Style)

1.  Microsoft, 2024. *Entity Framework Core Documentation*. Available at: https://learn.microsoft.com/en-us/ef/core/ [Accessed 20 November 2026].

2.  Microsoft, 2024. *ASP.NET Core Web API Documentation*. Available at: https://learn.microsoft.com/en-us/aspnet/core/web-api/ [Accessed 20 November 2026].

3.  Microsoft, 2024. *Authentication and Authorization in ASP.NET Core*. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/ [Accessed 20 November 2026].

4.  Microsoft, 2024. *Azure Blob Storage Documentation*. Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/ [Accessed 20 November 2026].

5.  Swashbuckle.AspNetCore, 2024. *Swagger Implementation for ASP.NET Core*. Available at: https://github.com/domaindrivendev/Swashbuckle.AspNetCore [Accessed 20 November 2026].

6.  BCrypt.Net-Next, 2024. *BCrypt Password Hashing Library*. Available at: https://github.com/BcryptNet/bcrypt.net [Accessed 20 November 2026].

7.  xUnit, 2024. *xUnit.net Testing Framework*. Available at: https://xunit.net/ [Accessed 20 November 2026].

8.  GitHub Actions, 2024. *Automating CI/CD with GitHub Actions*. Available at: https://docs.github.com/en/actions [Accessed 20 November 2026].
