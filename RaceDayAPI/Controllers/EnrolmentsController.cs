using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

/// <summary>
/// Manages event enrolments. Participants can enrol in events, Organisers can view all enrolments
/// </summary>
[ApiController]
[Route("api/enrolments")]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EnrolmentsController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Enrols the current participant in an event
    /// </summary>
    /// <param name="createDto">Enrolment details</param>
    /// <returns>Created enrolment</returns>
    [HttpPost]
    public async Task<IActionResult> CreateEnrolment([FromBody] EnrolmentCreateDto createDto)
    {
        // Only participants can enrol - organisers shouldn't be entering events
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        if (userRole != "Participant")
        {
            return Forbid(new { message = "Only Participants can enrol in events" });
        }

        // Get the current user's ID from the session
        int userId = (int)HttpContext.Items["UserId"]!;

        // Make sure the event actually exists
        var eventCheck = await _context.Events.FindAsync(createDto.EventId);
        if (eventCheck == null)
        {
            return NotFound(new { message = "Event not found" });
        }

        // And the category too
        var categoryCheck = await _context.Categories.FindAsync(createDto.CategoryId);
        if (categoryCheck == null)
        {
            return NotFound(new { message = "Category not found" });
        }

        // Can't enrol with a category from a different event
        if (categoryCheck.EventId != createDto.EventId)
        {
            return BadRequest(new { message = "Category does not belong to this event" });
        }

        // Check if they're already enrolled - no double bookings
        var existingEnrolment = await _context.Enrolments
            .AnyAsync(e => e.UserId == userId && e.EventId == createDto.EventId);

        if (existingEnrolment)
        {
            return Conflict(new { message = "Already enrolled in this event" });
        }

        // Create the enrolment record
        var newEnrolment = new Enrolment
        {
            UserId = userId,
            EventId = createDto.EventId,
            CategoryId = createDto.CategoryId,
            EnrolledAt = DateTime.UtcNow
        };

        _context.Enrolments.Add(newEnrolment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyEnrolments), new { id = newEnrolment.EnrolmentId }, newEnrolment);
    }

    /// <summary>
    /// Gets the current user's enrolments
    /// </summary>
    /// <returns>List of user's enrolments</returns>
    [HttpGet("my-enrolments")]
    public async Task<IActionResult> GetMyEnrolments()
    {
        int userId = (int)HttpContext.Items["UserId"]!;

        var enrolments = await _context.Enrolments
            .Include(e => e.Event)
            .Include(e => e.Category)
            .Include(e => e.User)
            .Where(e => e.UserId == userId)
            .Select(e => new
            {
                e.EnrolmentId,
                e.UserId,
                UserName = e.User.FirstName + " " + e.User.LastName,
                EventName = e.Event.Name,
                EventDate = e.Event.EventDate,
                CategoryName = e.Category.Name,
                e.EnrolledAt
            })
            .ToListAsync();

        return Ok(enrolments);
    }

    /// <summary>
    /// Gets all enrolments for a specific event (Organiser only)
    /// </summary>
    /// <param name="eventId">Event ID</param>
    /// <returns>List of enrolments for the event</returns>
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetEnrolmentsByEvent(int eventId)
    {
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        if (userRole != "Organiser")
        {
            return Forbid(new { message = "Only Organisers can view event enrolments" });
        }

        int userId = (int)HttpContext.Items["UserId"]!;

        // Verify the event exists and belongs to this organiser
        var eventCheck = await _context.Events.FindAsync(eventId);
        if (eventCheck == null)
        {
            return NotFound(new { message = "Event not found" });
        }

        if (eventCheck.OrganiserId != userId)
        {
            return Forbid(new { message = "You can only view enrolments for your own events" });
        }

        var enrolments = await _context.Enrolments
            .Include(e => e.User)
            .Include(e => e.Category)
            .Where(e => e.EventId == eventId)
            .Select(e => new
            {
                e.EnrolmentId,
                ParticipantName = e.User.FirstName + " " + e.User.LastName,
                ParticipantEmail = e.User.Email,
                CategoryName = e.Category.Name,
                e.EnrolledAt
            })
            .ToListAsync();

        return Ok(enrolments);
    }
}
