namespace SecretHitler.Models;

public class GameStateUpdateResult {

    public List<Minister> Ministers;
    public Dictionary<PolicyType, int> PoliciesPassed;
    public List<GameMessage> Messages { get; set; }
    public Role Role { get; set; }
    public List<PolicyType>? Hand { get; set; }
    public List<PolicyType>? TopOfDeck { get; set; }

    public GameStateUpdateResult(){
        this.Ministers = new List<Minister>();
        this.PoliciesPassed = new Dictionary<PolicyType, int>();
        this.Messages = new List<GameMessage>();
        this.Role = Role.Liberal;
    }

    public GameStateUpdateResult(Minister minister, Session session){

        this.Ministers = session.Parliament.Ministers.ToList();
        this.PoliciesPassed = Enum.GetValues<PolicyType>().ToDictionary(o => o, session.PolicyBoard.CountPassed);
        this.Messages = session.Messages;
        this.Role = session.Roles.GetRole(minister);

        if(minister.Equals(session.MinisterViewingHand)){
            this.Hand = session.PolicyBoard.GetHand();
        } else {
            this.Hand = null;
        }

        if(minister.Equals(session.MinisterViewingTopOfDeck)){
            this.TopOfDeck = session.PolicyBoard.PeekThree();
        } else {
            this.TopOfDeck = null;
        }

    }

}