using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;
using RaceDayAPI.Services;
using Xunit;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace RaceDayAPI.Tests;

public class AuthTests
{
    private RaceDayDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RaceDayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new RaceDayDbContext(options);
        return context;
    }

    [Fact]
    public async Task RegisterUser_WithValidData_ReturnsTrue()
    {
        // Set up a fresh in-memory database for this test
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);
        
        // Create a test user with valid details
        var registerDto = new UserRegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123!",
            Role = "Participant"
        };

        // Try to register the user
        var result = await authService.RegisterUser(registerDto);

        // Should work and we should have one user in the DB
        Assert.True(result);
        Assert.Equal(1, await context.Users.CountAsync());
    }

    [Fact]
    public async Task RegisterUser_WithDuplicateEmail_ReturnsFalse()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);
        var registerDto = new UserRegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123!",
            Role = "Participant"
        };

        // Register first user
        await authService.RegisterUser(registerDto);

        // Try to register with same email
        var duplicateDto = new UserRegisterDto
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "john@example.com",
            Password = "Password456!",
            Role = "Participant"
        };

        // Act
        var result = await authService.RegisterUser(duplicateDto);

        // Assert
        Assert.False(result);
        Assert.Equal(1, await context.Users.CountAsync());
    }

    [Fact]
    public async Task RegisterUser_WithInvalidRole_ReturnsFalse()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);
        var registerDto = new UserRegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123!",
            Role = "InvalidRole"
        };

        // Act
        var result = await authService.RegisterUser(registerDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task LoginUser_WithValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);
        var registerDto = new UserRegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123!",
            Role = "Participant"
        };

        await authService.RegisterUser(registerDto);

        var loginDto = new UserLoginDto
        {
            Email = "john@example.com",
            Password = "Password123!"
        };

        // Act
        var (success, userId, role) = await authService.LoginUser(loginDto);

        // Assert
        Assert.True(success);
        Assert.NotNull(userId);
        Assert.Equal("Participant", role);
    }

    [Fact]
    public async Task LoginUser_WithInvalidEmail_ReturnsFailure()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);
        var loginDto = new UserLoginDto
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        // Act
        var (success, userId, role) = await authService.LoginUser(loginDto);

        // Assert
        Assert.False(success);
        Assert.Null(userId);
        Assert.Null(role);
    }

    [Fact]
    public async Task LoginUser_WithInvalidPassword_ReturnsFailure()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);
        var registerDto = new UserRegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123!",
            Role = "Participant"
        };

        await authService.RegisterUser(registerDto);

        var loginDto = new UserLoginDto
        {
            Email = "john@example.com",
            Password = "WrongPassword!"
        };

        // Act
        var (success, userId, role) = await authService.LoginUser(loginDto);

        // Assert
        Assert.False(success);
        Assert.Null(userId);
        Assert.Null(role);
    }

    [Fact]
    public async Task UserExists_WithExistingEmail_ReturnsTrue()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);
        var registerDto = new UserRegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Password = "Password123!",
            Role = "Participant"
        };

        await authService.RegisterUser(registerDto);

        // Act
        var exists = await authService.UserExists("john@example.com");

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task UserExists_WithNonExistingEmail_ReturnsFalse()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var authService = new AuthService(context);

        // Act
        var exists = await authService.UserExists("nonexistent@example.com");

        // Assert
        Assert.False(exists);
    }
}
