using Microsoft.AspNetCore.Mvc;
using RaceDayWeb.Services;

namespace RaceDayWeb.Controllers;

public class HomeController : Controller
{
    private readonly ApiService _apiService;

    public HomeController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }
}
