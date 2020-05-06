import * as protocol from "../../generated/communication_protocol_pb";
import { SessionStore } from "../Session";
import { Socket } from "net";

//Broadcasts given event to all clients
export class EventBroadcaster
{
    constructor(sessionStore: SessionStore)
    {
        this.sessionStore = sessionStore;
    }
    private sessionStore: SessionStore;

    public BroadcastEvent(sender: Socket, event: protocol.EventData)
    {
        var command: protocol.Command = new protocol.Command();
        command.setType(protocol.CommandType.EVENT);
        var serializedEvent = event.serializeBinary();
        command.setPayload(serializedEvent);

        this.sessionStore.Sessions.forEach(_ =>
        {
            if (_.Client == sender)
            {
                return;
            }
            _.Client.write(command.serializeBinary());
        });
    }
}

