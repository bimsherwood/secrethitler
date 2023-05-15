namespace SecretHitler.Models;

public class MinisterProfileViewModel {

    public Guid MinisterId { get; set; }
    public Role Role { get; set; }
    public Party Party { get; set; }
    public List<Minister> Confederates { get; set; }
    public Minister Hitler { get; set; }
    public bool KnowsFascists { get; set; }

    // Deserialiser
    public MinisterProfileViewModel(){
        this.KnowsFascists = false;
        this.Confederates = new List<Minister>();
        this.Hitler = new Minister("UNKNOWN");
     }

    public MinisterProfileViewModel(Session session, Minister minister){
        
        if(session.Roles is null || session.Parliament is null){
            throw new ArgumentException("The session has not been set up.");
        }

        this.MinisterId = minister.Id;
        this.Role = session.Roles.GetRole(minister);
        this.Party = session.Roles.GetParty(minister);
        this.Confederates = session.GetConfederates();
        this.Hitler = session.GetHitler();
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
