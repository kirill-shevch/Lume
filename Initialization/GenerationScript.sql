DROP TABLE IF EXISTS LumeDB.dbo.LumeUserAuth; 
DROP TABLE IF EXISTS LumeDB.dbo.LumeUserFriendList;
DROP TABLE IF EXISTS LumeDB.dbo.LumeUserToEvent;
DROP TABLE IF EXISTS LumeDB.dbo.LumeUserToChat;
DROP TABLE IF EXISTS LumeDB.dbo.LumeUser;
DROP TABLE IF EXISTS LumeDB.dbo.LumeChatMessage;
DROP TABLE IF EXISTS LumeDB.dbo.LumeChat;
DROP TABLE IF EXISTS LumeDB.dbo.LumeEvent;
DROP TABLE IF EXISTS LumeDB.dbo.LumeEventType;
DROP TABLE IF EXISTS LumeDB.dbo.LumeEventStatus;
DROP TABLE IF EXISTS LumeDB.dbo.LumeImageContent;

CREATE TABLE LumeDB.dbo.LumeUserAuth (
	UserAuthId int IDENTITY(1,1) NOT NULL UNIQUE,
	UserUid uniqueidentifier NOT NULL UNIQUE,
	AccessKey nvarchar(50) NULL,
	RefreshKey nvarchar(50) NULL,
	ExpirationTime datetime2(7) NULL,
	TemporaryCode nvarchar(5) NULL,
	PhoneNumber nvarchar(20) NOT NULL UNIQUE,
	CONSTRAINT PK_UserAuthId PRIMARY KEY CLUSTERED (UserAuthId)
);

CREATE TABLE LumeDB.dbo.LumeImageContent (
	ImageContentId int IDENTITY(1,1) NOT NULL UNIQUE,
	ContentHash nvarchar(200) NOT NULL UNIQUE,
	Content VARBINARY(MAX) NOT NULL,
	CONSTRAINT PK_ImageContentId PRIMARY KEY CLUSTERED (ImageContentId)
);

CREATE TABLE LumeDB.dbo.LumeUser (
	UserId int IDENTITY(1,1) NOT NULL UNIQUE,
	UserUid uniqueidentifier NOT NULL UNIQUE,
	Name nvarchar(200) NULL,
	Agenda nvarchar(800) NULL,
	Age tinyint NULL,
	ImageContentId int NULL,
	CONSTRAINT FK_User_ImageContent FOREIGN KEY (ImageContentId) REFERENCES LumeDB.dbo.LumeImageContent (ImageContentId),
	CONSTRAINT PK_UserId PRIMARY KEY CLUSTERED (UserId)
);

CREATE TABLE LumeDB.dbo.LumeEventType (
	EventTypeId int IDENTITY(1,1) NOT NULL UNIQUE,
	EventTypeName nvarchar(100) NOT NULL UNIQUE,
	CONSTRAINT PK_EventTypeId PRIMARY KEY CLUSTERED (EventTypeId)
);

CREATE TABLE LumeDB.dbo.LumeEventStatus (
	EventStatusId int IDENTITY(1,1) NOT NULL UNIQUE,
	EventStatusName nvarchar(100) NOT NULL UNIQUE,
	CONSTRAINT PK_EventStatusId PRIMARY KEY CLUSTERED (EventStatusId)
);

CREATE TABLE LumeDB.dbo.LumeEvent (
	EventId int IDENTITY(1,1) NOT NULL UNIQUE,
	Name nvarchar(200) NULL,
	MinAge tinyint NULL,
	MaxAge tinyint NULL,
	XCoordinate nvarchar(40) NULL,
	YCoordinate nvarchar(40) NULL,
	Agenda nvarchar(800) NULL,
	StartTime datetime2(7) NULL,
	EndTime datetime2(7) NULL,
	ImageContentId int NULL,
	EventTypeId int NULL,
	EventStatusId int NULL,
	CONSTRAINT FK_Event_ImageContent FOREIGN KEY (ImageContentId) REFERENCES LumeDB.dbo.LumeImageContent (ImageContentId),
	CONSTRAINT FK_Event_EventType FOREIGN KEY (EventTypeId) REFERENCES LumeDB.dbo.LumeEventType (EventTypeId),
	CONSTRAINT FK_Event_EventStatus FOREIGN KEY (EventStatusId) REFERENCES LumeDB.dbo.LumeEventStatus (EventStatusId),
	CONSTRAINT PK_EventId PRIMARY KEY CLUSTERED (EventId)
);

CREATE TABLE LumeDB.dbo.LumeChat (
	ChatId int IDENTITY(1,1) NOT NULL UNIQUE,
	Name nvarchar(200) NULL,
	CONSTRAINT PK_ChatId PRIMARY KEY CLUSTERED (ChatId)
);

CREATE TABLE LumeDB.dbo.LumeChatMessage (
	ChatMessageId int IDENTITY(1,1) NOT NULL UNIQUE,
	Content nvarchar(4000) NULL,
	ImageContentId int NULL,
	ChatId int NULL,
	CONSTRAINT FK_ChatMessage_Chat FOREIGN KEY (ChatId) REFERENCES LumeDB.dbo.LumeChat (ChatId),
	CONSTRAINT FK_ChatMessage_ImageContent FOREIGN KEY (ImageContentId) REFERENCES LumeDB.dbo.LumeImageContent (ImageContentId),
	CONSTRAINT PK_ChatMessageId PRIMARY KEY CLUSTERED (ChatMessageId)
);

CREATE TABLE LumeDB.dbo.LumeUserToEvent (
	UserId int,
  	EventId int,
  	CONSTRAINT PK_User_Event PRIMARY KEY (UserId, EventId),
  	CONSTRAINT FK_User_UserToEvent FOREIGN KEY (UserId) REFERENCES LumeDB.dbo.LumeUser (UserId),
  	CONSTRAINT FK_Event_UserToEvent FOREIGN KEY (EventId) REFERENCES LumeDB.dbo.LumeEvent (EventId)
);

CREATE TABLE LumeDB.dbo.LumeUserToChat (
	UserId int,
  	ChatId int,
  	CONSTRAINT PK_User_Chat PRIMARY KEY (UserId, ChatId),
  	CONSTRAINT FK_User_UserToChat FOREIGN KEY (UserId) REFERENCES LumeDB.dbo.LumeUser (UserId),
  	CONSTRAINT FK_Chat_UserToChat FOREIGN KEY (ChatId) REFERENCES LumeDB.dbo.LumeChat (ChatId)
);

CREATE TABLE LumeDB.dbo.LumeUserFriendList (
	UserId int,
  	FriendId int,
  	CONSTRAINT PK_User_Friend PRIMARY KEY (UserId, FriendId),
  	CONSTRAINT FK_User_UserFriendList FOREIGN KEY (UserId) REFERENCES LumeDB.dbo.LumeUser (UserId),
  	CONSTRAINT FK_Friend_UserFriendList FOREIGN KEY (FriendId) REFERENCES LumeDB.dbo.LumeUser (UserId)
);