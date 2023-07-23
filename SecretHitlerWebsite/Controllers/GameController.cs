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
            var game = lockedSession.Game;
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
            var game = lockedSession.Game;
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

    public IActionResult DrawThree(){
        var drawer = new Drawer();
        var shuffler = new Shuffler(Random.Shared);
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            if(game.Hand.Count == 0){
                drawer.MaybeShuffleThenDraw(game, shuffler, 3);
            }
        });
        return RedirectToAction("GameState");
    }

    public IActionResult DrawFromDeck(){
        var drawer = new Drawer();
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            if(game.Hand.Count < 3){
                drawer.TryDraw(game, 1);
            }
        });
        return RedirectToAction("GameState");
    }

    public IActionResult ReplaceOnDeck(int index){
        var drawer = new Drawer();
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            drawer.ReplaceOnDeck(game, index);
        });
        return RedirectToAction("GameState");
    }

    public IActionResult Discard(int index){
        var drawer = new Drawer();
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            drawer.Discard(game, index);
        });
        return RedirectToAction("GameState");
    }

    public IActionResult UnDiscard(){
        var drawer = new Drawer();
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            drawer.TryUnDiscard(game);
        });
        return RedirectToAction("GameState");
    }

    public IActionResult PassPolicy(){
        var passer = new PolicyPasser();
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            if(game.Hand.Count == 1){
                passer.PassHand(game);
            }
        });
        return RedirectToAction("GameState");
    }

    public IActionResult PassTopPolicy(){
        var passer = new PolicyPasser();
        var shuffler = new Shuffler(Random.Shared);
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        });
        return RedirectToAction("GameState");
    }

    public IActionResult UndoPolicyToHand(string policy){
        var passer = new PolicyPasser();
        var session = this.DataService.GetSession(this.Cookies.Session);
        session.LockSession(lockedSession => {
            var game = lockedSession.Game;
            if(game.Hand.Count < 3){
                if(Enum.TryParse<Policy>(policy, out Policy parsedPolicy)){
                    passer.TryRevokeToHand(game, parsedPolicy);
                } else {
                    throw new ArgumentException($"Unknown policy {policy}");
                }
            }
        });
        return RedirectToAction("GameState");
    }

}