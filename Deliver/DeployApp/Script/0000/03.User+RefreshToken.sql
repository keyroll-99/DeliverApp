ALTER TABLE RefreshTokens
	ADD CONSTRAINT	FK_RefreshToken_User_Id
	FOREIGN KEY (UserId) REFERENCES "Users"(Id);