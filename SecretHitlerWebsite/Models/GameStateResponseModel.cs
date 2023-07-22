using SecretHitler;

namespace SecretHitlerWebsite.Models;

public class GameStateResponse {

    public List<PlayerModel> Players { get; set; }
    public int DrawPileSize { get; set; }
    public int DiscardPileSize { get; set; }
    public int LiberalPolicyPassed { get; set; }
    public int FascistPolicyPassed { get; set; }
    public string[] Hand { get; set; }
    public string CurrentPlayer { get; set; }
    public string HasTheFloor { get; set; }
    public string CurrentPlayerRole { get; set; }

    public GameStateResponse(Session.ILockedSession session, string currentPlayerName){
        if(session.Game is GameState game){
            this.Players = game.Players.Select(o => new PlayerModel(o, game.Votes[o])).ToList();
            this.DrawPileSize = game.Deck.Count;
            this.DiscardPileSize = game.Discard.Count;
            this.LiberalPolicyPassed = game.LiberalPolicyPassed;
            this.FascistPolicyPassed = game.FascistPolicyPassed;
            this.Hand = game.Hand.Clone().Select(PolicyName).ToArray();
            this.CurrentPlayer = currentPlayerName;
            this.HasTheFloor = game.HasTheFloor.Name;
            this.CurrentPlayerRole = PlayerRoleName(game, currentPlayerName);
        } else {
            throw new InvalidOperationException("The game has not started.");
        }
    }

    private string PolicyName(Policy policy){
        return Enum.GetName(policy)
            ?? throw new ArgumentException($"Unrecognised policy {policy}.");
    }

    private string PlayerRoleName(GameState game, string playerName){
        var player = game.Players.Single(o => o.Name == playerName);
        var role = game.Roles[player];
        if(role.IsHitler){
            return "Hitler";
        } else if(role.IsConfederate){
            return "Fascist";
        } else {
            return "Liberal";
        }
    }

    public class PlayerModel {
        public string Name { get; set; }
        public string Vote { get; set; }
        public PlayerModel(Player player, Vote vote){
            this.Name = player.Name;
            this.Vote = Enum.GetName(vote)
                ?? throw new ArgumentException($"Unknown vote {vote}");
        }
    }
    
}