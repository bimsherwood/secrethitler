namespace SecretHitler;

public class Drawer {

    public bool TryDraw(GameState game, int count){
        if (game.Deck.Count >= count){
            var drawn = game.Deck.RemoveFromTop(count);
            game.Hand.AddToBottom(drawn);
            return true;
        } else {
            return false;
        }
    }

    public void MaybeShuffleThenDraw(GameState game, Shuffler shuffler, int count){
        if(game.Deck.Count < count){
            shuffler.ShuffleDiscardIntoDeck(game);
        }
        if (!TryDraw(game, count)){
            throw new InvalidOperationException("There are not enough cards in the deck to draw.");
        }
    }

    public bool ReplaceOnDeck(GameState game, int index){
        if(index < game.Hand.Count){
            var replaced = game.Hand.RemoveAt(index);
            game.Deck.AddToTop(replaced);
            return true;
        } else {
            return false;
        }
    }

    public void Discard(GameState game, int index){
        var policy = game.Hand.RemoveAt(index);
        game.Discard.AddToTop(policy);
    }

    public bool TryUnDiscard(GameState game){
        if(game.Discard.Count > 0){
            var drawn = game.Discard.RemoveFromTop();
            game.Hand.AddToBottom(drawn);
            return true;
        } else {
            return false;
        }
    }

}