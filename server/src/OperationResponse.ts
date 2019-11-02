import { Socket } from "net";
import * as protocol from "./generated/communication_protocol_pb";
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
