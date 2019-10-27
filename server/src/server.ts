
var app = require('./app');
var net = require('net');
var fs: File = require('fs');
var protobuf = require("protobufjs");

//CommunicationProtocol.js contains protobuf generated classes with protocol structures
//CommunicationProtocol.d.ts contains interfaces for static-typing purposes
import * as protocol from "./generated/communication_protocol_pb";

//TODO: serializacja i deserializacja eventów i operacji - IProtocol + Protocol18 - referencje z photona + notki z evernote
var tempPayload: CommunicationProtocol.HandshakePayload = new protocol.HandshakePayload()

app.set('port', process.env.PORT || 3010);

const appServer = app.listen(app.get('port'), () =>
{
    console.log(`Application listening on  ${appServer.address().port}`);
});


var connectedSockets: any[] = [];

var server = net.Server(function (socket: any)
{
    var clientId = socket.remoteAddress + ":" + socket.remotePort;
    //TODO: identify client by id received during handshake
    //TODO: establish protocol between client and server (header which indicates how to interpret message ? (eg. handshake, gamestate update etc.) - read about such protocols and existing solutions)
    clientSessions.set(clientId, socket);

    //Node automatically creates and send socket when client connects
    socket.on('connection', function (socket: any)
    {
        console.log("Client connected");
    });

    //Data received from client
    socket.on('data', function (data: any)
    {
        console.log(`Received data: ${data}`)
        var command = protocol.Command.deserializeBinary(data);

        if (command == undefined)
        {
            throw new Error(`Received unrecognized command, only operation requests are expected - mistmached protocols?`)
        }

        var commandType = command.getType();
        if (commandType !== protocol.CommandType.OP_REQUEST)
        {
            throw new Error(`Received not expected commant type: ${commandType}`);
        }

        var request = protocol.OperationRequest.deserializeBinary(command.getPayload());
        HandleOperationRequest(request, socket)
            .then(SendOperationResponse, error => 
            {
                console.log("Failed to send operation response");
            })
            .catch(e => 
            {
                console.log(`Error occured while processing operation request: ${e}`);
            });
    });

    socket.on('end', function ()
    {
        console.log("Client disconnected");
    });
});

server.on('listening', function ()
{
    console.log(`Main server listening...`);
});

let port = 3000;
let protocolVersion = 1;
server.listen(port);

let clientSessions: Map<string, any> = new Map<string, any>();


//TODO: It should be static typed, but for whatever reason  ts-protoc generates only interface definitions without appropriate getters and setters
//currently generated .d.ts interfaces are not compatible with generated .js prototypes (mismatched camel case)
function HandleOperationRequest(operationRequest: any, socket: any, callback?: Error): Promise<OperationResponse>
{
    var requestCode = operationRequest.getRequestCode();

    if (requestCode == undefined)
    {
        throw new Error(`Undefined request code`);
    }

    switch (requestCode)
    {
        case protocol.OperationRequestCode.HANDSHAKE:
            return HandleHandshake(operationRequest, socket);

        case protocol.OperationRequestCode.RAISE_EVENT:
            return HandleRaiseEvent(operationRequest);
        default:
            throw new Error(`Not supported request code: ${requestCode}`);
    }
}

//TODO: handshake should be static typed
function HandleHandshake(opRequest: any, socket: any): Promise<OperationResponse>
{
    var handshake = protocol.HandshakePayload.deserializeBinary(opRequest.getPayload());
    //TODO: Unify creating responses
    var request: protocol.OperationRequest = new protocol.OperationRequest();
    var response: any = new OperationResponse(socket, request);

    request.setRequestCode(opRequest.getRequestCode());

    if (protocolVersion !== handshake.getProtocolVersion())
    {
        request.setResponseCode(protocol.OperationResponseCode.INVALID_PROTOCOL);
        return Promise.resolve(response);
    }

    request.setResponseCode(protocol.OperationResponseCode.HANDSHAKE_SUCCESS);
    return Promise.resolve(response);
}

//TODO: raise event should be static typed
function HandleRaiseEvent(raiseEvent: any): Promise<OperationResponse>
{
    //TODO:
    return Promise.resolve(new OperationResponse(null, null));
}

function SendOperationResponse(response: OperationResponse)
{
    response.socket.write(response.request.serializeBinary());
}

class OperationResponse
{
    socket: any;
    request: protocol.OperationRequest;

    constructor(socket: any, request: protocol.OperationRequest)
    {
        this.request = request;
        this.socket = socket;
    }
}