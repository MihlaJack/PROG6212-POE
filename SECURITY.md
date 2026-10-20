# Security Policy

## Supported Versions

Currently, only the latest version of RaceDay API is supported with security updates.

## Reporting a Vulnerability

If you discover a security vulnerability, please report it privately to the project maintainers.

## Security Best Practices

- Passwords are hashed using BCrypt
- Session-based authentication is used
- Role-based access control is enforced
- SQL injection is prevented through parameterized queries (EF Core)
- CORS is configured for cross-origin requests

## Dependencies

The project uses the following security-related packages:
- BCrypt.Net-Next for password hashing
- Microsoft.AspNetCore.Authentication for authentication
- Microsoft.EntityFrameworkCore for database operations

Regular updates are recommended for security patches.
