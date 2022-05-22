SET IDENTITY_INSERT "Roles" ON

INSERT INTO "Roles"
	(Id, CreateTime, "Name")
VALUES
	(6, getdate(), 'Dispatcher');

SET IDENTITY_INSERT "Roles" OFF