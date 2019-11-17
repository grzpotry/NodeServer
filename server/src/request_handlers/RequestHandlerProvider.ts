import { HandshakeHandler } from "./HandshakeHandler";
import * as protocol from "../generated/communication_protocol_pb";
import { Session } from "../Session";

// Provides request handler for given OperationRequestCode
export class RequestHandlerProvider
{
    constructor(session: Session)
    {
        this.handlers.set(protocol.OperationRequestCode.HANDSHAKE, new HandshakeHandler(session));
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
