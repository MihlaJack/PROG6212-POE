using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

/// <summary>
/// Manages race results. Organisers can capture results, Participants can view their own results
/// </summary>
[ApiController]
[Route("api/results")]
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new result for a participant (Organiser only)
    /// </summary>
    /// <param name="createDto">Result creation details</param>
    /// <returns>Created result</returns>
    [HttpPost]
    public async Task<IActionResult> CreateResult([FromBody] ResultCreateDto createDto)
    {
        // Only organisers can add results - this is an admin function
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        if (userRole != "Organiser")
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only Organisers can capture results" });
        }

        // Get the organiser's ID
        int userId = (int)HttpContext.Items["UserId"]!;

        // Make sure the event exists and belongs to this organiser
        var eventCheck = await _context.Events.FindAsync(createDto.EventId);
        if (eventCheck == null)
        {
            return NotFound(new { message = "Event not found" });
        }

        if (eventCheck.OrganiserId != userId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "You can only capture results for your own events" });
        }

        // Check the participant actually exists
        var userCheck = await _context.Users.FindAsync(createDto.UserId);
        if (userCheck == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Don't allow duplicate results for the same person in the same event
        var existingResult = await _context.Results
            .AnyAsync(r => r.UserId == createDto.UserId && r.EventId == createDto.EventId);

        if (existingResult)
        {
            return Conflict(new { message = "Result already exists for this user in this event" });
        }

        // Create the result record
        var newResult = new Result
        {
            UserId = createDto.UserId,
            EventId = createDto.EventId,
            FinishTime = createDto.FinishTime,
            FinishingPosition = createDto.FinishingPosition,
            RecordedAt = DateTime.UtcNow
        };

        _context.Results.Add(newResult);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyResults), new { id = newResult.ResultId }, newResult);
    }

    /// <summary>
    /// Gets the current user's results
    /// </summary>
    /// <returns>List of user's results</returns>
    [HttpGet("my-results")]
    public async Task<IActionResult> GetMyResults()
    {
        int userId = (int)HttpContext.Items["UserId"]!;

        var results = await _context.Results
            .Include(r => r.Event)
            .Where(r => r.UserId == userId)
            .Select(r => new
            {
                r.ResultId,
                EventName = r.Event.Name,
                EventDate = r.Event.EventDate,
                r.FinishTime,
                r.FinishingPosition,
                r.RecordedAt
            })
            .ToListAsync();

        return Ok(results);
    }

    /// <summary>
    /// Gets all results for a specific event (Organiser only)
    /// </summary>
    /// <param name="eventId">Event ID</param>
    /// <returns>List of results for the event</returns>
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetResultsByEvent(int eventId)
    {
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        if (userRole != "Organiser")
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only Organisers can view event results" });
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
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "You can only view results for your own events" });
        }

        var results = await _context.Results
            .Include(r => r.User)
            .Where(r => r.EventId == eventId)
            .OrderBy(r => r.FinishingPosition)
            .Select(r => new
            {
                r.ResultId,
                ParticipantName = r.User.FirstName + " " + r.User.LastName,
                ParticipantEmail = r.User.Email,
                r.FinishTime,
                r.FinishingPosition,
                r.RecordedAt
            })
            .ToListAsync();

        return Ok(results);
    }
}
