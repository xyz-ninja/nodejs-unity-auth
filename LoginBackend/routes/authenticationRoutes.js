const mongoose = require('mongoose');
const Account = mongoose.model('accounts');

module.exports = app => {
	app.get('/account', async (request, response) => {
		const {username, password} = request.query;

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

			response.send("New account created successfully");

			return;
		
		} else {
			if (userAccount.password == password) {

				userAccount.lastAuthentification = Date.now();

				await userAccount.save();

				response.send("Login successful");

			} else {
				
				response.send("Login failed");
			}
		}
	});
}