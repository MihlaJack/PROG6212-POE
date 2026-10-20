using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers;

/// <summary>
/// Handles user authentication operations including registration and login
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user with the specified role (Organiser or Participant)
    /// </summary>
    /// <param name="registerDto">User registration details</param>
    /// <returns>Success message if registration succeeds</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        // First check if the model validation passed
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Make sure this email isn't already taken
        if (await _authService.UserExists(registerDto.Email))
        {
            return Conflict(new { message = "User with this email already exists" });
        }

        // Try to register the user now
        bool success = await _authService.RegisterUser(registerDto);

        // If it failed, probably because of an invalid role
        if (!success)
        {
            return BadRequest(new { message = "Invalid role or registration failed" });
        }

        // All good, return success
        return CreatedAtAction(nameof(Register), new { message = "User registered successfully" });
    }

    /// <summary>
    /// Logs in a user with their email and password
    /// </summary>
    /// <param name="loginDto">User login credentials</param>
    /// <returns>User information upon successful login</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        // Validate the input first
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Attempt to authenticate the user
        var (success, userId, role) = await _authService.LoginUser(loginDto);

        // If authentication failed, let them know
        if (!success)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        // Set up the session so we remember who they are
        HttpContext.Session.SetInt32("UserId", userId!.Value);
        HttpContext.Session.SetString("UserRole", role!);

        // Send back their info
        return Ok(new 
        { 
            message = "Login successful", 
            userId = userId.Value, 
            role = role 
        });
    }

    /// <summary>
    /// Logs out the current user by clearing their session
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Logout successful" });
    }
}
