namespace SecretHitler;

public class VoteCaster {

    public void ClearVotes(GameState game){
        foreach(var player in game.Players){
            game.Votes[player] = Vote.Undecided;
        }
    }

    public void CastVote(GameState game, Player player, Vote vote){
        game.Votes[player] = vote;
    }

}