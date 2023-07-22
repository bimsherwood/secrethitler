using SecretHitler;

namespace SecretHitlerWebsite;

public class Session {
    
    public string Id { get; set; }
    private List<string> RegisteredPlayers; 
    private GameState? Game;
    private object Lock;
    
    public Session(string id){
        this.Id = id;
        this.RegisteredPlayers = new List<string>();
        this.Lock = new Object();
    }

    public void SetGameState(GameState game){
        lock(this.Lock){
            this.Game = game;
        }
    }

    public void LockSession(Action<ILockedSession> operation){
        lock(this.Lock){
            var lockedSession = new LockedSession(this.Game, this.RegisteredPlayers);
            operation(lockedSession);
        }
    }

    public interface ILockedSession {
        GameState? Game { get; }
        List<string> RegisteredPlayers { get; }
        bool GameStarted { get; }
    }

    private class LockedSession : ILockedSession {
        public GameState? Game { get; set; }
        public List<string> RegisteredPlayers { get; set; }
        public bool GameStarted => this.Game != null;
        public LockedSession(GameState? game, List<string> registeredPlayers){
            this.Game = game;
            this.RegisteredPlayers = registeredPlayers;
        }
    }

}