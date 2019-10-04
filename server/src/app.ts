

const express = require('express');
const responseTime = require('response-time');
const fileSystem = require("fs");
const path = require("path");

var app = express();


//Create a middleware that adds a X-Response-Time header to responses.
app.use(responseTime());


app.use((request: { headers: any; }, response: any, next: () => void) => 
{
  console.log(request.headers)
  next()
})

app.get("/", function (req: Request, res: any) 
{

  console.log(`Roger that babyyyyy`);
  //console.log(fileSystem.readdirSync('./'));
  res.sendFile(path.resolve('./test_input.txt'));

  // let data = fileSystem.readFile('./test_input.txt', (error: Error, data: string) =>
  // {
  //   console.log(`Data: ${data.toString()}`);
  // });
  // console.log(`Mission in progress`);
});








module.exports = app;