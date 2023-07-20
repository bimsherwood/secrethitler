namespace SecretHitler;

public class PolicyPasser {

    private bool TryPassTopPolicy(GameState game){
        if(game.Deck.Content.Count > 0){
            var topPolicy = game.Deck.Content[0];
            game.Deck.Content.RemoveAt(0);
            if(topPolicy == Policy.Fascist){
                game.FascistPolicyPassed++;
            } else {
                game.LiberalPolicyPassed++;
            }
            return true;
        } else {
            return false;
        }
    }

    public void MaybeShuffleThenPassTopPolicy(GameState game, Shuffler shuffler){
        if(game.Deck.Content.Count == 0){
            shuffler.ShuffleDiscardIntoDeck(game);
        }
        if(!this.TryPassTopPolicy(game)){
            throw new InvalidOperationException("There are no policies left in the deck or discard pile.");
        }
    }

}