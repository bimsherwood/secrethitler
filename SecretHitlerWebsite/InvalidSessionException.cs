
namespace SecretHitlerWebsite;

public class InvalidSessionException : Exception {
    public InvalidSessionException(string message) : base(message) { }
}