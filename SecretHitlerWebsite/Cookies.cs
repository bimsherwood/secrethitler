
namespace SecretHitlerWebsite;

public class Cookies {
    
    const string SessionCookieName = "Session";
    const string PlayerNameCookieName = "PlayerName";

    private IHttpContextAccessor Http;

    public string Session {
        get {
            return this.Http.HttpContext?.Request.Cookies[SessionCookieName]
                ?? throw new InvalidSessionException($"Missing cookie {SessionCookieName}");
        }
        set {
            this.Http.HttpContext?.Response.Cookies.Delete(SessionCookieName);
            this.Http.HttpContext?.Response.Cookies.Append(SessionCookieName, value);
        }
    }

    public string PlayerName {
        get {
            return this.Http.HttpContext?.Request.Cookies[PlayerNameCookieName]
                ?? throw new InvalidSessionException($"Missing cookie {PlayerNameCookieName}");
        }
        set {
            this.Http.HttpContext?.Response.Cookies.Delete(PlayerNameCookieName);
            this.Http.HttpContext?.Response.Cookies.Append(PlayerNameCookieName, value);
        }
    }

    public Cookies(IHttpContextAccessor httpContext) {
        this.Http = httpContext;
    }

}