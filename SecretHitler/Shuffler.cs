namespace SecretHitler;

public class Shuffler {

    private Random Random;

    public Shuffler(Random rand){
        this.Random = rand;
    }

    // Fisher-Yates
    public void Shuffle(GameState game){
        for (int i = game.Deck.Content.Count - 1; i > 0; i--) {

            // Pick a random index from 0 to i
            int j = this.Random.Next(1, i+1);
    
            // Swap arr[i] with the element at random index
            var a = game.Deck.Content[i];
            var b = game.Deck.Content[j];
            game.Deck.Content[i] = b;
            game.Deck.Content[j] = a;

        }
    }

}
