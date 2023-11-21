const mongoose = require('mongoose');

const { Schema } = mongoose;

const accountSchema = new Schema({
	username : String,
	password : String,

	lastAuthentification: Date
});

mongoose.model('accounts', accountSchema);