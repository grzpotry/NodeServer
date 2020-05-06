import { RequestHandler } from "./RequestHandler";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../../generated/communication_protocol_pb";
import { SessionStore } from "../Session";
import { Socket } from "net";
import { EventBroadcaster } from "./EventsBroadcaster";

//Handles events received from clients
export class EventHandler extends RequestHandler<protocol.EventData> {
    constructor(sessionStore: SessionStore, broadcaster: EventBroadcaster)
    {
        super(sessionStore, broadcaster);
    }

    OnHandle(serializedPayload: any, socket: Socket, response: OperationResponse)
    {
        var eventData: protocol.EventData = protocol.EventData.deserializeBinary(serializedPayload);

        response.body.setResponseCode(protocol.OperationResponseCode.EVENT_BROADCASTED);

        //TODO: validate payload if it is safe to broadcast to all clients
        this.Broadcaster.BroadcastEvent(socket, eventData);

        return Promise.resolve(response);
    }
}