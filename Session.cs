
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

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Parliament Parliament { get; private set; }
    public RoleDistribution Roles { get; private set; }
    public List<GameMessage> Messages { get; private set; }
    public PolicyBoard PolicyBoard { get; private set; }
    public Minister? MinisterViewingHand { get; private set; }
    public Minister? MinisterViewingTopOfDeck { get; private set; }

    private Session(string[] names){

        this.Id = Guid.NewGuid();
        this.CreatedAt = DateTime.Now;
        this.Messages = new List<GameMessage>();
        this.PolicyBoard = new PolicyBoard(Random.Shared);
        this.MinisterViewingHand = null;
        this.MinisterViewingTopOfDeck = null;
        
        // Create ministers
        var ministers = new List<Minister>();
        for(var i = 0; i < names.Length; i++){
            var minister = new Minister(names[i]);
            ministers.Add(minister);
        }
        this.Parliament = new Parliament(ministers);

        // Assign roles
        this.Roles = new RoleDistribution(Random.Shared, this.Parliament);

    }

    public List<Minister> GetConfederates(){
        return this.Parliament.Ministers
            .Where(o => this.Roles.Is(o, Role.Confederate))
            .ToList();
    }

    public Minister GetHitler(){
        return this.Parliament.Ministers.Single(o => this.Roles.Is(o, Role.Hitler));
    }

}