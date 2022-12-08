using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gallery.Models;

namespace Gallery.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Gallery");
        }
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
