using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite.Controllers;

public class GameController : Controller {
    
    private readonly ILogger<HomeController> Logger;

    public GameController(ILogger<HomeController> logger) {
        this.Logger = logger;
    }

    public IActionResult Index(string session) {
        ViewData["Session"] = session;
        ViewData["MyName"] = "You there";
        return View();
    }
    
}