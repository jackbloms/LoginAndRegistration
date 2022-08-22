using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoginAndRegis.Models;

namespace LoginAndRegis.Controllers;

public class HomeController : Controller
{
    public IActionResult Privacy()
    {
        return View();
    }

}
