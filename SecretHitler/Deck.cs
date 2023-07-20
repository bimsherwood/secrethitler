namespace SecretHitler;

public class Deck {
    public List<Policy> Content { get; set; }
    public Deck(){
        this.Content = new List<Policy>();
        this.Content.AddRange(Enumerable.Repeat(Policy.Fascist, 11));
        this.Content.AddRange(Enumerable.Repeat(Policy.Liberal, 6));
    }
}
