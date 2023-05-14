namespace SecretHitler.Models;

public class NewSessionViewModel {

    public string? Error { get; set; }
    public List<string> MinisterNames { get; set; }

    // Deserialiser
    public NewSessionViewModel(){
        this.Error = null;
        this.MinisterNames = new List<string>(){ "" };
    }

}
