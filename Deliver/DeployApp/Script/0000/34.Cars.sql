ALTER TABLE Cars
	ADD CompanyId NOT NULL BIGINT FOREIGN KEY REFERENCES "Company"(Id);