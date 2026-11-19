using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Models;

namespace RaceDayAPI.Data;

public class RaceDayDbContext : DbContext
{
    public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Enrolment> Enrolments { get; set; }
    public DbSet<Result> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set up the User table - email needs to be unique and required
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Role).IsRequired();
        });

        // Event table setup - linked to the organiser who created it
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.EventType).IsRequired();
            entity.Property(e => e.Distance).HasPrecision(10, 2);
            entity.HasOne(e => e.Organiser)
                  .WithMany(u => u.OrganisedEvents)
                  .HasForeignKey(e => e.OrganiserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Categories belong to events - cascade delete if event is removed
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne(e => e.Event)
                  .WithMany(e => e.Categories)
                  .HasForeignKey(e => e.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Enrolments link users to events with a category
        modelBuilder.Entity<Enrolment>(entity =>
        {
            entity.HasKey(e => e.EnrolmentId);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Enrolments)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Event)
                  .WithMany(e => e.Enrolments)
                  .HasForeignKey(e => e.EventId)
                  .OnDelete(DeleteBehavior.ClientCascade);
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Enrolments)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.ClientCascade);
            
            // Make sure a user can't enrol in the same event twice
            entity.HasIndex(e => new { e.UserId, e.EventId }).IsUnique();
        });

        // Results capture finish times for participants
        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultId);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Results)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Event)
                  .WithMany()
                  .HasForeignKey(e => e.EventId)
                  .OnDelete(DeleteBehavior.ClientCascade);
            
            // Only one result per user per event allowed
            entity.HasIndex(e => new { e.UserId, e.EventId }).IsUnique();
        });
    }
}
