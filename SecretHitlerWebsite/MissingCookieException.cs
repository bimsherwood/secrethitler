using SecretHitler;

namespace SecretHitlerWebsite;

public class MissingCookieException : Exception {
    public MissingCookieException(string cookieName) : base($"Missing session cookie {cookieName}") { }
}