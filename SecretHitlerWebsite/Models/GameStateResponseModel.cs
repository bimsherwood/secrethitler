using SecretHitler;

namespace SecretHitlerWebsite.Models;

public class GameStateResponse {

    public List<string> Players { get; set; }
    public int DrawPileSize { get; set; }
    public int DiscardPileSize { get; set; }
    public int LiberalPolicyPassed { get; set; }
    public int FascistPolicyPassed { get; set; }
    public string[] Hand { get; set; }
    public string HasTheFloor { get; set; }

    public GameStateResponse(Session.ILockedSession session){
        if(session.GameState is GameState game){
            this.Players = game.Players.Select(o => o.Name).ToList();
            this.DrawPileSize = game.Deck.Count;
            this.DiscardPileSize = game.Discard.Count;
            this.LiberalPolicyPassed = game.LiberalPolicyPassed;
            this.FascistPolicyPassed = game.FascistPolicyPassed;
            this.Hand = game.Hand.Clone().Select(PolicyName).ToArray();
            this.HasTheFloor = game.HasTheFloor.Name;
        } else {
            throw new InvalidOperationException("The game has not started.");
        }
    }

    private string PolicyName(Policy policy){
        return Enum.GetName(policy)
            ?? throw new ArgumentException($"Unrecognised policy {policy}.");
    }
    
}