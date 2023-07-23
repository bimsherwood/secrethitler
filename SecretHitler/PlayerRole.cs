namespace SecretHitler;

public class PlayerRole {

    public bool IsHitler { get; private set; }
    public bool IsFascist { get; private set; }
    public bool IsConfederate => this.IsFascist && !this.IsHitler;
    public bool IsLiberal => !this.IsFascist;

    private PlayerRole(){
        this.IsHitler = false;
        this.IsFascist = false;
    }

    public static PlayerRole Hitler(){
        return new PlayerRole(){
            IsHitler = true,
            IsFascist = true
        };
    }

    public static PlayerRole Confederate(){
        return new PlayerRole(){
            IsHitler = false,
            IsFascist = true
        };
    }
    
    public static PlayerRole Liberal(){
        return new PlayerRole(){
            IsHitler = false,
            IsFascist = false
        };
    }

}