using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitler;
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
    
    public IActionResult CastVote(string vote){
        var caster = new VoteCaster();
        var session = this.DataService.GetSession(this.Cookies.Session);
        var playerName = this.Cookies.PlayerName;
        session.LockSession(lockedSession => {
            var game = lockedSession.Game
                ?? throw new InvalidOperationException("The game has not started.");
            var targetPlayer = game.Players.FirstOrDefault(o => o.Name == playerName)
                ?? throw new InvalidOperationException($"Player {playerName} does not exist.");
            if(Enum.TryParse<Vote>(vote, out Vote parsedVote)){
                if(parsedVote == Vote.Undecided){
                    caster.ClearVotes(game);
                } else {
                    caster.CastVote(game, targetPlayer, parsedVote);
                }
            } else {
                throw new ArgumentException($"Unknown vote {vote}");
            }
        });
        return RedirectToAction("GameState");
    }

}