using Microsoft.AspNetCore.SignalR;

namespace SecretHitlerWebsite.Hubs;

public class GameHub : Hub {

    private readonly Cookies Cookies;

    public GameHub (Cookies cookies){
        this.Cookies = cookies;
    }

    public async Task NotifyUpdate(string message) {
        var notifier = this.Cookies.PlayerName;
        var notification = $"{notifier}: {message}";
        await Clients.All.SendAsync("ReceiveUpdate", notification);
    }
}