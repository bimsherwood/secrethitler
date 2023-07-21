namespace SecretHitler;

public class Player {
    
    public string Name { get; set; }

    public Player(string name){
        this.Name = name;
    }

    public override bool Equals(object? obj) {
        return obj is Player player &&
               Name == player.Name;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Name);
    }

    public override string? ToString() {
        return this.Name.ToString();
    }
    
}