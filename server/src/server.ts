
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
        var rawMessage = protocol.OperationRequest.deserializeBinary(data);
        var request: any = rawMessage.toObject();
        //CommunicationProtocol.OperationRequest except any

        //TODO: while style is mistmatched with generated code? (generated field name is in camelCase)
        if (request.requestCode == protocol.OperationRequestCode.Handshake)
        {
            console.log(`received HANDSHAKE`)
        }
        console.log(`received code: ${request.requestCode}`)
        console.log(`received ${data}`)
        socket.write('Hello baby, do you wanna play the game ?');
    });

    socket.on('end', function ()
    {
        console.log("client disconnected");
    });
});

server.on('listening', function ()
{
    console.log(`Main server listening...`);
});

let port = 3000;
server.listen(port);

let clientSessions: Map<string, any> = new Map<string, any>();
//module.exports.connectedClients = connectedClientsById;