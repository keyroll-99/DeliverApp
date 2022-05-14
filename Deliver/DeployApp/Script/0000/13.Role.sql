SET IDENTITY_INSERT "Roles" ON

INSERT INTO "Roles"
	(Id, CreateTime, "Name")
VALUES
	(1, getdate(), 'Admin'),
	(2, getdate(), 'CompanyAdmin'),
	(3, getdate(), 'HR'),
	(4, getdate(), 'CompanyOwner'),
	(5, getdate(), 'Driver');

SET IDENTITY_INSERT "Roles" OFF