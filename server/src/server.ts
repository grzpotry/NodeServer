var app = require('./app');
var net = require('net');
var fs: File = require('fs');
let root = require('./CommunicationProtocol'); //TODO: Find a way to import protocol gen files with type definitions


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