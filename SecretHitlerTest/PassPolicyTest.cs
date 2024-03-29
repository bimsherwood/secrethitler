using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class PassPolicyTest {

    private GameState InitialGameState() => new GameState(new[]{ "Aaron", "Ben", "Connor", "Denise", "Erin" }.Select(o => new Player(o)).ToList());

    [TestMethod]
    public void PassingTopPolicyMaintainsTotalPolicy() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        Assert.AreEqual(game.Deck.Count + game.FascistPolicyPassed + game.LiberalPolicyPassed, initialDeckSize);
    }

    [TestMethod]
    public void PassingTopPolicyIncreasesCounters() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        Assert.AreEqual(game.FascistPolicyPassed + game.LiberalPolicyPassed, 1);
    }

    [TestMethod]
    public void PassingTopPolicyDecreasesDeckSize() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        Assert.AreEqual(game.Deck.Count, initialDeckSize - 1);
    }

    [TestMethod]
    public void PassingTopPolicyExhaustsDeck() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var passer = new PolicyPasser();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        for(var i = 0; i < initialDeckSize; i++){
            passer.MaybeShuffleThenPassTopPolicy(game, shuffler);
        }
        Assert.ThrowsException<InvalidOperationException>(() => passer.MaybeShuffleThenPassTopPolicy(game, shuffler));
        Assert.AreEqual(game.Deck.Count, 0);
    }

    
    [TestMethod]
    public void PassingHandMaintainsTotalPolicy() {
        var passer = new PolicyPasser();
        var drawer = new Drawer();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        Assert.IsTrue(drawer.TryDraw(game, 3));
        passer.PassHand(game);
        Assert.AreEqual(game.Deck.Count + game.FascistPolicyPassed + game.LiberalPolicyPassed, initialDeckSize);
    }

    [TestMethod]
    public void PassingHandPolicyIncreasesCounters() {
        var passer = new PolicyPasser();
        var drawer = new Drawer();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        Assert.IsTrue(drawer.TryDraw(game, 3));
        passer.PassHand(game);
        Assert.AreEqual(game.FascistPolicyPassed + game.LiberalPolicyPassed, 3);
    }

    [TestMethod]
    public void PassingHandPolicyDecreasesDeckSize() {
        var passer = new PolicyPasser();
        var drawer = new Drawer();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        Assert.IsTrue(drawer.TryDraw(game, 3));
        passer.PassHand(game);
        Assert.AreEqual(game.Deck.Count, initialDeckSize - 3);
    }

}