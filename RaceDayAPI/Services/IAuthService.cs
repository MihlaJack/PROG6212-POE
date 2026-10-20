using RaceDayAPI.DTOs;

namespace RaceDayAPI.Services;

public interface IAuthService
{
    Task<bool> RegisterUser(UserRegisterDto registerDto);
    Task<(bool success, int? userId, string? role)> LoginUser(UserLoginDto loginDto);
    Task<bool> UserExists(string email);
}
