using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Services;

public class AuthService : IAuthService
{
    private readonly RaceDayDbContext _context;

    public AuthService(RaceDayDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterUser(UserRegisterDto registerDto)
    {
        // First thing - check if someone already has this email
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            return false;
        }

        // Make sure they picked a valid role - only Organiser or Participant allowed
        if (registerDto.Role != "Organiser" && registerDto.Role != "Participant")
        {
            return false;
        }

        // Don't store the password as plain text - hash it with BCrypt
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        // Create the user object now
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            Role = registerDto.Role,
            CreatedAt = DateTime.UtcNow
        };

        // Save it to the database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<(bool success, int? userId, string? role)> LoginUser(UserLoginDto loginDto)
    {
        // Look for the user by their email address
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        // Can't find them? Login fails
        if (user == null)
        {
            return (false, null, null);
        }

        // Check if the password matches what we have stored
        bool isValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

        // Wrong password? No access
        if (!isValid)
        {
            return (false, null, null);
        }

        // Everything checks out - send back their info
        return (true, user.UserId, user.Role);
    }

    public async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
