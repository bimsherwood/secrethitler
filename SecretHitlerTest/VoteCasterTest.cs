using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class VoteCasterTest {

    private GameState InitialGameState() => new GameState(new[]{ "Aaron", "Ben", "Connor", "Denise", "Erin" }.Select(o => new Player(o)).ToList());

    [TestMethod]
    public void CastingVoteCounts() {
        var caster = new VoteCaster();
        var game = InitialGameState();
        var firstPlayer = game.Players.First();
        caster.SubmitVote(game, firstPlayer, Vote.Yes);
        Assert.AreEqual(game.Votes.Values.Where(o => o == Vote.Yes).Count(), 1);
    }

    [TestMethod]
    public void ClearingVotesWorks() {
        var caster = new VoteCaster();
        var game = InitialGameState();
        var firstPlayer = game.Players.First();
        caster.SubmitVote(game, firstPlayer, Vote.Yes);
        caster.ClearVotes(game);
        Assert.AreEqual(game.Votes.Values.Where(o => o != Vote.Undecided).Count(), 0);
    }
    
}