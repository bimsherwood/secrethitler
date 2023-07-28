namespace SecretHitlerWebsite.Models;

public class LobbyStateResponseModel {

    public bool GameStarted { get; set; }
    public List<string> Players { get; set; }

    public LobbyStateResponseModel(List<string> players, bool gameStarted){
        this.Players = players;
        this.GameStarted = gameStarted;
    }
    
}
