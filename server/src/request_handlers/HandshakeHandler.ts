import { RequestHandler } from "./RequestHandler";
import { Config } from "../Config";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../generated/communication_protocol_pb";

//Handles Handshake operation request
export class HandshakeHandler extends RequestHandler<protocol.HandshakePayload> {
    OnHandle(serializedPayload: any, response: OperationResponse)
    {
        var payload: protocol.HandshakePayload = protocol.HandshakePayload.deserializeBinary(serializedPayload);
        if (Config.ProtocolVersion !== payload.getProtocolVersion())
        {
            response.body.setResponseCode(protocol.OperationResponseCode.INVALID_PROTOCOL);
            return Promise.resolve(response);
        }
        response.body.setResponseCode(protocol.OperationResponseCode.HANDSHAKE_SUCCESS);
        return Promise.resolve(response);
    }
}