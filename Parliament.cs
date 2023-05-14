
namespace SecretHitler;

public class Parliament {
    
    public List<Minister> Ministers { get; private set; }

    public Parliament(IEnumerable<Minister> ministers){
        this.Ministers = ministers.ToList();
    }

}