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
        //return RedirectToAction("JoinGame", "Home", new{ session = application.SessionKey });
        //return RedirectToAction("Rejected", "Home", new{ message = "Just because" });
    }

    public IActionResult NewGame(string session){
        ViewData["Session"] = session;
        ViewData["MyName"] = "You there";
        return View();
    }

    public IActionResult JoinGame(string session){
        ViewData["Session"] = session;
        ViewData["MyName"] = "You there";
        return View();
    }

    public IActionResult Rejected(string message){
        ViewData["RejectionMessage"] = message;
        return View();
    }

    public IActionResult StartGame(string session){
        return RedirectToAction("Index", "Game", new{ session = session });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
