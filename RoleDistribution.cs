
namespace SecretHitler;

public class RoleDistribution {

    private Minister Hitler;
    private Minister[] Confederates;

    public RoleDistribution(Random rng, Parliament parliament){

        // Members are drawn from the hat
        var hat = parliament.Ministers.ToList();

        // Determine Hitler
        var hitlerIndex = rng.Next(0, hat.Count);
        this.Hitler = hat[hitlerIndex];
        hat.Remove(this.Hitler);
        
        // Determine confederates
        var confederateCount = ((parliament.Ministers.Count - 1) / 2) - 1;
        this.Confederates = new Minister[confederateCount];
        for(var i = 0; i < confederateCount; i++){
            var confederateIndex = rng.Next(0, hat.Count);
            this.Confederates[i] = hat[confederateIndex];
            hat.Remove(this.Confederates[i]);
        }

    }

    public Role GetRole(Minister minister){
        if(minister.Equals(this.Hitler)){
            return Role.Hitler;
        } else if (this.Confederates.Contains(minister)){
            return Role.Confederate;
        } else {
            return Role.Liberal;
        }
    }

    public Party GetParty(Minister minister){
        if(minister.Equals(this.Hitler)){
            return Party.Fascist;
        } else if (this.Confederates.Contains(minister)){
            return Party.Fascist;
        } else {
            return Party.Liberal;
        }
    }

    public bool Is(Minister minister, Role role){
        return GetRole(minister).Equals(role);
    }

    public bool IsIn(Minister minister, Party party){
        return GetParty(minister).Equals(party);
    }

}