namespace SecretHitler;

public class Drawer {

    public bool TryDraw(GameState game, int count){
        if (game.Deck.Content.Count >= count){
            var drawn = game.Deck.Content.Take(count);
            var remaining = game.Deck.Content.Skip(count).ToList();
            game.Hand.Content.AddRange(drawn);
            game.Deck.Content = remaining;
            return true;
        } else {
            return false;
        }
    }

    public void MaybeShuffleThenDraw(GameState game, Shuffler shuffler, int count){
        if(game.Deck.Content.Count < count){
            shuffler.ShuffleDiscardIntoDeck(game);
        }
        if (!TryDraw(game, count)){
            throw new InvalidOperationException("There are not enough cards in the deck to draw.");
        }
    }

    public void DiscardOne(GameState game, int index){
        var discarded = game.Hand.Content[index];
        game.Hand.Content.RemoveAt(index);
        game.Discard.Content.Add(discarded);
    }

    public void DiscardHand(GameState game){
        game.Discard.Content.AddRange(game.Hand.Content);
        game.Hand.Content.Clear();
    }

}