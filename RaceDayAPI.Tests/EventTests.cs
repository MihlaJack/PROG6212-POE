using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;
using Xunit;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace RaceDayAPI.Tests;

public class EventTests
{
    private RaceDayDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RaceDayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionWarning))
            .Options;

        var context = new RaceDayDbContext(options);
        return context;
    }

    private async Task<User> CreateTestUser(RaceDayDbContext context, string role)
    {
        // Helper method to create a test user - use GUID for email to avoid conflicts
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = $"test{Guid.NewGuid()}@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task CreateEvent_WithValidData_Succeeds()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var organiser = await CreateTestUser(context, "Organiser");

        var newEvent = new Event
        {
            Name = "Comrades Marathon",
            Description = "The ultimate human race",
            EventDate = DateTime.UtcNow.AddDays(30),
            Location = "Pietermaritzburg to Durban",
            Distance = 89,
            EventType = "Run",
            OrganiserId = organiser.UserId,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Events.Add(newEvent);
        await context.SaveChangesAsync();

        // Assert
        var savedEvent = await context.Events.FirstOrDefaultAsync(e => e.EventId == newEvent.EventId);
        Assert.NotNull(savedEvent);
        Assert.Equal("Comrades Marathon", savedEvent.Name);
        Assert.Equal(organiser.UserId, savedEvent.OrganiserId);
    }

    [Fact]
    public async Task UpdateEvent_WithValidData_Succeeds()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var organiser = await CreateTestUser(context, "Organiser");

        var newEvent = new Event
        {
            Name = "Comrades Marathon",
            Description = "The ultimate human race",
            EventDate = DateTime.UtcNow.AddDays(30),
            Location = "Pietermaritzburg to Durban",
            Distance = 89,
            EventType = "Run",
            OrganiserId = organiser.UserId,
            CreatedAt = DateTime.UtcNow
        };

        context.Events.Add(newEvent);
        await context.SaveChangesAsync();

        // Act
        var eventToUpdate = await context.Events.FindAsync(newEvent.EventId);
        eventToUpdate.Name = "Comrades Marathon 2024";
        eventToUpdate.Description = "Updated description";
        await context.SaveChangesAsync();

        // Assert
        var updatedEvent = await context.Events.FindAsync(newEvent.EventId);
        Assert.Equal("Comrades Marathon 2024", updatedEvent.Name);
        Assert.Equal("Updated description", updatedEvent.Description);
    }

    [Fact]
    public async Task DeleteEvent_WithValidId_Succeeds()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var organiser = await CreateTestUser(context, "Organiser");

        var newEvent = new Event
        {
            Name = "Comrades Marathon",
            Description = "The ultimate human race",
            EventDate = DateTime.UtcNow.AddDays(30),
            Location = "Pietermaritzburg to Durban",
            Distance = 89,
            EventType = "Run",
            OrganiserId = organiser.UserId,
            CreatedAt = DateTime.UtcNow
        };

        context.Events.Add(newEvent);
        await context.SaveChangesAsync();

        // Act
        var eventToDelete = await context.Events.FindAsync(newEvent.EventId);
        context.Events.Remove(eventToDelete);
        await context.SaveChangesAsync();

        // Assert
        var deletedEvent = await context.Events.FindAsync(newEvent.EventId);
        Assert.Null(deletedEvent);
    }

    [Fact]
    public async Task GetEvents_ReturnsAllEvents()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var organiser = await CreateTestUser(context, "Organiser");

        var event1 = new Event
        {
            Name = "Comrades Marathon",
            Description = "The ultimate human race",
            EventDate = DateTime.UtcNow.AddDays(30),
            Location = "Pietermaritzburg to Durban",
            Distance = 89,
            EventType = "Run",
            OrganiserId = organiser.UserId,
            CreatedAt = DateTime.UtcNow
        };

        var event2 = new Event
        {
            Name = "Two Oceans Marathon",
            Description = "The world's most beautiful marathon",
            EventDate = DateTime.UtcNow.AddDays(60),
            Location = "Cape Town",
            Distance = 56,
            EventType = "Run",
            OrganiserId = organiser.UserId,
            CreatedAt = DateTime.UtcNow
        };

        context.Events.AddRange(event1, event2);
        await context.SaveChangesAsync();

        // Act
        var events = await context.Events.Include(e => e.Organiser).ToListAsync();

        // Assert
        Assert.Equal(2, events.Count);
    }

    [Fact]
    public async Task CreateCategory_WithValidData_Succeeds()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var organiser = await CreateTestUser(context, "Organiser");

        var newEvent = new Event
        {
            Name = "Comrades Marathon",
            Description = "The ultimate human race",
            EventDate = DateTime.UtcNow.AddDays(30),
            Location = "Pietermaritzburg to Durban",
            Distance = 89,
            EventType = "Run",
            OrganiserId = organiser.UserId,
            CreatedAt = DateTime.UtcNow
        };

        context.Events.Add(newEvent);
        await context.SaveChangesAsync();

        var category = new Category
        {
            Name = "Senior",
            Description = "Age 40-49",
            EventId = newEvent.EventId
        };

        // Act
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Assert
        var savedCategory = await context.Categories.FindAsync(category.CategoryId);
        Assert.NotNull(savedCategory);
        Assert.Equal("Senior", savedCategory.Name);
        Assert.Equal(newEvent.EventId, savedCategory.EventId);
    }
}
