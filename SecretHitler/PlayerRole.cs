namespace SecretHitler;

public class PlayerRole {

    public bool IsHitler { get; private set; }
    public bool IsNazi { get; private set; }
    public bool IsConfederate => this.IsNazi && !this.IsHitler;
    public bool IsLiberal => !this.IsNazi;

    private PlayerRole(){
        this.IsHitler = false;
        this.IsNazi = false;
    }

    public static PlayerRole Hitler(){
        return new PlayerRole(){
            IsHitler = true,
            IsNazi = true
        };
    }

    public static PlayerRole Confederate(){
        return new PlayerRole(){
            IsHitler = false,
            IsNazi = true
        };
    }
    
    public static PlayerRole Liberal(){
        return new PlayerRole(){
            IsHitler = false,
            IsNazi = false
        };
    }

}