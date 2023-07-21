namespace SecretHitler;

public class Pile {
    
    private List<Policy> Content { get; set; }

    private Pile(){
        this.Content = new List<Policy>();
    }

    private Pile(List<Policy> content){
        this.Content = content;
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

    public int Count => this.Content.Count;
    
    public List<Policy> RemoveFromTop(int count){
        var top = this.Content.Take(count).ToList();
        this.Content = this.Content.Skip(count).ToList();
        return top;
    }

    public Policy RemoveAt(int index){
        var policy = this.Content[index];
        this.Content.RemoveAt(index);
        return policy;
    }

    public List<Policy> RemoveAll(){
        var top = this.Content.ToList();
        this.Content = new List<Policy>();
        return top;
    }

    public void AddToTop(Policy policy){
        this.Content = new[]{policy}.Concat(this.Content).ToList();
    }

    public void AddToTop(List<Policy> policies){
        this.Content = policies.Concat(this.Content).ToList();
    }

    public void AddToBottom(List<Policy> policies){
        this.Content.AddRange(policies);
    }

    public List<Policy> Clone(){
        return this.Content.ToList();
    }

}
