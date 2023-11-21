const keys = require('./config/keys.js');

const express = require('express');
const app = express();

// db setup
const mongoose = require('mongoose');
mongoose.connect(keys.mongoURI);

// db models
require('./model/Account');

// routes
require('./routes/authenticationRoutes')(app);

app.listen(keys.port, () => {
	console.log('listening on port ' + keys.port);
});