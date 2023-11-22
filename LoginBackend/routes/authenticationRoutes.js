const mongoose = require('mongoose');
const Account = mongoose.model('accounts');

module.exports = app => {
	app.post('/account/login', async (request, response) => {
		const {username, password} = request.body;

		console.log(username);
		console.log(password);

		if (username == null || password == null) {
			response.send("Invalid username or password");
			return;
		}

		let userAccount = await Account.findOne({username: username});

		if (userAccount != null) {
			if (userAccount.password == password) {

				userAccount.lastAuthentification = Date.now();

				await userAccount.save();

				response.send(userAccount);
			
			} else {

				response.send("Login failed");
			}
		}
	});

	app.post('/account/create', async (request, response) => {
		const {username, password} = request.body;

		console.log(username);
		console.log(password);

		if (username == null || password == null) {
			response.send("Invalid username or password");
			return;
		}

		let userAccount = await Account.findOne({username: username});

		if (userAccount == null) {
			console.log("Create a new account");

			let newAccount = new Account({
				username : username,
				password : password,
				lastAuthentification: Date.now()
			});

			await newAccount.save();

			response.send(newAccount);

			return;
		
		} else {
			response.send("User already exists");
		}
	});
}