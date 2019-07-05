const express = require('express');

const app = express();

app.get("/", function(req, res) 
{
  console.log("Hey");
});

module.exports = app;