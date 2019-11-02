import { Socket } from "net";
import { OperationResponse } from "../OperationResponse";
import * as protocol from "../generated/communication_protocol_pb";

//Base class for all request handlers
export abstract class RequestHandler<T>
{
    public Handle(serializedPayload: any, socket: Socket, requestCode: protocol.OperationRequestCode): Promise<OperationResponse>
    {
        var responseBody: protocol.OperationResponse = new protocol.OperationResponse();
        var response: any = new OperationResponse(socket, responseBody);
        responseBody.setRequestCode(requestCode);
        return this.OnHandle(serializedPayload, response);
    }

    protected abstract OnHandle(payload: any, response: OperationResponse): Promise<OperationResponse>;
}
