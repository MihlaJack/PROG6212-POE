using Microsoft.AspNetCore.Mvc;
using RaceDayWeb.Models;
using RaceDayWeb.Services;

namespace RaceDayWeb.Controllers;

public class ParticipantController : Controller
{
    private readonly ApiService _apiService;

    public ParticipantController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
        {
            return RedirectToAction("Login", "Auth");
        }

        var events = await _apiService.GetEventsAsync();
        var upcomingEvents = events?.Where(e => e.EventDate > DateTime.Now).ToList();
        
        return View(upcomingEvents);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
        {
            return RedirectToAction("Login", "Auth");
        }

        var eventDto = await _apiService.GetEventAsync(id);
        var categories = await _apiService.GetCategoriesByEventAsync(id);
        
        ViewBag.Categories = categories;
        return View(eventDto);
    }

    [HttpPost]
    public async Task<IActionResult> Enrol(EnrolmentRequest request)
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
        {
            return RedirectToAction("Login", "Auth");
        }

        var success = await _apiService.EnrolAsync(request);
        
        if (success)
        {
            return RedirectToAction("MyEnrolments");
        }

        return RedirectToAction("Details", new { id = request.EventId });
    }

    public async Task<IActionResult> MyEnrolments()
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
        {
            return RedirectToAction("Login", "Auth");
        }

        var enrolments = await _apiService.GetMyEnrolmentsAsync();
        return View(enrolments);
    }

    public async Task<IActionResult> MyResults()
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
        {
            return RedirectToAction("Login", "Auth");
        }

        var results = await _apiService.GetMyResultsAsync();
        return View(results);
    }

    public async Task<IActionResult> Profile()
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
        {
            return RedirectToAction("Login", "Auth");
        }

        var profile = await _apiService.GetProfileAsync();
        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(UserProfileUpdateRequest request, IFormFile? profilePicture)
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
        {
            return RedirectToAction("Login", "Auth");
        }

        if (profilePicture != null)
        {
            var imageUrl = await _apiService.UploadFileAsync(profilePicture, "profile");
            request.ProfilePictureUrl = imageUrl;
        }

        var success = await _apiService.UpdateProfileAsync(request);
        
        if (success)
        {
            return RedirectToAction("Profile");
        }

        ModelState.AddModelError("", "Failed to update profile");
        var profile = await _apiService.GetProfileAsync();
        return View(profile);
    }
}
