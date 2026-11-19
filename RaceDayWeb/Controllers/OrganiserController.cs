using Microsoft.AspNetCore.Mvc;
using RaceDayWeb.Models;
using RaceDayWeb.Services;

namespace RaceDayWeb.Controllers;

public class OrganiserController : Controller
{
    private readonly ApiService _apiService;

    public OrganiserController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Dashboard()
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var events = await _apiService.GetEventsAsync();
        var userId = HttpContext.Session.GetInt32("UserId");

        var myEvents = events?.Where(e => e.OrganiserName.Contains(HttpContext.Session.GetString("UserName") ?? "")).ToList();
        
        ViewBag.TotalEvents = myEvents?.Count ?? 0;
        ViewBag.UpcomingEvents = myEvents?.Where(e => e.EventDate > DateTime.Now).Count() ?? 0;
        
        return View(myEvents);
    }

    public async Task<IActionResult> Events()
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var events = await _apiService.GetEventsAsync();
        return View(events);
    }

    public IActionResult CreateEvent()
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(EventCreateRequest request, IFormFile? bannerImage)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        if (bannerImage != null)
        {
            var imageUrl = await _apiService.UploadFileAsync(bannerImage, "banner");
            request.BannerImageUrl = imageUrl;
        }

        var success = await _apiService.CreateEventAsync(request);
        
        if (success)
        {
            return RedirectToAction("Events");
        }

        ModelState.AddModelError("", "Failed to create event");
        return View(request);
    }

    public async Task<IActionResult> EditEvent(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var eventDto = await _apiService.GetEventAsync(id);
        
        if (eventDto == null)
        {
            return NotFound();
        }

        var updateRequest = new EventUpdateRequest
        {
            Name = eventDto.Name,
            Description = eventDto.Description,
            EventDate = eventDto.EventDate,
            Location = eventDto.Location,
            Distance = eventDto.Distance,
            EventType = eventDto.EventType,
            BannerImageUrl = eventDto.BannerImageUrl
        };

        return View(updateRequest);
    }

    [HttpPost]
    public async Task<IActionResult> EditEvent(int id, EventUpdateRequest request, IFormFile? bannerImage)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        if (bannerImage != null)
        {
            var imageUrl = await _apiService.UploadFileAsync(bannerImage, "banner");
            request.BannerImageUrl = imageUrl;
        }

        var success = await _apiService.UpdateEventAsync(id, request);
        
        if (success)
        {
            return RedirectToAction("Events");
        }

        ModelState.AddModelError("", "Failed to update event");
        return View(request);
    }

    public async Task<IActionResult> DeleteEvent(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        await _apiService.DeleteEventAsync(id);
        return RedirectToAction("Events");
    }

    public async Task<IActionResult> Categories(int eventId)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var categories = await _apiService.GetCategoriesByEventAsync(eventId);
        ViewBag.EventId = eventId;
        return View(categories);
    }

    public IActionResult CreateCategory(int eventId)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var request = new CategoryCreateRequest { EventId = eventId };
        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryCreateRequest request)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var success = await _apiService.CreateCategoryAsync(request);
        
        if (success)
        {
            return RedirectToAction("Categories", new { eventId = request.EventId });
        }

        ModelState.AddModelError("", "Failed to create category");
        return View(request);
    }

    public async Task<IActionResult> Enrolments(int eventId)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var enrolments = await _apiService.GetEventEnrolmentsAsync(eventId);
        ViewBag.EventId = eventId;
        return View(enrolments);
    }

    public async Task<IActionResult> Results(int eventId)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var enrolments = await _apiService.GetEventEnrolmentsAsync(eventId);
        ViewBag.EventId = eventId;
        return View(enrolments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResult(ResultCreateRequest request)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
        {
            return RedirectToAction("Login", "Auth");
        }

        var success = await _apiService.CreateResultAsync(request);
        
        if (success)
        {
            return RedirectToAction("Results", new { eventId = request.EventId });
        }

        return RedirectToAction("Results", new { eventId = request.EventId });
    }
}
