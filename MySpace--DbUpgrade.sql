CREATE TABLE Artists (
	Id int PRIMARY KEY IDENTITY(1,1),
	UserId int NOT NULL,
	Name nvarchar(50) NOT NULL,
	MainPhotoGUID nvarchar(100),
	Description nvarchar(50),
	Approved bit NOT NULL DEFAULT 0,
	Visits int,
	Likes int,

	CONSTRAINT fk_artists_users FOREIGN KEY (userId) REFERENCES Users([Id]),
);

CREATE TABLE Videos (
	Id int primary key identity(1,1),
	ArtistId int not null,
	YoutubeLink nvarchar(100) not null,
	Creation datetime not null default GETDATE(),

	CONSTRAINT uk_videos_youtubeLink UNIQUE(youtubeLink),
	CONSTRAINT fk_videos_artists FOREIGN KEY (artistId) REFERENCES Artists(Id),
);

CREATE TABLE Messages (
	Id int primary key identity(1,1),
	ArtistId int not null,
	UserId int not null,
	Text nvarchar(50),
	Creation datetime not null default GETDATE(),

	CONSTRAINT fk_messages_users FOREIGN KEY (UserId) REFERENCES Users(Id),
	CONSTRAINT fk_messages_artists FOREIGN KEY (ArtistId) REFERENCES Artists(Id),
);

CREATE TABLE FanLikes (
	Id int primary key identity(1,1),
	ArtistId int not null,
	UserId int not null,
	Creation datetime not null default GETDATE(),

	CONSTRAINT fk_fanlikes_users FOREIGN KEY (UserId) REFERENCES Users(Id),
	CONSTRAINT fk_fanlikes_artists FOREIGN KEY (ArtistId) REFERENCES Artists(Id),
);
