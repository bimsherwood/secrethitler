using Microsoft.AspNetCore.SignalR;
using SecretHitlerWebsite.Models;

namespace SecretHitlerWebsite;

public class GameHub : Hub {
    public async Task NotifyUpdate() {
        await Clients.All.SendAsync("ReceiveUpdate");
    }
}