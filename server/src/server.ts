var app = require('./app');

app.set('port', process.env.PORT || 3000);

const server = app.listen(app.get('port'), () => 
{
    console.log(`Listening on  ${server.address().port}`);
});

var io = require('socket.io')(server);

console.log('waiting for connection');

io.on('connection', function(client: any)
{
    console.log('a user connected');

    client.on('join', function(data: any)
    {
        console.log(data);
    })
});

