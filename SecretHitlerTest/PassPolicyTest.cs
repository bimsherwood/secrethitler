using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class PassPolicyTest {

    [TestMethod]
    public void PassingTopPolicyIncreasesCounters() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        Assert.AreEqual(game.Deck.Content.Count + game.FascistPolicyPassed + game.LiberalPolicyPassed, initialDeckSize);
    }

    [TestMethod]
    public void PassingTopPolicyExhaustsDeck() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        for(var i = 0; i < initialDeckSize; i++){
            passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        }
        Assert.ThrowsException<InvalidOperationException>(() => passer.MaybeShuffleThenPassTopPolicy(game, shuffler));
        Assert.AreEqual(game.Deck.Content.Count, 0);
    }

}