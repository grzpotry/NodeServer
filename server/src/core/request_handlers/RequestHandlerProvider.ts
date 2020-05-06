import { HandshakeHandler } from "./HandshakeHandler";
import * as protocol from "../../generated/communication_protocol_pb";
import { SessionStore } from "../Session";
import { EventHandler } from "./EventHandler";
import { EventBroadcaster } from "./EventsBroadcaster";

// Provides request handler for given OperationRequestCode
export class RequestHandlerProvider
{
    constructor(session: SessionStore, broadcaster: EventBroadcaster)
    {
        //TODO: add default provider for other non-handled requests
        this.handlers.set(protocol.OperationRequestCode.HANDSHAKE, new HandshakeHandler(session, broadcaster));
        this.handlers.set(protocol.OperationRequestCode.RAISE_EVENT, new EventHandler(session, broadcaster));
    }

    public GetHandler(requestCode: protocol.OperationRequestCode): any
    {
        if (!this.handlers.has(requestCode))
        {
            throw new Error(`No handler for request ${requestCode} found`);
        }
        return this.handlers.get(requestCode);
    }

    private handlers: Map<protocol.OperationRequestCode, any> = new Map<protocol.OperationRequestCode, any>();
}
