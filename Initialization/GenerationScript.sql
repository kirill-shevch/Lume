DROP TABLE IF EXISTS LumeDB.dbo.UserAuthEntities;

CREATE TABLE LumeDB.dbo.UserAuthEntities (
	UserId int IDENTITY(1,1) NOT NULL UNIQUE,
	UserUid uniqueidentifier NOT NULL UNIQUE,
	AccessKey nvarchar(50) NULL,
	RefreshKey nvarchar(50) NULL,
	ExpirationTime datetime2(7) NULL,
	TemporaryCode nvarchar(5) NULL,
	PhoneNumber nvarchar(20) NOT NULL UNIQUE,
	CONSTRAINT PK_UserId PRIMARY KEY CLUSTERED (UserId)
);