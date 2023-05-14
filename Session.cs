
namespace SecretHitler;

public class Session {

    public static Dictionary<Guid, Session> Sessions { get; private set; }

    static Session (){
        Sessions = new Dictionary<Guid, Session>();
    }

    public static Session New(string [] names){
        var newSession = new Session(names);
        Sessions.Add(newSession.Id, newSession);
        return newSession;
    }

    public static Session Get(Guid id){
        if (Sessions.TryGetValue(id, out var session)){
            return session;
        } else {
            throw new InvalidOperationException($"There is no active session {id}");
        }
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public Dictionary<Minister, string> Names { get; private set; }
    public Parliament Parliament { get; set; }
    public RoleDistribution Roles { get; set; }

    private Session(string[] names){
        this.Id = Guid.NewGuid();
        this.CreatedAt = DateTime.Now;
        this.Names = new Dictionary<Minister, string>();
        
        // Create ministers
        var ministers = new List<Minister>();
        for(var i = 0; i < names.Length; i++){
            var minister = new Minister();
            ministers.Add(minister);
            this.Names[minister] = names[i];
        }
        this.Parliament = new Parliament(ministers);

        // Assign roles
        this.Roles = new RoleDistribution(Random.Shared, this.Parliament);

    }

    public Minister[] GetConfederates(){
        return this.Parliament.Ministers
            .Where(o => this.Roles.Is(o, Role.Confederate))
            .ToArray();
    }

    public Minister GetHitler(){
        return this.Parliament.Ministers.Single(o => this.Roles.Is(o, Role.Hitler));
    }

}