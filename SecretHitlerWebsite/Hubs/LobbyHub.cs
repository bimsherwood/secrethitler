using Microsoft.AspNetCore.SignalR;

namespace SecretHitlerWebsite.Hubs;

public class LobbyHub : Hub {
    public async Task NotifyUpdate() {
        await Clients.All.SendAsync("ReceiveUpdate");
    }
}