SET IDENTITY_INSERT "PermissionActionEnum" ON

INSERT INTO "PermissionActionEnum"
	("Id", "Name")
VALUES
	(1, 'Create'),
	(2, 'Update'),
	(3, 'Get'),
	(4, 'Delete'),
	(5, 'Assing');

SET IDENTITY_INSERT "PermissionActionEnum" OFF