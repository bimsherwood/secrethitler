using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite.Controllers;

public class HomeController : Controller {
    
    private readonly ILogger<HomeController> Logger;

    public HomeController(ILogger<HomeController> logger) {
        this.Logger = logger;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult Apply(ApplicationSubmission application) {
        return RedirectToAction("NewGame", "Home", new{ session = application.SessionKey });
    }

    public IActionResult NewGame(string session){
        ViewData["Session"] = session;
        ViewData["MyName"] = "You there";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
