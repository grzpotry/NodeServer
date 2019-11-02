import { Socket } from "net";
import * as protocol from "./generated/communication_protocol_pb";

//Wraps operation response which is sent to client
export class OperationResponse
{
    socket: Socket;
    body: protocol.OperationResponse;
    constructor(socket: Socket, body: protocol.OperationResponse)
    {
        this.body = body;
        this.socket = socket;
    }
}
