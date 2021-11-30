const express = require("express");
const bodyParser = require("body-parser");
const cors = require("cors");
const db = require("./models");
const app = express();
const PORT = process.env.PORT || 8080;
const Role = db.role;

var corsOptions = {
  origin: "http://localhost:8081"
};

app.use(cors(corsOptions));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.get("/", (req, res) => {
  res.json({ message: "Hello, world!" });
});

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}.`);
});


require('./routes/auth.routes')(app);
require('./routes/user.routes')(app);

db.sequelize.sync({force: true}).then(() => {
	  console.log('Drop and Resync Db');
	  initial();
});


function initial() {
	  Role.create({
		      id: 1,
		      name: "user"
		    });
	 
	  Role.create({
		      id: 2,
		      name: "moderator"
		    });
	 
	  Role.create({
		      id: 3,
		      name: "admin"
		    });
}
