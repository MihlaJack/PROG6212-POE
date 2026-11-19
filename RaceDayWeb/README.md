# RaceDay MVC Application

This is the web frontend for the RaceDay event management platform.

## Features

- Role-based authentication (Organiser/Participant)
- Event browsing and management
- Category management
- Enrolment tracking
- Results management
- Profile management
- Image uploads
- Responsive design with Bootstrap 5

## Setup

1. Configure API URL in `appsettings.json`
2. Run the API first: `cd ../RaceDayAPI/RaceDayAPI && dotnet run`
3. Run this app: `dotnet run`

## Controllers

- `HomeController`: Landing page and about
- `AuthController`: Login, register, logout
- `OrganiserController`: Event management for organisers
- `ParticipantController`: Event browsing and enrolment for participants

## Services

- `ApiService`: HTTP client for API communication

## Models

- `ApiModels.cs`: Data transfer objects for API communication

## Views

### Shared
- `_Layout.cshtml`: Main layout with navigation

### Home
- `Index.cshtml`: Landing page
- `About.cshtml`: About page

### Auth
- `Login.cshtml`: Login form
- `Register.cshtml`: Registration form

### Organiser
- `Dashboard.cshtml`: Event statistics dashboard
- `Events.cshtml`: Event list management
- `CreateEvent.cshtml`: Event creation form
- `EditEvent.cshtml`: Event editing form
- `Categories.cshtml`: Category management
- `CreateCategory.cshtml`: Category creation form
- `Enrolments.cshtml`: View event enrolments
- `Results.cshtml`: Capture race results

### Participant
- `Index.cshtml`: Browse events
- `Details.cshtml`: Event details and enrolment
- `MyEnrolments.cshtml`: View personal enrolments
- `MyResults.cshtml`: View race results
- `Profile.cshtml`: Profile management
