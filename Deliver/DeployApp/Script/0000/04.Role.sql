﻿CREATE TABLE Roles(
	Id BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CreateTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	UpdateTime DATETIME,
	"Name" VARCHAR(25)
);
