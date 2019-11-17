import { RequestHandler } from "./RequestHandler";
import { Config } from "../Config";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../generated/communication_protocol_pb";
import { Session } from "../Session";

//Handles Handshake operation request
export class HandshakeHandler extends RequestHandler<protocol.HandshakePayload> {
    constructor(session: Session)
    {
        super();
        this.session = session;
    }

    OnHandle(serializedPayload: any, response: OperationResponse)
    {
        var payload: protocol.HandshakePayload = protocol.HandshakePayload.deserializeBinary(serializedPayload);
        if (Config.ProtocolVersion !== payload.getProtocolVersion())
        {
            response.body.setResponseCode(protocol.OperationResponseCode.INVALID_PROTOCOL);
            return Promise.resolve(response);
        }
        response.body.setResponseCode(protocol.OperationResponseCode.HANDSHAKE_SUCCESS);
        this.session.AddClient(response.socket);
        return Promise.resolve(response);
    }

    private session: Session;
}