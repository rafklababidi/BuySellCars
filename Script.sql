CREATE DATABASE rafikLababidi;
GO
USE rafikLababidi;
GO

CREATE TABLE users
(
	id INT IDENTITY,
	name VARCHAR(50),
	email VARCHAR(50),
	username VARCHAR(50),
	password VARCHAR(50),
	isAdmin bit,
	CONSTRAINT pk_users PRIMARY KEY (id)
);

GO
CREATE TABLE cars
(
	id INT IDENTITY,
	fullName VARCHAR(50),
	email VARCHAR(50),
	contactNumber VARCHAR(50),
	vehicleDetails VARCHAR(250),
	price FLOAT,
	type VARCHAR(50),
	vehicleEngine VARCHAR(50),
	vehicleKm VARCHAR(50),
	imageFront VARCHAR(250),
	imageBack VARCHAR(250),
	isValid bit,
	CONSTRAINT pk_cars PRIMARY KEY (id)
);

GO
CREATE TABLE contact
(
	id INT IDENTITY,
	fullName VARCHAR(50),
	message TEXT,
	CONSTRAINT pk_contact PRIMARY KEY (id)
);