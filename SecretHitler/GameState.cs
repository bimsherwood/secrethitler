namespace SecretHitler;

public class GameState {

    public Pile Deck { get; set; }
    public Pile Discard { get; set; }
    public Pile Hand { get; set; }
    public int FascistPolicyPassed { get; set; }
    public int LiberalPolicyPassed { get; set; }

    public GameState(){
        this.Deck = Pile.UnshuffledDeck();
        this.Discard = Pile.Empty();
        this.Hand = Pile.Empty();
    }

}
