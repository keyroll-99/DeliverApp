CREATE TABLE DeployScripts (
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    create_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_at DATETIME NULL,
    name varchar(255),
    folder varchar (255)
);