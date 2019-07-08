var app = require('./app');
var net = require('net');
var fs: File = require('fs');

app.set('port', process.env.PORT || 3010);

const appServer = app.listen(app.get('port'), () => 
{
    console.log(`Application listening on  ${appServer.address().port}`);
});

var connectedSockets: any[] = [];

// Create a TCP socket listener
var server = net.Server(function (socket: any) 
{ 
    console.log("Client connected");
    console.log(Object.keys(socket));

        // 'data' is an event that means that a message was just sent by the 
        // client application
        socket.on('data', function (data: any) 
        {
            console.log(`received ${data}`)
        });

        socket.on('end', function ()
        {
            console.log("client disconnected");
        }); 
});

server.on('listening', function() 
{
    console.log(`Main server listening...`);
});

let port = 3000;
server.listen(port);

let connectedClientsById: Map<string, Client> = new Map<string, Client>();
//module.exports.connectedClients = connectedClientsById;

class Client
{
    public get Id(): string
    {
        return this.client.id;
    }

    public constructor(client: any)
    {
        this.client = client;
    }

    private client: any;
}