using System.Collections.Concurrent;

namespace SecretHitlerWebsite;

public class DataService {
    
    private static ConcurrentDictionary<string, Session> Sessions;

    static DataService(){
        Sessions = new ConcurrentDictionary<string, Session>();
    }

    public bool TryFindSession(string id, out Session? session){
        return Sessions.TryGetValue(id, out session);
    }

    public Session CreateSession(string id){
        var session = new Session(id);
        Sessions[id] = session;
        return session;
    }

}