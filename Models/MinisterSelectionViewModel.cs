namespace SecretHitler.Models;

public class MinisterSelectionViewModel {

    public Guid SessionId { get; set; }
    public Guid? ChosenMinisterId { get; set; }
    public List<Minister> Ministers { get; set; }

    // Deserialiser
    public MinisterSelectionViewModel(){
        this.Ministers = new List<Minister>();
    }

    public MinisterSelectionViewModel(Session session){
        this.SessionId = session.Id;
        this.Ministers = session.Parliament.Ministers.ToList();
    }

}
