using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite.Controllers;

public class GameController : Controller {
    
    private readonly ILogger<HomeController> Logger;
    private readonly DataService DataService;
    private readonly Cookies Cookies;

    public GameController(
            ILogger<HomeController> logger,
            DataService dataService,
            Cookies cookies) {
        this.Logger = logger;
        this.DataService = dataService;
        this.Cookies = cookies;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult GameState(){
        var session = this.DataService.GetSession(this.Cookies.Session);
        GameStateResponse? response = null;
        session.LockSession(session => {
            response = new GameStateResponse(session, this.Cookies.PlayerName);
        });
        return Json(response);
    }

    public IActionResult PassTheFloor(string playerName){
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game
                ?? throw new InvalidOperationException("The game has not started.");
            var targetPlayer = game.Players.FirstOrDefault(o => o.Name == playerName)
                ?? throw new InvalidOperationException($"Player {playerName} does not exist.");
            game.HasTheFloor = targetPlayer;
        });
        return RedirectToAction("GameState");
    }
    
}