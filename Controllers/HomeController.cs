using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecretHitler.Models;

namespace SecretHitler.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger) {
        _logger = logger;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult NewSession(){
        var viewModel = new NewSessionViewModel();
        return View(viewModel);
    }

    public IActionResult SetMinisters(NewSessionViewModel viewModel){
        var validNames = viewModel.MinisterNames
            .Where(o => !string.IsNullOrWhiteSpace(o))
            .Select(o => o.Trim())
            .ToArray();
        if(validNames.Length < 5){
            ModelState.Clear();
            viewModel.Error = "You must have at least 5 players";
            viewModel.MinisterNames.RemoveAll(string.IsNullOrWhiteSpace);
            return View("NewSession", viewModel);
        }
        var session = Session.New(validNames);
        return RedirectToAction("MinisterSelection", "Game", new { sessionId = session.Id });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
