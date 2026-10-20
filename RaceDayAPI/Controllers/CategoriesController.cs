using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

/// <summary>
/// Manages event categories. Organisers can create categories, both roles can view categories
/// </summary>
[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public CategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all categories in the system
    /// </summary>
    /// <returns>List of all categories</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _context.Categories
            .Include(c => c.Event)
            .Select(c => new
            {
                c.CategoryId,
                c.Name,
                c.Description,
                EventName = c.Event.Name,
                c.EventId
            })
            .ToListAsync();

        return Ok(categories);
    }

    /// <summary>
    /// Gets categories for a specific event
    /// </summary>
    /// <param name="eventId">Event ID</param>
    /// <returns>List of categories for the event</returns>
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetCategoriesByEvent(int eventId)
    {
        var categories = await _context.Categories
            .Where(c => c.EventId == eventId)
            .Select(c => new
            {
                c.CategoryId,
                c.Name,
                c.Description,
                c.EventId
            })
            .ToListAsync();

        return Ok(categories);
    }

    /// <summary>
    /// Creates a new category for an event (Organiser only)
    /// </summary>
    /// <param name="createDto">Category creation details</param>
    /// <returns>Created category</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto createDto)
    {
        string userRole = HttpContext.Items["UserRole"]!.ToString()!;

        if (userRole != "Organiser")
        {
            return Forbid(new { message = "Only Organisers can create categories" });
        }

        int userId = (int)HttpContext.Items["UserId"]!;

        // Verify the event exists and belongs to this organiser
        var eventCheck = await _context.Events.FindAsync(createDto.EventId);
        if (eventCheck == null)
        {
            return NotFound(new { message = "Event not found" });
        }

        if (eventCheck.OrganiserId != userId)
        {
            return Forbid(new { message = "You can only create categories for your own events" });
        }

        var newCategory = new Category
        {
            Name = createDto.Name,
            Description = createDto.Description,
            EventId = createDto.EventId
        };

        _context.Categories.Add(newCategory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllCategories), new { id = newCategory.CategoryId }, newCategory);
    }
}
