namespace SecretHitler;

public class PolicyPasser {

    private void PassPolicy(GameState game, Policy policy){
        if(policy == Policy.Fascist){
            game.FascistPolicyPassed++;
        } else {
            game.LiberalPolicyPassed++;
        }
    }

    private bool TryPassTopPolicy(GameState game){
        if(game.Deck.Count > 0){
            var topPolicy = game.Deck.RemoveAt(0);
            PassPolicy(game, topPolicy);
            return true;
        } else {
            return false;
        }
    }

    private bool TryUnPassPolicy(GameState game, Policy policy){
        if(policy == Policy.Fascist){
            if(game.FascistPolicyPassed > 0){
                game.FascistPolicyPassed--;
                return true;
            } else {
                return false;
            }
        } else {
            if(game.LiberalPolicyPassed > 0){
                game.LiberalPolicyPassed--;
                return true;
            } else {
                return false;
            }
        }
    }

    public void MaybeShuffleThenPassTopPolicy(GameState game, Shuffler shuffler){
        if(game.Deck.Count == 0){
            shuffler.ShuffleDiscardIntoDeck(game);
        }
        if(!this.TryPassTopPolicy(game)){
            throw new InvalidOperationException("There are no policies left in the deck or discard pile.");
        }
    }

    public void PassHand(GameState game){
        while(game.Hand.Count > 0){
            var policies = game.Hand.RemoveAll();
            foreach (var policy in policies){
                PassPolicy(game, policy);
            }
        }
    }

    public bool TryRevokeToHand(GameState game, Policy policy){
        if(TryUnPassPolicy(game, policy)){
            game.Hand.AddToBottom(policy);
            return true;
        } else {
            return false;
        }
    }

}