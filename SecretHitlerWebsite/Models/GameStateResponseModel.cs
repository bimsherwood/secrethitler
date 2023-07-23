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
        var game = session.Game;
        this.Players = game.Players.Select(o => MapPlayer(game, o, currentPlayerName)).ToList();
        this.DrawPileSize = game.Deck.Count;
        this.DiscardPileSize = game.Discard.Count;
        this.LiberalPolicyPassed = game.LiberalPolicyPassed;
        this.FascistPolicyPassed = game.FascistPolicyPassed;
        this.Hand = game.Hand.Clone().Select(PolicyName).ToArray();
        this.CurrentPlayer = currentPlayerName;
        this.HasTheFloor = game.HasTheFloor.Name;
        this.CurrentPlayerRole = PlayerRoleName(game, currentPlayerName);
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

    private PlayerModel MapPlayer(GameState game, Player player, string currentPlayerName){

        Vote visibleVote;
        var voteIsVisible = player.Name == currentPlayerName
            || game.Votes.Values.All(o => o != Vote.Undecided);
        if(voteIsVisible){
            visibleVote = game.Votes[player];
        } else {
            visibleVote = Vote.Undecided;
        }

        string? alliance;
        var playerRole = game.Roles[player];
        var currentPlayer = game.Players.Single(o => o.Name == currentPlayerName);
        var currentPlayerRole = game.Roles[currentPlayer];
        var allianceIsVisible = currentPlayerRole.IsConfederate
            || (currentPlayerRole.IsHitler && game.Players.Count < 7);
        if(allianceIsVisible){
            if(playerRole.IsHitler){
                alliance = "H";
            } else if (playerRole.IsConfederate){
                alliance = "F";
            } else {
                alliance = null;
            }
        } else {
            alliance = null;
        }

        var model = new PlayerModel(player, visibleVote, alliance);
        return model;

    }

    public class PlayerModel {
        public string Name { get; set; }
        public string Vote { get; set; }
        public string? Alliance { get; set; }
        public PlayerModel(Player player, Vote vote, string? alliance){
            this.Name = player.Name;
            this.Vote = Enum.GetName(vote)
                ?? throw new ArgumentException($"Unknown vote {vote}");
            this.Alliance = alliance;
        }
    }
    
}