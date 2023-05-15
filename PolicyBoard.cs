
namespace SecretHitler;

public class PolicyBoard {
    
    private Random Random;
    private Stack<PolicyType> Draw;
    private List<PolicyType> Hand;
    private List<PolicyType> Discard;
    private List<PolicyType> Passed;

    public PolicyBoard(Random rng){

        this.Random = rng;

        // Create an initial policy deck
        var policyList = new List<PolicyType>();
        policyList.AddRange(Enumerable.Repeat(PolicyType.Fascist, 11));
        policyList.AddRange(Enumerable.Repeat(PolicyType.Liberal, 6));
        this.Shuffle(policyList);

        this.Draw = new Stack<PolicyType>(policyList);
        this.Hand = new List<PolicyType>();
        this.Discard = new List<PolicyType>();
        this.Passed = new List<PolicyType>();

    }

    public List<PolicyType> PeekThree(){
        return new List<PolicyType>() {
            this.Draw.ElementAt(0),
            this.Draw.ElementAt(1),
            this.Draw.ElementAt(2)
        };
    }

    public List<PolicyType> GetHand(){
        return this.Hand.ToList();
    }

    public void DrawThree(){
        this.Hand.Add(this.Draw.Pop());
        this.Hand.Add(this.Draw.Pop());
        this.Hand.Add(this.Draw.Pop());
    }

    public void DiscardFromHand(int index){
        var item = this.Hand[index];
        this.Hand.RemoveAt(index);
        this.Discard.Add(item);
    }

    public void PlayFromHandDiscardRest(int index){

        // Play from hand
        var played = this.Hand[index];
        this.Hand.RemoveAt(index);
        this.Passed.Add(played);

        // Discard the rest
        this.Discard.AddRange(this.Hand);
        this.Hand.Clear();

    }

    public void PlayTopPolicyFromDeck(){
        var played = this.Draw.Pop();
        this.Passed.Add(played);
    }

    public void ShuffleInDiscardsIfNecessary(){
        if(this.Draw.Count < 3){
            var combinedPiles = this.Draw.Concat(this.Discard).ToList();
            Shuffle(combinedPiles);
            this.Draw.Clear();
            foreach(var policy in combinedPiles){
                this.Draw.Push(policy);
            }
        }
    }

    public void Unplay(PolicyType type){
        if(this.Passed.Any(o => o.Equals(type))){
            this.Passed.Remove(type);
            this.Discard.Add(type);
        }
    }

    public int CountPassed(PolicyType type){
        return this.Passed.Count(o => o.Equals(type));
    }

    private void Shuffle<T>(IList<T> list) {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = this.Random.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}