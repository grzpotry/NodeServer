
import * as protocol from "../generated/communication_protocol_pb";

//Wraps operation response which is sent to client
export class OperationResponse
{
    body: protocol.OperationResponse;
    constructor(body: protocol.OperationResponse)
    {
        this.body = body;
    }
}
