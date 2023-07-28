using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitler;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite.Controllers;

public class HomeController : Controller {
    
    private readonly ILogger<HomeController> Logger;
    private readonly IConfiguration Config;
    private readonly Cookies Cookies;
    private readonly DataService DataService;

    public HomeController(
            ILogger<HomeController> logger,
            IConfiguration config,
            Cookies cookies,
            DataService dataService) {
        this.Logger = logger;
        this.Config = config;
        this.Cookies = cookies;
        this.DataService = dataService;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult Apply(ApplicationSubmission application) {
        if(!ModelState.IsValid){

            // Report a validation error
            return RedirectToAction("Rejected", "Home", new { message = PrimaryModelStateError() });

        } else if(this.DataService.TryGetSession(application.SessionKey, out var existingSession)){

            // Try check the existing session.
            var successfullyJoined = false;
            var rejectionMessage = "";
            existingSession?.LockSession(lockedSession => {
                if(lockedSession.GameStarted){
                    rejectionMessage = "The game has already started.";
                    successfullyJoined = false;
                } else if (lockedSession.RegisteredPlayers.Contains(application.PlayerName)) {
                    rejectionMessage = "That name is already taken.";
                    successfullyJoined = false;
                } else {
                    lockedSession.RegisteredPlayers.Add(application.PlayerName);
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
                session.RegisteredPlayers.Add(application.PlayerName);
                session.RegisteredPlayers.AddRange(new [] { "Adam", "Bob", "Carol", "David" }); // TESTING
            });
            this.Cookies.PlayerName = application.PlayerName;
            this.Cookies.Session = application.SessionKey;
            return RedirectToAction("NewGame", "Home");

        }
    }

    [InvalidSessionExceptionFilter]
    public IActionResult NewGame(){
        CreateSession(this.Cookies.Session, this.Cookies.PlayerName);
        return RedirectToAction("Index", "Lobby", new { hosting = true });
    }

    [InvalidSessionExceptionFilter]
    public IActionResult JoinGame(){
        return RedirectToAction("Index", "Lobby", new { hosting = false });
    }

    public IActionResult Rejected(string message){
        ViewData["RejectionMessage"] = message;
        return View();
    }

    public IActionResult InvalidSession(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private Session CreateSession(string sessionKey, string firstPlayerName){

        var newSession = this.DataService.CreateSession(sessionKey);
        newSession.LockSession(lockedSession => {
            lockedSession.RegisteredPlayers.Add(firstPlayerName);
        });

        if(this.Config["ASPNETCORE_ENVIRONMENT"] == "Development"){
            newSession.LockSession(lockedSession => {
                lockedSession.RegisteredPlayers.AddRange(new [] { "Bob", "Carol", "David", "Edward" });
            });
        }

        return newSession;
        
    }

    private string PrimaryModelStateError(){
        var errors = ModelState
            .SelectMany(o =>
                o.Value?.Errors
                ?? new Microsoft.AspNetCore.Mvc.ModelBinding.ModelErrorCollection())
            .Select(o => o.ErrorMessage)
            .Where(o => o is not null);
        return errors.FirstOrDefault()
            ?? "Unknown error";
    }

}
