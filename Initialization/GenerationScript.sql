DROP TABLE IF EXISTS LumeDB.dbo.PersonAuth; 
DROP TABLE IF EXISTS LumeDB.dbo.PersonFriendList;
DROP TABLE IF EXISTS LumeDB.dbo.PersonToEvent;
DROP TABLE IF EXISTS LumeDB.dbo.PersonToChat;
DROP TABLE IF EXISTS LumeDB.dbo.Event;
DROP TABLE IF EXISTS LumeDB.dbo.EventType;
DROP TABLE IF EXISTS LumeDB.dbo.EventStatus;
DROP TABLE IF EXISTS LumeDB.dbo.Person;
DROP TABLE IF EXISTS LumeDB.dbo.ChatImageContent;
DROP TABLE IF EXISTS LumeDB.dbo.ChatMessage;
DROP TABLE IF EXISTS LumeDB.dbo.Chat;
DROP TABLE IF EXISTS LumeDB.dbo.PersonImageContent;
DROP TABLE IF EXISTS LumeDB.dbo.EventImageContent;

CREATE TABLE LumeDB.dbo.PersonAuth (
	PersonAuthId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	PersonUid uniqueidentifier NOT NULL UNIQUE,
	AccessKey nvarchar(50) NULL,
	RefreshKey nvarchar(50) NULL,
	ExpirationTime datetime2(7) NULL,
	TemporaryCode nvarchar(5) NULL,
	PhoneNumber nvarchar(20) NOT NULL UNIQUE,
	CONSTRAINT PK_PersonAuthId PRIMARY KEY CLUSTERED (PersonAuthId)
);

CREATE TABLE LumeDB.dbo.PersonImageContent (
	PersonImageContentId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	ContentHash nvarchar(200) NOT NULL UNIQUE,
	Content VARBINARY(MAX) NOT NULL,
	CONSTRAINT PK_PersonImageContentId PRIMARY KEY CLUSTERED (PersonImageContentId)
);

CREATE TABLE LumeDB.dbo.EventImageContent (
	EventImageContentId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	ContentHash nvarchar(200) NOT NULL UNIQUE,
	Content VARBINARY(MAX) NOT NULL,
	CONSTRAINT PK_EventImageContentId PRIMARY KEY CLUSTERED (EventImageContentId)
);

CREATE TABLE LumeDB.dbo.Person (
	PersonId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	PersonUid uniqueidentifier NOT NULL UNIQUE,
	Name nvarchar(200) NULL,
	Description nvarchar(800) NULL,
	Age tinyint NULL,
	PersonImageContentId bigint NULL,
	CONSTRAINT FK_Person_PersonImageContent FOREIGN KEY (PersonImageContentId) REFERENCES LumeDB.dbo.PersonImageContent (PersonImageContentId),
	CONSTRAINT PK_PersonId PRIMARY KEY CLUSTERED (PersonId)
);

CREATE TABLE LumeDB.dbo.EventType (
	EventTypeId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	EventTypeName nvarchar(100) NOT NULL UNIQUE,
	CONSTRAINT PK_EventTypeId PRIMARY KEY CLUSTERED (EventTypeId)
);

CREATE TABLE LumeDB.dbo.EventStatus (
	EventStatusId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	EventStatusName nvarchar(100) NOT NULL UNIQUE,
	CONSTRAINT PK_EventStatusId PRIMARY KEY CLUSTERED (EventStatusId)
);

CREATE TABLE LumeDB.dbo.Event (
	EventId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	Name nvarchar(200) NULL,
	MinAge tinyint NULL,
	MaxAge tinyint NULL,
	XCoordinate float NULL,
	YCoordinate float NULL,
	Description nvarchar(800) NULL,
	StartTime datetime2(7) NULL,
	EndTime datetime2(7) NULL,
	EventImageContentId bigint NULL,
	EventTypeId bigint NULL,
	EventStatusId bigint NULL,
	AdministratorId bigint NULL,
	CONSTRAINT FK_Event_EventImageContent FOREIGN KEY (EventImageContentId) REFERENCES LumeDB.dbo.EventImageContent (EventImageContentId),
	CONSTRAINT FK_Event_EventType FOREIGN KEY (EventTypeId) REFERENCES LumeDB.dbo.EventType (EventTypeId),
	CONSTRAINT FK_Event_EventStatus FOREIGN KEY (EventStatusId) REFERENCES LumeDB.dbo.EventStatus (EventStatusId),
	CONSTRAINT FK_Event_Person FOREIGN KEY (AdministratorId) REFERENCES LumeDB.dbo.Person (PersonId),
	CONSTRAINT PK_EventId PRIMARY KEY CLUSTERED (EventId)
);

CREATE TABLE LumeDB.dbo.Chat (
	ChatId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	Name nvarchar(200) NULL,
	CONSTRAINT PK_ChatId PRIMARY KEY CLUSTERED (ChatId)
);

CREATE TABLE LumeDB.dbo.ChatMessage (
	ChatMessageId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	Content nvarchar(4000) NULL,
	MessageTime datetime2(7) NULL,
	ChatId bigint NULL,
	CONSTRAINT FK_ChatMessage_Chat FOREIGN KEY (ChatId) REFERENCES LumeDB.dbo.Chat (ChatId),
	CONSTRAINT PK_ChatMessageId PRIMARY KEY CLUSTERED (ChatMessageId)
);

CREATE TABLE LumeDB.dbo.ChatImageContent (
	ChatImageContentId bigint IDENTITY(1,1) NOT NULL UNIQUE,
	ContentHash nvarchar(200) NOT NULL UNIQUE,
	Content VARBINARY(MAX) NOT NULL,
	ChatMessageId bigint NULL,
	CONSTRAINT PK_ChatImageContentId PRIMARY KEY CLUSTERED (ChatImageContentId),
	CONSTRAINT FK_ChatImageContent_ChatMessage FOREIGN KEY (ChatMessageId) REFERENCES LumeDB.dbo.ChatMessage (ChatMessageId)
);

CREATE TABLE LumeDB.dbo.PersonToEvent (
	PersonId bigint,
  	EventId bigint,
  	CONSTRAINT PK_Person_Event PRIMARY KEY (PersonId, EventId),
  	CONSTRAINT FK_Person_PersonToEvent FOREIGN KEY (PersonId) REFERENCES LumeDB.dbo.Person (PersonId),
  	CONSTRAINT FK_Event_PersonToEvent FOREIGN KEY (EventId) REFERENCES LumeDB.dbo.Event (EventId)
);

CREATE TABLE LumeDB.dbo.PersonToChat (
	PersonId bigint,
  	ChatId bigint,
  	CONSTRAINT PK_Person_Chat PRIMARY KEY (PersonId, ChatId),
  	CONSTRAINT FK_Person_PersonToChat FOREIGN KEY (PersonId) REFERENCES LumeDB.dbo.Person (PersonId),
  	CONSTRAINT FK_Chat_PersonToChat FOREIGN KEY (ChatId) REFERENCES LumeDB.dbo.Chat (ChatId)
);

CREATE TABLE LumeDB.dbo.PersonFriendList (
	PersonId bigint,
  	FriendId bigint,
  	CONSTRAINT PK_Person_Friend PRIMARY KEY (PersonId, FriendId),
  	CONSTRAINT FK_Person_PersonFriendList FOREIGN KEY (PersonId) REFERENCES LumeDB.dbo.Person (PersonId),
  	CONSTRAINT FK_Friend_PersonFriendList FOREIGN KEY (FriendId) REFERENCES LumeDB.dbo.Person (PersonId)
);