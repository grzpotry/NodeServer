import { Socket } from "net";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../../generated/communication_protocol_pb";
import { SessionStore } from "../Session";
import { EventBroadcaster } from "./EventsBroadcaster";

//Base class for all request handlers
export abstract class RequestHandler<T>
{
    constructor(session: SessionStore, broadcaster: EventBroadcaster)
    {
        this.SessionStore = session;
        this.Broadcaster = broadcaster;
    }

    public Handle(serializedPayload: any, socket: Socket, requestCode: protocol.OperationRequestCode): Promise<OperationResponse>
    {
        var responseBody: protocol.OperationResponse = new protocol.OperationResponse();
        var response: any = new OperationResponse(responseBody);
        responseBody.setRequestCode(requestCode);
        //TODO: if interface for IMessage would be appropriatery generated, it could be used to deserialize payload here in base class except derived classes
        return this.OnHandle(serializedPayload, socket, response);
    }

    protected abstract OnHandle(payload: any, socket: Socket, response: OperationResponse): Promise<OperationResponse>;
    protected SessionStore: SessionStore;
    protected Broadcaster: EventBroadcaster;
}
