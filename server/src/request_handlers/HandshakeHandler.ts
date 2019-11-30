import { RequestHandler } from "./RequestHandler";
import { Config } from "../Config";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../generated/communication_protocol_pb";
import { SessionStore, Session } from "../Session";
import { Socket } from "net";

//Handles Handshake operation request
export class HandshakeHandler extends RequestHandler<protocol.HandshakePayload> {
    constructor(session: SessionStore)
    {
        super();
        this.sessionStore = session;
    }

    OnHandle(serializedPayload: any, socket: Socket, response: OperationResponse)
    {
        var payload: protocol.HandshakePayload = protocol.HandshakePayload.deserializeBinary(serializedPayload);
        if (Config.ProtocolVersion !== payload.getProtocolVersion())
        {
            response.body.setResponseCode(protocol.OperationResponseCode.INVALID_PROTOCOL);
            return Promise.resolve(response);
        }
        response.body.setResponseCode(protocol.OperationResponseCode.HANDSHAKE_SUCCESS);
        this.sessionStore.AddSession(new Session(socket, this.sessionStore));
        return Promise.resolve(response);
    }

    private sessionStore: SessionStore;
}