using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitler;
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
        if(!ModelState.IsValid){

            // Report a validation error
            return RedirectToAction("Rejected", "Home", new { message = PrimaryModelStateError() });

        } else if(this.DataService.TryGetSession(application.SessionKey, out var existingSession)){

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
                session.Players.AddRange(new [] { "Adam", "Bob", "Carol", "David" }); // TESTING
            });
            this.Cookies.PlayerName = application.PlayerName;
            this.Cookies.Session = application.SessionKey;
            return RedirectToAction("NewGame", "Home");

        }
    }

    [InvalidSessionExceptionFilter]
    public IActionResult NewGame(){

        // Load the players list
        var session = this.DataService.GetSession(this.Cookies.Session);
        var players = new List<string>();
        session.LockSession(session => {
            players.AddRange(session.Players);
        });

        ViewData["Session"] = this.Cookies.Session;
        ViewData["MyName"] = this.Cookies.PlayerName;
        ViewData["Players"] = players;
        return View();

    }

    [InvalidSessionExceptionFilter]
    public IActionResult JoinGame(){

        // Load the players list
        var session = this.DataService.GetSession(this.Cookies.Session);
        var players = new List<string>();
        session.LockSession(session => {
            players.AddRange(session.Players);
        });

        ViewData["Session"] = this.Cookies.Session;
        ViewData["MyName"] = this.Cookies.PlayerName;
        ViewData["Players"] = players;
        return View();
        
    }

    public IActionResult Rejected(string message){
        ViewData["RejectionMessage"] = message;
        return View();
    }

    [InvalidSessionExceptionFilter]
    public IActionResult StartGame(){

        // Determine players
        var session = this.DataService.GetSession(this.Cookies.Session);
        var players = new List<Player>();
        session.LockSession(lockedSession => {
            players.AddRange(lockedSession.Players.Select(o => new Player(o)));
        });

        // Create a game
        var game = new GameState(players);
        var assigner = new RoleAssigner(Random.Shared);
        var shuffler = new Shuffler(Random.Shared);
        assigner.AssignRoles(game);
        shuffler.Shuffle(game);
        game.LiberalPolicyPassed = 3; // TESTING
        game.FascistPolicyPassed = 2; // TESTING
        game.Hand.AddToTop(Policy.Fascist); // TESTING
        game.Hand.AddToTop(Policy.Liberal); // TESTING
        session.SetGameState(game);

        // Go to the game screen
        return RedirectToAction("Index", "Game");

    }

    public IActionResult InvalidSession(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
