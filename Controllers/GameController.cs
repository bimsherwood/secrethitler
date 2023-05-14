using Microsoft.AspNetCore.Mvc;
using SecretHitler.Models;

namespace SecretHitler.Controllers;

public class GameController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public GameController(ILogger<HomeController> logger) {
        _logger = logger;
    }

    // Join a game
    public IActionResult Index(Guid sessionId, Guid ministerId) {
        if(ministerId == Guid.Empty){
            return RedirectToAction("MinisterSelection", new { sessionId });
        }
        var session = Session.Get(sessionId);
        var minister = session.Parliament.Ministers.FirstOrDefault(o => o.Id == ministerId);
        if(minister is null) throw new Exception($"Minister {ministerId} does not exist.");
        var viewModel = new MinisterProfileViewModel(session, minister);
        return View(viewModel);
    }

    public IActionResult MinisterSelection(Guid sessionId){
        var session = Session.Get(sessionId);
        var viewModel = new MinisterSelectionViewModel(session);
        return View(viewModel);
    }

}