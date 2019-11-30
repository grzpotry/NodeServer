import { Socket } from "net";

///TODO: manages clients
export class SessionStore
{
    public AddSession(session: Session)
    {
        if (this.sessions.has(session))
        {
            console.log(`Client ${session.Client.localAddress} session already exists `);
            return;
        }

        this.sessions.add(session);
        console.log(`Added client ${session.Client.localAddress} session. Active sessions: ${this.sessions.size}`)
    }

    public Exists(client: Socket): boolean
    {
        return this.TryGetSession(client) != null;
    }

    public TryGetSession(client: Socket): Session | null
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

    public RemoveSession(client: Socket): boolean
    {
        var session = this.TryGetSession(client);
        if (session == null)
        {
            return false;
        }
        var result = this.sessions.delete(session);
        if (result)
        {
            console.log(`Removed session ${client.localAddress}. Active sessions: ${this.sessions.entries.length}`);
        }
        return result;
    }
    private sessions: Set<Session> = new Set<Session>();
}

export class Session
{
    private close()
    {
        console.log(`Closed session`);
        this.Client.end();
        this.sessionStore.RemoveSession(this.Client);
    }

    public Write(data: any)
    {
        this.Tick();
        this.Client.write(data);
    }

    public OnRequest(request: Request)
    {
        this.Tick();
    }

    constructor(client: Socket, store: SessionStore)
    {
        this.Client = client;

        //TODO: disconnect client after timeout
        this.sessionStore = store;
        this.Tick();
    }

    public Client: Socket;
    private sessionStore: SessionStore;
    private timeoutHandler: any;
    private timeoutMs: number = 2000;

    private OnTimeoutReached()
    {
        console.log(`Reached timeout due to inactivity, closing session`);
        if (this.timeoutHandler != undefined)
        {
            this.timeoutHandler.clearTimeout();
        }
        this.close();
    }

    private Tick()
    {
        if (this.timeoutHandler != undefined)
        {
            this.timeoutHandler.clearTimeout();
        }

        this.timeoutHandler = setTimeout(this.OnTimeoutReached, this.timeoutMs);
    }
}
