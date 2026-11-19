using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

/// <summary>
/// Manages event operations. Organisers can create/update/delete events, both roles can view events
/// </summary>
[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventsController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all events in the system
    /// </summary>
    /// <returns>List of all events</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        // Grab all events from the database, including the organiser info and categories
        var events = await _context.Events
            .Include(e => e.Organiser)
            .Include(e => e.Categories)
            .Select(e => new
            {
                e.EventId,
                e.Name,
                e.Description,
                e.EventDate,
                e.Location,
                e.Distance,
                e.EventType,
                e.BannerImageUrl,
                OrganiserName = e.Organiser.FirstName + " " + e.Organiser.LastName,
                Categories = e.Categories.Select(c => new { c.CategoryId, c.Name })
            })
            .ToListAsync();

        return Ok(events);
    }

    /// <summary>
    /// Gets a specific event by ID
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <returns>Event details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var eventDetails = await _context.Events
            .Include(e => e.Organiser)
            .Include(e => e.Categories)
            .Where(e => e.EventId == id)
            .Select(e => new
            {
                e.EventId,
                e.Name,
                e.Description,
                e.EventDate,
                e.Location,
                e.Distance,
                e.EventType,
                e.BannerImageUrl,
                OrganiserName = e.Organiser.FirstName + " " + e.Organiser.LastName,
                Categories = e.Categories.Select(c => new { c.CategoryId, c.Name, c.Description })
            })
            .FirstOrDefaultAsync();

        if (eventDetails == null)
        {
            return NotFound(new { message = "Event not found" });
        }

        return Ok(eventDetails);
    }

    /// <summary>
    /// Creates a new event (Organiser only)
    /// </summary>
    /// <param name="createDto">Event creation details</param>
    /// <returns>Created event</returns>
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto createDto)
    {
        // Get the user's role from the session
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        // Only organisers can create events - participants can't
        if (userRole != "Organiser")
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only Organisers can create events" });
        }

        // Get the user ID so we know who the organiser is
        int userId = (int)HttpContext.Items["UserId"]!;

        // Build the event object from the DTO
        var newEvent = new Event
        {
            Name = createDto.Name,
            Description = createDto.Description,
            EventDate = createDto.EventDate,
            Location = createDto.Location,
            Distance = createDto.Distance,
            EventType = createDto.EventType,
            BannerImageUrl = createDto.BannerImageUrl,
            OrganiserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        // Save it to the database
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        // Return the created event
        return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, new
        {
            newEvent.EventId,
            newEvent.Name,
            newEvent.Description,
            newEvent.EventDate,
            newEvent.Location,
            newEvent.Distance,
            newEvent.EventType,
            newEvent.BannerImageUrl,
            newEvent.OrganiserId,
            newEvent.CreatedAt
        });
    }

    /// <summary>
    /// Updates an existing event (Organiser only)
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <param name="updateDto">Updated event details</param>
    /// <returns>Updated event</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventUpdateDto updateDto)
    {
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        if (userRole != "Organiser")
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only Organisers can update events" });
        }

        int userId = (int)HttpContext.Items["UserId"]!;

        var eventToUpdate = await _context.Events.FindAsync(id);

        if (eventToUpdate == null)
        {
            return NotFound(new { message = "Event not found" });
        }

        // Check if the user is the organiser of this event
        if (eventToUpdate.OrganiserId != userId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "You can only update your own events" });
        }

        eventToUpdate.Name = updateDto.Name;
        eventToUpdate.Description = updateDto.Description;
        eventToUpdate.EventDate = updateDto.EventDate;
        eventToUpdate.Location = updateDto.Location;
        eventToUpdate.Distance = updateDto.Distance;
        eventToUpdate.EventType = updateDto.EventType;
        eventToUpdate.BannerImageUrl = updateDto.BannerImageUrl;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            eventToUpdate.EventId,
            eventToUpdate.Name,
            eventToUpdate.Description,
            eventToUpdate.EventDate,
            eventToUpdate.Location,
            eventToUpdate.Distance,
            eventToUpdate.EventType,
            eventToUpdate.BannerImageUrl,
            eventToUpdate.OrganiserId,
            eventToUpdate.CreatedAt
        });
    }

    /// <summary>
    /// Deletes an event (Organiser only)
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        if (userRole != "Organiser")
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only Organisers can delete events" });
        }

        int userId = (int)HttpContext.Items["UserId"]!;

        var eventToDelete = await _context.Events.FindAsync(id);

        if (eventToDelete == null)
        {
            return NotFound(new { message = "Event not found" });
        }

        // Check if the user is the organiser of this event
        if (eventToDelete.OrganiserId != userId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "You can only delete your own events" });
        }

        _context.Events.Remove(eventToDelete);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Event deleted successfully" });
    }
}
