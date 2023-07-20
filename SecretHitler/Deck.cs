namespace SecretHitler;

public class Pile {
    
    public List<Policy> Content { get; set; }

    private Pile(){
        this.Content = new List<Policy>();
    }

    public static Pile UnshuffledDeck(){
        var deck = new Pile();
        deck.Content = new List<Policy>();
        deck.Content.AddRange(Enumerable.Repeat(Policy.Fascist, 11));
        deck.Content.AddRange(Enumerable.Repeat(Policy.Liberal, 6));
        return deck;
    }

    public static Pile Empty(){
        return new Pile();
    }

}
