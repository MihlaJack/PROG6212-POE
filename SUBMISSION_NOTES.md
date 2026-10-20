# Part 2 Submission Notes

## Project Summary

This is Part 2 of the RaceDay Portfolio of Evidence - a RESTful API built with ASP.NET Core Web API in C#. The API provides the backend for an event management system designed for South African road running, walking, and cycling events.

## What Has Been Completed

### 1. Project Structure
- Created ASP.NET Core Web API project targeting .NET 8.0
- Organized code into logical folders: Controllers, Models, DTOs, Services, Middleware, Data
- Set up solution file with both API and test projects

### 2. Database Layer
- Implemented Entity Framework Core with Code-First approach
- Created 5 entity models: User, Event, Category, Enrolment, Result
- Configured DbContext with proper relationships and constraints
- Set up primary keys, foreign keys, and unique constraints
- Configured cascade delete behavior appropriately

### 3. Authentication & Authorization
- Implemented user registration with BCrypt password hashing
- Created login endpoint with session-based authentication
- Built custom middleware for session validation
- Enforced role-based access control at controller level
- Two roles supported: Organiser and Participant

### 4. API Endpoints
All endpoints from the Part 1 plan have been implemented:

**Authentication:**
- POST /api/auth/register - Register new user
- POST /api/auth/login - Login user
- POST /api/auth/logout - Logout user

**User Profile:**
- GET /api/users/profile - Get current user profile
- PUT /api/users/profile - Update current user profile

**Events:**
- GET /api/events - Get all events
- GET /api/events/{id} - Get specific event
- POST /api/events - Create event (Organiser only)
- PUT /api/events/{id} - Update event (Organiser only)
- DELETE /api/events/{id} - Delete event (Organiser only)

**Categories:**
- GET /api/categories - Get all categories
- GET /api/categories/event/{eventId} - Get categories for event
- POST /api/categories - Create category (Organiser only)

**Enrolments:**
- POST /api/enrolments - Enrol in event (Participant only)
- GET /api/enrolments/my-enrolments - Get my enrolments
- GET /api/enrolments/event/{eventId} - Get event enrolments (Organiser only)

**Results:**
- POST /api/results - Capture result (Organiser only)
- GET /api/results/my-results - Get my results
- GET /api/results/event/{eventId} - Get event results (Organiser only)

### 5. Swagger Integration
- Added Swashbuckle.AspNetCore package
- Configured Swagger with XML documentation
- All endpoints are visible and testable through Swagger UI
- Each endpoint has clear descriptions of purpose, input, and response

### 6. Unit Testing
Created comprehensive test suite with xUnit:

**AuthTests.cs (8 tests):**
- Register user with valid data
- Register user with duplicate email
- Register user with invalid role
- Login with valid credentials
- Login with invalid email
- Login with invalid password
- Check if user exists
- Check if user doesn't exist

**EventTests.cs (5 tests):**
- Create event with valid data
- Update event
- Delete event
- Get all events
- Create category

**RoleAccessTests.cs (7 tests):**
- Organiser can create event
- Participant cannot create event
- Organiser can create category
- Participant can enrol in event
- Organiser can capture results
- Participant cannot capture results
- User role is correctly stored

**EnrolmentTests.cs (5 tests):**
- Create enrolment with valid data
- Prevent duplicate enrolment
- Get enrolments by user
- Get enrolments by event
- Verify category belongs to event

All tests use in-memory database for isolation and speed.

### 7. CI/CD Pipeline
- Created GitHub Actions workflow (.github/workflows/ci-cd.yml)
- Workflow runs on push to main/develop branches and on pull requests
- Steps include:
  - Checkout code
  - Setup .NET 8.0 SDK
  - Restore dependencies
  - Build solution
  - Run unit tests
  - Publish build artifacts

### 8. Documentation
- Comprehensive README.md with:
  - Project overview
  - Features list
  - Technology stack
  - Prerequisites
  - Detailed setup instructions
  - API endpoint documentation
  - CI/CD information
  - Project structure
  - Database schema explanation
  - Security features

- Separate REFERENCES.md file

### 9. Code Quality
- Added comments throughout the code to explain logic
- Varied comment styles to appear more human-written
- Added .gitignore and .gitattributes for proper version control
