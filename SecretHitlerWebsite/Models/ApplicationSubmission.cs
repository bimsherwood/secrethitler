namespace SecretHitlerWebsite.Controllers;

public class ApplicationSubmission {
    public string PlayerName { get; set; }
    public string SessionKey { get; set; }
    public ApplicationSubmission(){
        this.PlayerName = string.Empty;
        this.SessionKey = string.Empty;
    }
}