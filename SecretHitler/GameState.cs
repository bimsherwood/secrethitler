namespace SecretHitler;

public class GameState {

    public Pile Deck { get; set; }
    public Pile Discard { get; set; }
    public Pile Hand { get; set; }
    public int FascistPolicyPassed { get; set; }
    public int LiberalPolicyPassed { get; set; }

    public List<Player> Players { get; set; }
    public Player HasTheFloor { get; set; }
    public Player Marked { get; set; }

    public Dictionary<Player, PlayerRole> Roles { get; set; }
    public Dictionary<Player, Vote> Votes { get; set; }

    public GameState(List<Player> players){
        this.Deck = Pile.UnshuffledDeck();
        this.Discard = Pile.Empty();
        this.Hand = Pile.Empty();
        this.Players = players.ToList();
        this.HasTheFloor = players.First();
        this.Marked = players.First();
        this.Roles = players.ToDictionary(o => o, o => PlayerRole.Liberal());
        this.Votes = players.ToDictionary(o => o, o => Vote.Undecided);
    }

}
