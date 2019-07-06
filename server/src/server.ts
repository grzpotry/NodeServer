var app = require('./app');


app.set('port', process.env.PORT || 3000);

const server = app.listen(app.get('port'), () => 
{
    console.log(`Listening on  ${server.address().port}`);
});

var io: WebSocket = require('socket.io')(server);

io.onopen = function()
{
    console.log('a user connected');
};