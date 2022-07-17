﻿CREATE TABLE Cars(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CreateTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdateTime DATETIME,
	Hash UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	RegistrationNumber VARCHAR(12) NOT NULL,
	DriverId BIGINT FOREIGN KEY REFERENCES "Users"(Id)
);