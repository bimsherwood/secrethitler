
namespace SecretHitler;

public class GameMessage {
    public string Heading { get; set; }
    public string Content { get; set; }
    public GameMessage(string heading, string content) {
        this.Heading = heading;
        this.Content = content;
    }
}