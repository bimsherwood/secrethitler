namespace SecretHitler.Models;

public class MinisterProfileViewModel {

    public Guid MinisterId { get; set; }
    public Role Role { get; set; }
    public Party Party { get; set; }
    public KnownFascist[] Confederates { get; set; }
    public KnownFascist Hitler { get; set; }
    public bool KnowsFascists { get; set; }

    // Deserialiser
    public MinisterProfileViewModel(){ }

    public MinisterProfileViewModel(Session session, Minister minister){
        
        if(session.Roles is null || session.Parliament is null){
            throw new ArgumentException("The session has not been set up.");
        }

        var confederates = session.GetConfederates();
        var hitler = session.GetHitler();

        this.MinisterId = minister.Id;
        this.Role = session.Roles.GetRole(minister);
        this.Party = session.Roles.GetParty(minister);
        this.Confederates = confederates.Select(o => new KnownFascist(session.Names[o])).ToArray();
        this.Hitler = new KnownFascist(session.Names[hitler]);
        this.KnowsFascists =
            this.Role == Role.Hitler && session.Parliament.Ministers.Count < 7 ||
            this.Role == Role.Confederate;

    }

    public class KnownFascist {
        public string Name { get; set; }
        public KnownFascist(string name) {
            Name = name;
        }
    }

}
