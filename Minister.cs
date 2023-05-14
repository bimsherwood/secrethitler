
namespace SecretHitler;

public class Minister {
    
    public Guid Id { get; private set; }
    
    public Minister(){
        this.Id = Guid.NewGuid();
    }

    public Minister(Guid id){
        this.Id = id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Minister minister &&
               Id.Equals(minister.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}