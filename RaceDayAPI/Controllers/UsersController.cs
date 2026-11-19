using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers;

/// <summary>
/// Manages user profile operations for both Organisers and Participants
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public UsersController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the current user's profile information
    /// </summary>
    /// <returns>User profile details</returns>
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        int userId = (int)HttpContext.Items["UserId"]!;

        var user = await _context.Users
            .Where(u => u.UserId == userId)
            .Select(u => new UserProfileDto
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                ProfilePictureUrl = u.ProfilePictureUrl,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(user);
    }

    /// <summary>
    /// Updates the current user's profile information
    /// </summary>
    /// <param name="updateDto">Updated profile information</param>
    /// <returns>Updated user profile</returns>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto updateDto)
    {
        int userId = (int)HttpContext.Items["UserId"]!;

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        user.FirstName = updateDto.FirstName;
        user.LastName = updateDto.LastName;
        user.Email = updateDto.Email;
        user.ProfilePictureUrl = updateDto.ProfilePictureUrl;

        await _context.SaveChangesAsync();

        return Ok(new UserProfileDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            ProfilePictureUrl = user.ProfilePictureUrl,
            CreatedAt = user.CreatedAt
        });
    }
}
