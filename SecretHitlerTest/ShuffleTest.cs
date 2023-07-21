using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class ShuffleTest {

    private GameState InitialGameState() => new GameState(new[]{ "Aaron", "Ben", "Connor", "Denise", "Erin" }.Select(o => new Player(o)).ToList());

    [TestMethod]
    public void ShuffleChangesDeckOrder() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = InitialGameState();
        shuffler.Shuffle(game);
        Assert.IsFalse(Enumerable.SequenceEqual(Pile.UnshuffledDeck().Clone(), game.Deck.Clone()));
    }
    
    [TestMethod]
    public void ShuffleRetainsDeckSize() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = InitialGameState();
        shuffler.Shuffle(game);
        Assert.AreEqual(Pile.UnshuffledDeck().Count, game.Deck.Count);
    }
    
    [TestMethod]
    public void ShuffleKeepsPolicyCount() {
        var rand = new Random(1234);
        var shuffler = new Shuffler(rand);
        var game = InitialGameState();
        shuffler.Shuffle(game);
        Assert.AreEqual(
            Pile.UnshuffledDeck().Clone().Count(o => o == Policy.Liberal),
            game.Deck.Clone().Count(o => o == Policy.Liberal));
    }

}