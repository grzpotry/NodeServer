import { Socket } from "net";

///TODO: manages clients
export class Session
{
    public AddClient(client: Socket)
    {
        if (this.clients.has(client))
        {
            console.log(`Client ${client.localAddress} already exists in session`);
            return;
        }

        console.log(`Client ${client.localAddress} added to session`)
        this.clients.add(client);
    }

    public RemoveClient(client: Socket)
    {
        if (!this.clients.has(client))
        {
            console.log(`Tried to remove client ${client.localAddress} which is not in session anymore`);
            return;
        }
        this.clients.delete(client);
    }
    private clients: Set<Socket> = new Set<Socket>();
}
