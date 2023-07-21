namespace SecretHitler;

public class Shuffler {

    private Random Random;

    public Shuffler(Random rand){
        this.Random = rand;
    }

    // Fisher-Yates
    public void Shuffle(GameState game){
        var buffer = game.Deck.RemoveAll();
        for (int i = buffer.Count - 1; i > 0; i--) {

            // Pick a random index from 0 to i
            int j = this.Random.Next(1, i+1);
    
            // Swap arr[i] with the element at random index
            var a = buffer[i];
            var b = buffer[j];
            buffer[i] = b;
            buffer[j] = a;

        }
        game.Deck.AddToTop(buffer);
    }

    public void ShuffleDiscardIntoDeck(GameState game){
        var discards = game.Discard.RemoveAll();
        game.Deck.AddToBottom(discards);
        this.Shuffle(game);
    }

}
