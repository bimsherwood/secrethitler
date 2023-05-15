
namespace SecretHitler;

public class Minister {
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public Minister(string name) {
        this.Id = Guid.NewGuid();
        this.Name = name;
    }

    public Minister(Guid id, string name) {
        this.Id = id;
        this.Name = name;
    }

    public override string ToString() {
        return this.Name;
    }

    public override bool Equals(object? obj) {
        return obj is Minister minister &&
               Id.Equals(minister.Id);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Id);
    }
}