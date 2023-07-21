using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class DrawTest {

    private GameState InitialGameState() => new GameState(new[]{ "Aaron", "Ben", "Connor", "Denise", "Erin" }.Select(o => new Player(o)).ToList());

    [TestMethod]
    public void DrawThreeSucceeds() {
        var drawer = new Drawer();
        var game = InitialGameState();
        Assert.IsTrue(drawer.TryDraw(game,3));
    }

    [TestMethod]
    public void DrawThreeFillsHand() {
        var drawer = new Drawer();
        var game = InitialGameState();
        drawer.TryDraw(game,3);
        Assert.AreEqual(game.Hand.Count, 3);
    }

    [TestMethod]
    public void DrawThreeDepletesDeck() {
        var drawer = new Drawer();
        var game = InitialGameState();
        var deckSizeBefore = game.Deck.Count;
        drawer.TryDraw(game,3);
        var deckSizeAfter = game.Deck.Count;
        Assert.AreEqual(deckSizeBefore, deckSizeAfter + 3);
    }
    
    [TestMethod]
    public void DrawTooMuchThrows() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var drawer = new Drawer();
        var game = InitialGameState();
        Assert.ThrowsException<InvalidOperationException>(() => drawer.MaybeShuffleThenDraw(game, shuffler, 100));
    }

    [TestMethod]
    public void DrawRecycles() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var drawer = new Drawer();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        for(var i = 0; i < 100; i++){
            drawer.MaybeShuffleThenDraw(game, shuffler, 3);
            drawer.DiscardHand(game);
        }
        Assert.AreEqual(game.Discard.Count + game.Deck.Count, initialDeckSize);
    }

    [TestMethod]
    public void DiscardSingleCanEmptyHand() {
        var drawer = new Drawer();
        var game = InitialGameState();
        var initialDeckSize = game.Deck.Count;
        drawer.TryDraw(game, 6);
        for(var i = 0; i < 6; i++){
            drawer.DiscardOne(game, 0);
        }
        Assert.AreEqual(game.Hand.Count, 0);
        Assert.AreEqual(game.Discard.Count + game.Deck.Count, initialDeckSize);
    }

    [TestMethod]
    public void ReplacingHandRevertsDeckStatus() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var drawer = new Drawer();
        var game = InitialGameState();
        shuffler.Shuffle(game);
        var deckBeforeDraw = game.Deck.Clone();
        Assert.IsTrue(drawer.TryDraw(game, 3));
        drawer.ReplaceHandOnDeck(game);
        Assert.IsTrue(game.Deck.Clone().SequenceEqual(deckBeforeDraw));
    }

}