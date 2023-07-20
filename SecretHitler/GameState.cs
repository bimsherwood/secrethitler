namespace SecretHitler;

public class GameState {

    public Deck Deck { get; set; }

    public GameState(){
        this.Deck = new Deck();
    }

}
