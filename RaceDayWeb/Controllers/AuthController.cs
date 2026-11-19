using Microsoft.AspNetCore.Mvc;
using RaceDayWeb.Models;
using RaceDayWeb.Services;

namespace RaceDayWeb.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _apiService;

    public AuthController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _apiService.LoginAsync(request);
        
        if (response != null)
        {
            HttpContext.Session.SetInt32("UserId", response.UserId);
            HttpContext.Session.SetString("UserRole", response.Role);
            
            return response.Role == "Organiser" 
                ? RedirectToAction("Dashboard", "Organiser") 
                : RedirectToAction("Index", "Participant");
        }

        ModelState.AddModelError("", "Invalid email or password");
        return View(request);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var success = await _apiService.RegisterAsync(request);
        
        if (success)
        {
            return RedirectToAction("Login");
        }

        ModelState.AddModelError("", "Registration failed. Email may already exist.");
        return View(request);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
