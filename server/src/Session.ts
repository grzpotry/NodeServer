import { Socket } from "net";

///TODO: manages clients
export class SessionStore
{
    public AddSession(client: Socket)
    {
        if (this.Exists(client))
        {
            console.log(`Client ${client.localAddress} already exists in session`);
            return;
        }

        console.log(`Client ${client.localAddress} added to session`)
        this.sessions.add(new Session(client));
    }

    private Exists(client: Socket): boolean
    {
        return this.TryGetSession(client) != null;
    }

    private TryGetSession(client: Socket): Session | null
    {
        this.sessions.forEach(function (session)
        {
            if (session.Client == client)
            {
                return session;
            }
        });

        return null;
    }

    public RemoveSession(client: Socket)
    {
        var session = this.TryGetSession(client);
        if (session == null)
        {
            console.log(`Tried to remove client ${client.localAddress} which is not in session anymore`);
            return;
        }
        this.sessions.delete(session);
    }
    private sessions: Set<Session> = new Set<Session>();
}

export class Session
{
    constructor(client: Socket)
    {
        this.Client = client;
    }

    public Client: Socket;
}
