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

    public void ReplaceHandOnDeck(GameState game){
        var hand = game.Hand.RemoveAll();
        game.Deck.AddToTop(hand);
    }

    public void DiscardOne(GameState game, int index){
        var policy = game.Hand.RemoveAt(index);
        game.Discard.AddToTop(policy);
    }

    public void DiscardHand(GameState game){
        var hand = game.Hand.RemoveAll();
        game.Discard.AddToTop(hand);
    }

}