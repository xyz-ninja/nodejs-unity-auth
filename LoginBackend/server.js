const keys = require('./config/keys.js');

const express = require('express');
const mongoose = require('mongoose');

mongoose.connect(keys.mongoURI);

const app = express();

app.get('/auth', async (request, response) => {
	response.send("Hello world!");
});


app.listen(keys.port, () => {
	console.log('listening on port ' + keys.port);
});