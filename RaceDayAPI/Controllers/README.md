# API Controllers

This folder contains all API controllers for the RaceDay system.

## Controllers

- **AuthController.cs**: Handles user registration, login, and logout
- **UsersController.cs**: Manages user profile operations
- **EventsController.cs**: Manages event CRUD operations
- **CategoriesController.cs**: Manages event categories
- **EnrolmentsController.cs**: Handles event enrolments
- **ResultsController.cs**: Manages race results

## Controller Pattern

All controllers follow these patterns:
- Dependency injection for DbContext
- Role-based authorization checks
- Proper HTTP status codes
- DTOs for request/response
- Swagger documentation comments
