using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class DeckTest {

    [TestMethod]
    public void ShuffleChangesDeckOrder() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = new GameState();
        shuffler.Shuffle(game);
        Assert.IsFalse(Enumerable.SequenceEqual(new Deck().Content, game.Deck.Content));
    }
    
    [TestMethod]
    public void ShuffleRetainsDeckSize() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = new GameState();
        shuffler.Shuffle(game);
        Assert.AreEqual(new Deck().Content.Count, game.Deck.Content.Count);
    }
    
    [TestMethod]
    public void ShuffleKeepsPolicyCount() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = new GameState();
        shuffler.Shuffle(game);
        Assert.AreEqual(
            new Deck().Content.Count(o => o == Policy.Liberal),
            game.Deck.Content.Count(o => o == Policy.Liberal));
    }

}