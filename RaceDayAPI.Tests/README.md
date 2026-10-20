# RaceDay API Tests

This project contains unit tests for the RaceDay API.

## Test Structure

- **AuthTests.cs**: Tests for user authentication (registration, login, password verification)
- **EventTests.cs**: Tests for event management (CRUD operations)
- **RoleAccessTests.cs**: Tests for role-based access control
- **EnrolmentTests.cs**: Tests for event enrolment functionality

## Running Tests

Run all tests:
```bash
dotnet test
```

Run tests with detailed output:
```bash
dotnet test --verbosity normal
```

Run a specific test class:
```bash
dotnet test --filter "FullyQualifiedName~AuthTests"
```

## Test Coverage

All tests use in-memory database for isolation and speed. Tests cover:
- Authentication flow
- Database operations
- Business logic validation
- Role-based authorization
- Data integrity constraints
