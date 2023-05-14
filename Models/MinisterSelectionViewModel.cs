namespace SecretHitler.Models;

public class MinisterSelectionViewModel {

    public Guid SessionId { get; set; }
    public Guid? ChosenMinisterId { get; set; }
    public List<KeyValuePair<Guid, string>> MinisterNames { get; set; }

    // Deserialiser
    public MinisterSelectionViewModel(){
        this.MinisterNames = new List<KeyValuePair<Guid, string>>();
    }

    public MinisterSelectionViewModel(Session session){

        if(session.Parliament is null){
            throw new ArgumentException("Cannot join an incomplete session.");
        }

        this.SessionId = session.Id;
        this.MinisterNames = session.Parliament.Ministers
            .Select(o => new KeyValuePair<Guid, string>(o.Id, session.Names[o]))
            .ToList();
        
    }

}
