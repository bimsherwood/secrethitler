namespace SecretHitler;

public class RoleAssigner {

    private Random Random;

    public RoleAssigner(Random random){
        this.Random = random;
    }

    public void AssignRoles(GameState game){

        if(game.Players.Count < 5 || game.Players.Count > 10){
            throw new InvalidOperationException("There must be between 5 and 10 players.");
        }

        var confederateCount = (int)Math.Floor((game.Players.Count - 3) / 2d);
        var players = game.Players.ToList();
        FischerYates.Shuffle(this.Random, players);

        // Pick one hitler
        game.Roles[players[0]] = PlayerRole.Hitler();

        // Pick some confederates
        for(int i = 1; i < confederateCount + 1; i++){
            game.Roles[players[i]] = PlayerRole.Confederate();
        }

        // The rest are liberals
        for(int i = confederateCount + 1; i < players.Count; i++){
            game.Roles[players[i]] = PlayerRole.Liberal();
        }

    }

}