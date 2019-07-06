var app = require('./app');

app.set('port', process.env.PORT || 3000);


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

const server = app.listen(app.get('port'), () => 
{
    console.log(`Listening on  ${server.address().port}`);
});

//TODO: refactor to plain 'net' - TCP socket listener, socket IO is not fully compatible with .NET (Websharp is able to connect but it is a bit cumbersome and not reliable)
let io = require("socket.io")(server);

let connectedClientsById: Map<string, Client> = new Map<string, Client>();
//module.exports.connectedClients = connectedClientsById;

io.on("connect", function(socket: any)
{  
    let client = socket.client;
    console.log(`Connected client: ${client.id}`);
    
    if (connectedClientsById.has(client.id))
    {
        throw new Error('Connected user already exists')
    }     

    connectedClientsById.set(client.id, client);
    console.log(`Clients:${connectedClientsById.size}`)

    socket.on('disconnect', () => 
    {
        console.log(`Disconnected client: ${client.id}`);
        if (!connectedClientsById.delete(client.id))
        {
            throw new Error('Failed to remove client');
        }
    });
});


io.use(function(socket: any, next: any) {
    var handshakeData = socket.request;
    console.log("TICK");

    // make sure the handshake data looks good as before
    // if error do this:
      // next(new Error('not authorized'));
    // else just call next
    next();
  });