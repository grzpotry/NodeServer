var app = require('./app');
var net = require('net');
var fs: File = require('fs');
var protobuf = require("protobufjs");

//CommunicationProtocol.js contains protobuf generated classes with protocol structures
//CommunicationProtocol.d.ts contains interfaces for static-typing purposes
import * as protocol from "./generated/communication_protocol_pb";
import { OperationResponse } from "./OperationResponse";
import { RequestHandlerProvider } from "./request_handlers/RequestHandlerProvider";
import { Socket } from "net";
import { SessionStore, Session } from "./Session";

var server = net.Server(function (socket: any)
{
    //TODO: identify client by id received during handshake
    // clientSessions.set(clientId, socket);

    //Node automatically creates and send socket when client connects
    socket.on('connection', function (socket: Socket)
    {
        console.log("Client connected");
    });

    //Data received from client
    socket.on('data', function (data: any)
    {
        var session: Session | null = sessionStore.TryGetSession(socket);
        //TODO: check if client is part of current session
        console.log(`Received data: ${data}`)
        var command = protocol.Command.deserializeBinary(data);

        if (command == undefined)
        {
            throw new Error(`Received unrecognized command, only operation requests are expected - mistmached protocols?`)
        }

        var commandType = command.getType();
        if (commandType !== protocol.CommandType.OP_REQUEST)
        {
            throw new Error(`Received not expected command type: ${commandType}`);
        }

        var request = protocol.OperationRequest.deserializeBinary(command.getPayload());
        var requestCode = request.getRequestCode();

        if (session == null && requestCode != protocol.OperationRequestCode.HANDSHAKE)
        {
            console.log(`Not expected request for not connected socket: ${requestCode}`);
            return;
        }
        else if (session != null)
        {
            session.OnRequest(request);
        }

        HandleOperationRequest(request, socket)
            .then((_) => SendOperationResponse(_, socket))
            .catch(e => 
            {
                console.log(`Error occured while processing operation request: ${e}`);
            });
    });

    socket.on('end', function ()
    {
        sessionStore.RemoveSession(socket);
    });
});

server.on('listening', function ()
{
    console.log(`Main server listening...`);
});

let port = 3000;
server.listen(port);

let sessionStore: SessionStore = new SessionStore();
let requestHandlerProvider: RequestHandlerProvider = new RequestHandlerProvider(sessionStore);

//TODO: It should be static typed, but for whatever reason  ts-protoc generates only interface definitions without appropriate getters and setters
//currently generated .d.ts interfaces are not compatible with generated .js prototypes (mismatched camel case)
function HandleOperationRequest(operationRequest: any, socket: any, callback?: Error): Promise<OperationResponse>
{
    var requestCode = operationRequest.getRequestCode();
    if (requestCode == undefined)
    {
        throw new Error(`Undefined request code`);
    }

    var requestHandler = requestHandlerProvider.GetHandler(requestCode);
    return requestHandler.Handle(operationRequest.getPayload(), socket, requestCode);
}

function SendOperationResponse(response: OperationResponse, socket: Socket)
{
    console.log(`sending response for request ${response.body.getRequestCode()}`);
    var command: protocol.Command = new protocol.Command();
    command.setType(protocol.CommandType.OP_RESPONSE);
    command.setPayload(response.body.serializeBinary());
    socket.write(command.serializeBinary());
}

