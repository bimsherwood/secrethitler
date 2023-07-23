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
        GameState Game { get; }
        List<string> RegisteredPlayers { get; }
        bool GameStarted { get; }
    }

    private class LockedSession : ILockedSession {
        private GameState? MaybeGame { get; set; }
        public List<string> RegisteredPlayers { get; set; }
        public GameState Game => this.MaybeGame ?? throw new InvalidOperationException("The game has not started.");
        public bool GameStarted => this.Game != null;
        public LockedSession(GameState? game, List<string> registeredPlayers){
            this.MaybeGame = game;
            this.RegisteredPlayers = registeredPlayers;
        }
    }

}