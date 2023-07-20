using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class DrawTest {

    [TestMethod]
    public void DrawThreeSucceeds() {
        var drawer = new Drawer();
        var game = new GameState();
        Assert.IsTrue(drawer.TryDraw(game,3));
    }

    [TestMethod]
    public void DrawThreeFillsHand() {
        var drawer = new Drawer();
        var game = new GameState();
        drawer.TryDraw(game,3);
        Assert.AreEqual(game.Hand.Content.Count, 3);
    }

    [TestMethod]
    public void DrawThreeDepletesDeck() {
        var drawer = new Drawer();
        var game = new GameState();
        var deckSizeBefore = game.Deck.Content.Count;
        drawer.TryDraw(game,3);
        var deckSizeAfter = game.Deck.Content.Count;
        Assert.AreEqual(deckSizeBefore, deckSizeAfter + 3);
    }
    
    [TestMethod]
    public void DrawTooMuchThrows() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var drawer = new Drawer();
        var game = new GameState();
        Assert.ThrowsException<InvalidOperationException>(() => drawer.MaybeShuffleThenDraw(game, shuffler, 100));
    }

    [TestMethod]
    public void DrawRecycles() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var drawer = new Drawer();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        for(var i = 0; i < 100; i++){
            drawer.MaybeShuffleThenDraw(game, shuffler, 3);
            drawer.DiscardHand(game);
        }
        Assert.AreEqual(game.Discard.Content.Count + game.Deck.Content.Count, initialDeckSize);
    }

    [TestMethod]
    public void DiscardSingleCanEmptyHand() {
        var drawer = new Drawer();
        var game = new GameState();
        var initialDeckSize = game.Deck.Content.Count;
        drawer.TryDraw(game, 6);
        for(var i = 0; i < 6; i++){
            drawer.DiscardOne(game, 0);
        }
        Assert.AreEqual(game.Hand.Content.Count, 0);
        Assert.AreEqual(game.Discard.Content.Count + game.Deck.Content.Count, initialDeckSize);
    }

}