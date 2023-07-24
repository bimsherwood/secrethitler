using Microsoft.AspNetCore.SignalR;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite;

public class GameHub : Hub {

    private readonly ILogger<GameHub> Logger;
    private readonly DataService DataService;
    private Cookies Cookies;

    public GameHub(
            ILogger<GameHub> logger,
            DataService dataService,
            Cookies cookies) {
        this.Logger = logger;
        this.DataService = dataService;
        this.Cookies = cookies;
    }

    public async Task NotifyUpdate() {
        var session = this.DataService.GetSession(this.Cookies.Session);
        GameStateResponse? response = null;
        session.LockSession(session => {
            response = new GameStateResponse(session, this.Cookies.PlayerName);
        });
        await Clients.All.SendAsync("ReceiveUpdate", response);
    }
}