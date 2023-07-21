namespace SecretHitler;

public class Shuffler {

    private Random Random;

    public Shuffler(Random rand){
        this.Random = rand;
    }

    public void Shuffle(GameState game){
        var buffer = game.Deck.RemoveAll();
        FischerYates.Shuffle(this.Random, buffer);
        game.Deck.AddToTop(buffer);
    }

    public void ShuffleDiscardIntoDeck(GameState game){
        var discards = game.Discard.RemoveAll();
        game.Deck.AddToBottom(discards);
        this.Shuffle(game);
    }

}
