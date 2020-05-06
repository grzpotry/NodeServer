import { Socket } from "net";

///TODO: manages clients
export class SessionStore
{
    public AddSession(session: Session)
    {
        if (this.Exists(session.Client))
        {
            console.log(`Client ${session.Client.localAddress} session already exists `);
            return;
        }

        this.Sessions.add(session);
        console.log(`Added client ${session.Client.localAddress} session. Active sessions: ${this.Sessions.size}`)
    }

    public Exists(client: Socket): boolean
    {
        return this.TryGetSession(client) != null;
    }

    public GetSession(client: Socket): Session
    {
        var session: Session | null = null;

        this.Sessions.forEach(function (s)
        {
            if (s.Client == client)
            {
                session = s;
                return;
            }
        });

        if (session != null)
        {
            return session;
        }
        throw new Error(`Given socket ${client.remoteAddress} is not associated with any active session.`);
    }

    public TryGetSession(client: Socket): Session | null
    {
        try
        {
            return this.GetSession(client);
        }
        catch (e)
        {
            return null;
        }
    }

    public RemoveSession(client: Socket): boolean
    {
        var session = this.TryGetSession(client);
        if (session == null)
        {
            return false;
        }
        var result = this.Sessions.delete(session);
        if (result)
        {
            console.log(`Removed session. Active sessions: ${this.Sessions.entries.length}`);
        }
        return result;
    }


    //TODO: change to more optimized structure
    //TODO: privaate
    public Sessions: Set<Session> = new Set<Session>();
}

export class Session
{
    public Client: Socket;

    public Write(data: any)
    {
        this.Client.write(data);
    }

    public Close(reason: string)
    {
        console.log(`Closing session. Reason: ${reason}`);
        this.Client.end();
    }

    constructor(client: Socket)
    {
        this.Client = client;
        this.Client.setTimeout(this.timeoutMs);
    }

    private timeoutMs: number = 3600000;
}
