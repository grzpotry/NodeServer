import { RequestHandler } from "./RequestHandler";
import { Config } from "../Config";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../../generated/communication_protocol_pb";
import { SessionStore, Session } from "../Session";
import { Socket } from "net";
import { EventBroadcaster } from "./EventsBroadcaster";

//Handles Handshake operation request
export class HandshakeHandler extends RequestHandler<protocol.HandshakePayload> {
    constructor(sessionStore: SessionStore, broadcaster: EventBroadcaster)
    {
        super(sessionStore, broadcaster);
    }

    OnHandle(serializedPayload: any, socket: Socket, response: OperationResponse)
    {
        var payload: protocol.HandshakePayload = protocol.HandshakePayload.deserializeBinary(serializedPayload);
        if (Config.ProtocolVersion !== payload.getProtocolVersion())
        {
            response.body.setResponseCode(protocol.OperationResponseCode.INVALID_PROTOCOL);
            return Promise.resolve(response);
        }

        var user = payload.getUser();
        console.log(`connected user: ` + user.getUsername());

        if (this.SessionStore.Exists(socket))
        {
            response.body.setResponseCode(protocol.OperationResponseCode.ALREADY_CONNECTED);
        }
        else
        {
            this.SessionStore.AddSession(new Session(socket, user));
            response.body.setResponseCode(protocol.OperationResponseCode.HANDSHAKE_SUCCESS);

            var event = new protocol.EventData();
            event.setPayload(user.serializeBinary());
            event.setCode(protocol.EventCode.CLIENT_JOINED);

            this.Broadcaster.BroadcastEvent(socket, event);
        }

        return Promise.resolve(response);
    }
}