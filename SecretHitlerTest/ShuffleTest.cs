using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class ShuffleTest {

    [TestMethod]
    public void ShuffleChangesDeckOrder() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = new GameState();
        shuffler.Shuffle(game);
        Assert.IsFalse(Enumerable.SequenceEqual(Pile.UnshuffledDeck().Content, game.Deck.Content));
    }
    
    [TestMethod]
    public void ShuffleRetainsDeckSize() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = new GameState();
        shuffler.Shuffle(game);
        Assert.AreEqual(Pile.UnshuffledDeck().Content.Count, game.Deck.Content.Count);
    }
    
    [TestMethod]
    public void ShuffleKeepsPolicyCount() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = new GameState();
        shuffler.Shuffle(game);
        Assert.AreEqual(
            Pile.UnshuffledDeck().Content.Count(o => o == Policy.Liberal),
            game.Deck.Content.Count(o => o == Policy.Liberal));
    }

}