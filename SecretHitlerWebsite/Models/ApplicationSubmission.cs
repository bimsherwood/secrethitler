using System.ComponentModel.DataAnnotations;

namespace SecretHitlerWebsite.Controllers;

public class ApplicationSubmission {

    [Required]
    public string PlayerName { get; set; }

    [Required]
    public string SessionKey { get; set; }

    public ApplicationSubmission(){
        this.PlayerName = string.Empty;
        this.SessionKey = string.Empty;
    }
}