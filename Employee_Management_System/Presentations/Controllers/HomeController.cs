using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Employee_Management_System.Presentations.ViewModels;

namespace Employee_Management_System.Presentations.Controllers;

[Route("")]
public class HomeController : Controller
{
    [Route("")]
    public IActionResult Root()
    {
        return RedirectToAction("Index");
    }

    [Route("Menu")]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
