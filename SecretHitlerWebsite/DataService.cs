using System.Collections.Concurrent;

namespace SecretHitlerWebsite;

public class DataService {
    
    private static ConcurrentDictionary<string, Session> Sessions;

    static DataService(){
        Sessions = new ConcurrentDictionary<string, Session>();
    }

    public bool TryGetSession(string id, out Session? session){
        return Sessions.TryGetValue(id, out session);
    }

    public Session GetSession(string id){
        if(Sessions.TryGetValue(id, out var session)){
            return session;
        } else {
            throw new InvalidSessionException("Your session does not exist.");
        }
    }

    public Session CreateSession(string id){
        var session = new Session(id);
        Sessions[id] = session;
        return session;
    }

}