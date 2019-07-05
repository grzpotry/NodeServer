const express = require('express');

var app = express();

app.get("/", function(req: Request, res: Response) 
{
  console.log("Hey Hesysss");
});

module.exports = app;