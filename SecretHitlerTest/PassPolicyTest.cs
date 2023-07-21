using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class PassPolicyTest {

    [TestMethod]
    public void PassingTopPolicyMaintainsTotalPolicy() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        Assert.AreEqual(game.Deck.Content.Count + game.FascistPolicyPassed + game.LiberalPolicyPassed, initialDeckSize);
    }

    [TestMethod]
    public void PassingTopPolicyIncreasesCounters() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        Assert.AreEqual(game.FascistPolicyPassed + game.LiberalPolicyPassed, 1);
    }

    [TestMethod]
    public void PassingTopPolicyDecreasesDeckSize() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        Assert.AreEqual(game.Deck.Content.Count, initialDeckSize - 1);
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

    
    [TestMethod]
    public void PassingHandMaintainsTotalPolicy() {
        var passer = new PolicyPasser();
        var drawer = new Drawer();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        Assert.IsTrue(drawer.TryDraw(game, 3));
        passer.PassHand(game);
        Assert.AreEqual(game.Deck.Content.Count + game.FascistPolicyPassed + game.LiberalPolicyPassed, initialDeckSize);
    }

    [TestMethod]
    public void PassingHandPolicyIncreasesCounters() {
        var passer = new PolicyPasser();
        var drawer = new Drawer();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        Assert.IsTrue(drawer.TryDraw(game, 3));
        passer.PassHand(game);
        Assert.AreEqual(game.FascistPolicyPassed + game.LiberalPolicyPassed, 3);
    }

    [TestMethod]
    public void PassingHandPolicyDecreasesDeckSize() {
        var passer = new PolicyPasser();
        var drawer = new Drawer();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        Assert.IsTrue(drawer.TryDraw(game, 3));
        passer.PassHand(game);
        Assert.AreEqual(game.Deck.Content.Count, initialDeckSize - 3);
    }

}