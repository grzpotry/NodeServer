var app = require('./app');

app.set('port', process.env.PORT || 3000);

// console.log(Object.keys(io));

// console.log('waiting for connection');

const server = app.listen(app.get('port'), () => 
{
    console.log(`Listening on  ${server.address().port}`);
});


let io = require("socket.io")(server);

io.on("connection", function(client: any)
{
    console.log('a user connected');

    client.on('join', function(data: any)
    {
        console.log(data);
    })
});
