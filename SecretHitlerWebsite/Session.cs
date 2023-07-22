using SecretHitler;

namespace SecretHitlerWebsite;

public class Session {
    
    public string Id { get; set; }
    private List<string> Players; 
    private GameState? GameState;
    private object Lock;
    
    public Session(string id){
        this.Id = id;
        this.Players = new List<string>();
        this.Lock = new Object();
    }

    public void SetGameState(GameState game){
        lock(this.Lock){
            this.GameState = game;
        }
    }

    public void LockSession(Action<ILockedSession> operation){
        lock(this.Lock){
            var lockedSession = new LockedSession(this.GameState, this.Players);
            operation(lockedSession);
        }
    }

    public interface ILockedSession {
        GameState? Game { get; }
        List<string> Players { get; }
        bool GameStarted { get; }
    }

    private class LockedSession : ILockedSession {
        public GameState? Game { get; set; }
        public List<string> Players { get; set; }
        public bool GameStarted => this.Game != null;
        public LockedSession(GameState? gameState, List<string> players){
            this.Game = gameState;
            this.Players = players;
        }
    }

}