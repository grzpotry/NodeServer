import { HandshakeHandler } from "./HandshakeHandler";
import * as protocol from "../generated/communication_protocol_pb";

// Provides request handler for given OperationRequestCode
export class RequestHandlerProvider
{
    constructor()
    {
        this.handlers.set(protocol.OperationRequestCode.HANDSHAKE, new HandshakeHandler());
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
