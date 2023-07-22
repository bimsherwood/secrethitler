using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite.Controllers;

public class HomeController : Controller {
    
    private readonly ILogger<HomeController> Logger;
    private readonly Cookies Cookies;
    private readonly DataService DataService;

    public HomeController(
            ILogger<HomeController> logger,
            Cookies cookies,
            DataService dataService) {
        this.Logger = logger;
        this.Cookies = cookies;
        this.DataService = dataService;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult Apply(ApplicationSubmission application) {
        if(this.DataService.TryFindSession(application.SessionKey, out var existingSession)){

            // Try check the existing session.
            var successfullyJoined = false;
            var rejectionMessage = "";
            existingSession?.LockSession(session => {
                if(session.GameStarted){
                    rejectionMessage = "The game has already started.";
                    successfullyJoined = false;
                } else if (session.Players.Contains(application.PlayerName)) {
                    rejectionMessage = "That name is already taken.";
                    successfullyJoined = false;
                } else {
                    session.Players.Add(application.PlayerName);
                    successfullyJoined = true;
                }
            });

            // Either join or be rejected
            if(successfullyJoined){
                this.Cookies.PlayerName = application.PlayerName;
                this.Cookies.Session = application.SessionKey;
                return RedirectToAction("JoinGame", "Home");
            } else {
                return RedirectToAction("Rejected", "Home", new { message = rejectionMessage });
            }

        } else {

            // Create a new session
            var newSession = this.DataService.CreateSession(application.SessionKey);
            newSession.LockSession(session => {
                session.Players.Add(application.PlayerName);
            });
            this.Cookies.PlayerName = application.PlayerName;
            this.Cookies.Session = application.SessionKey;
            return RedirectToAction("NewGame", "Home");

        }
    }

    public IActionResult NewGame(){
        ViewData["Session"] = this.Cookies.Session;
        ViewData["MyName"] = this.Cookies.PlayerName;
        return View();
    }

    public IActionResult JoinGame(){
        ViewData["Session"] = this.Cookies.Session;
        ViewData["MyName"] = this.Cookies.PlayerName;
        return View();
    }

    public IActionResult Rejected(string message){
        ViewData["RejectionMessage"] = message;
        return View();
    }

    public IActionResult StartGame(string session){
        return RedirectToAction("Index", "Game", new { session = session });
    }

    public IActionResult InvalidSession(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
