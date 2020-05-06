var net = require('net');
import { OperationResponse } from "./OperationResponse";
import { RequestHandlerProvider } from "./request_handlers/RequestHandlerProvider";
import { Socket } from "net";
import { SessionStore, Session } from "./Session";
import { EventBroadcaster } from "./request_handlers/EventsBroadcaster";

//CommunicationProtocol.js contains protobuf generated classes with protocol structures
//CommunicationProtocol.d.ts contains interfaces for static-typing purposes
import * as protocol from "../generated/communication_protocol_pb";

export class ServerCore
{
    public Listen(port: number)
    {
        this.server.listen(port);
    }

    constructor()
    {
        this.server = net.Server(function (socket: any)
        {
            socket.on('connection', function (socket: Socket)
            {
                console.log("Client connected");
            }.bind(this)); //In typescript in order to preserve lexical scope of 'this' we have to bind it explicitly

            //Data received from client
            socket.on('data', function (data: any)
            {
                console.log(`Received data: ${data}`);
                var command = protocol.Command.deserializeBinary(data);
                if (command == undefined)
                {
                    throw new Error(`Received unrecognized command, only operation requests are expected - mistmached protocols?`);
                }

                var commandType = command.getType();
                if (commandType !== protocol.CommandType.OP_REQUEST)
                {
                    throw new Error(`Received not expected command type: ${commandType}`);
                }

                var request = protocol.OperationRequest.deserializeBinary(command.getPayload());
                var requestCode = request.getRequestCode();
                var session: Session | null = this.sessionStore.TryGetSession(socket);

                if (session == null && requestCode != protocol.OperationRequestCode.HANDSHAKE)
                {
                    console.log(`Not expected request for not connected socket: ${requestCode}`);
                    return;
                }

                this.HandleOperationRequest(this.requestHandlerProvider, request, socket)
                    .then((_) => this.SendOperationResponse(_, socket))
                    .catch(e =>
                    {
                        console.log(`Error occured while processing operation request: ${e}`);
                    });
            }.bind(this));

            socket.on('end', function ()
            {
                console.log(`Closed connection with ${socket.remoteAddress}`);
                let session = this.sessionStore.GetSession(socket);

                var event = new protocol.EventData();
                event.setCode(protocol.EventCode.CLIENT_LEFT);
                event.setPayload(session.UserData.serializeBinary());
                this.sessionStore.RemoveSession(socket);
                this.broadcaster.BroadcastEvent(socket, event);
            }.bind(this));

            socket.on('timeout', function ()
            {
                var session: Session = this.sessionStore.GetSession(socket);
                session.Close("Activity timeout reached.");
            }.bind(this));

        }.bind(this));

        this.server.on('listening', function ()
        {
            console.log(`Main server listening...`);
        });
    }

    private server: any;
    private sessionStore: SessionStore = new SessionStore();
    private broadcaster: EventBroadcaster = new EventBroadcaster(this.sessionStore);
    private requestHandlerProvider: RequestHandlerProvider = new RequestHandlerProvider(this.sessionStore, this.broadcaster);

    //TODO: It should be static typed, but for whatever reason  ts-protoc generates only interface definitions without appropriate getters and setters
    public HandleOperationRequest(requestHandlerProvider: RequestHandlerProvider, operationRequest: any, socket: any, callback?: Error): Promise<OperationResponse>
    {
        var requestCode = operationRequest.getRequestCode();
        if (requestCode == undefined)
        {
            throw new Error(`Undefined request code`);
        }
        var requestHandler = requestHandlerProvider.GetHandler(requestCode);
        return requestHandler.Handle(operationRequest.getPayload(), socket, requestCode);
    }

    public SendOperationResponse(response: OperationResponse, socket: Socket)
    {
        console.log(`sending response for request ${response.body.getRequestCode()}`);
        var command: protocol.Command = new protocol.Command();
        command.setType(protocol.CommandType.OP_RESPONSE);
        command.setPayload(response.body.serializeBinary());
        socket.write(command.serializeBinary());
    }
}
