using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitler;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite.Controllers;

public class LobbyController : Controller {
    
    private readonly ILogger<HomeController> Logger;
    private readonly Cookies Cookies;
    private readonly DataService DataService;

    public LobbyController(
            ILogger<HomeController> logger,
            Cookies cookies,
            DataService dataService) {
        this.Logger = logger;
        this.Cookies = cookies;
        this.DataService = dataService;
    }

    [InvalidSessionExceptionFilter]
    public IActionResult Index(bool hosting){

        var session = this.DataService.GetSession(this.Cookies.Session);
        var players = new List<string>();
        var started = false; 
        session.LockSession(lockedSession => {
            players.AddRange(lockedSession.RegisteredPlayers);
            started = lockedSession.GameStarted;
        });

        if(started){
            return RedirectToAction("Index", "Game");
        } else {
            ViewData["Session"] = this.Cookies.Session;
            ViewData["MyName"] = this.Cookies.PlayerName;
            ViewData["Hosting"] = hosting;
            return View();
        }
        
    }

    [InvalidSessionExceptionFilter]
    public IActionResult LobbyState(){
        var session = this.DataService.GetSession(this.Cookies.Session);
        var players = new List<string>();
        var started = false; 
        session.LockSession(lockedSession => {
            players.AddRange(lockedSession.RegisteredPlayers);
            started = lockedSession.GameStarted;
        });
        return Json(new LobbyStateResponseModel(players, started));
    }

    [InvalidSessionExceptionFilter]
    public IActionResult StartGame(){

        // Determine players
        var session = this.DataService.GetSession(this.Cookies.Session);
        var players = new List<string>();
        session.LockSession(lockedSession => players.AddRange(lockedSession.RegisteredPlayers));

        // Create a game
        var game = CreateInitialGameState(players);
        session.SetGameState(game);

        return Ok();

    }

    private GameState CreateInitialGameState(List<string> playerNames){
        var players = playerNames.Select(o => new Player(o)).ToList();
        var game = new GameState(players);
        var assigner = new RoleAssigner(Random.Shared);
        var shuffler = new Shuffler(Random.Shared);
        assigner.AssignRoles(game);
        shuffler.Shuffle(game);
        return game;
    }

}
