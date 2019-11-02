import { Socket } from "net";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../generated/communication_protocol_pb";

export abstract class RequestHandler<T> {
    protected payload: T;
    socket: Socket;
    constructor(payload: T, socket: Socket)
    {
        this.payload = payload;
        this.socket = socket;
    }
    protected abstract OnHandle(response: OperationResponse): Promise<OperationResponse>;
    Handle(requestCode: protocol.OperationRequestCode): Promise<OperationResponse>
    {
        var responseBody: protocol.OperationResponse = new protocol.OperationResponse();
        var response: any = new OperationResponse(this.socket, responseBody);
        responseBody.setRequestCode(requestCode);
        return this.OnHandle(response);
    }
}
