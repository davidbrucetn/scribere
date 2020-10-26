USE [master]

IF db_id('Scribere') IS NULl
  CREATE DATABASE [Scribere]
GO

USE [Scribere]
GO

CREATE TABLE [UserData] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserLevelId] int NOT NULL,
  [FirebaseUserId] varchar(28),
  [NameFirst] varchar(40),
  [NameLast] varchar(40),
  [Pseudonym] varchar(100),
  [Email] varchar(350) NOT NULL,
  [City] varchar(100),
  [State] varchar(100),
  [CountryId] int,
  [Created_at] datetime,
  [AllowMessaging] int NOT NULL DEFAULT (0),
  [IsActive] int NOT NULL DEFAULT (1)
)
GO

CREATE TABLE [UserLevel] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Level] varchar(25)
)
GO

CREATE TABLE [Country] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [UserImage] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [ImageUrl] varchar(300)
)
GO

CREATE TABLE [Article] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [CategoryId] int,
  [Heading] varchar(100),
  [Text] nvarchar(1000),
  [CreateDate] datetime NOT NULL,
  [VisibilityId] int NOT NULL
)
GO

CREATE TABLE [Category] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Type] varchar(75)
)
GO

CREATE TABLE [Book] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ArticleId] int NOT NULL,
  [ISBN] varchar(13),
  [LCCN] varchar(12),
  [Title] varchar(150),
  [Author] varchar(100)
)
GO

CREATE TABLE [ArticleImage] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ArticleId] int NOT NULL,
  [ImageUrl] varchar(300)
)
GO

CREATE TABLE [Visibility] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Type] varchar(50) NOT NULL
)
GO

CREATE TABLE [Comment] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ArticleId] int NOT NULL,
  [UserId] int NOT NULL,
  [Text] Text,
  [CreateDate] datetime
)
GO

CREATE TABLE [UserBlock] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [SourceUserId] int NOT NULL,
  [BlockedUserId] int NOT NULL
)
GO

CREATE TABLE [Tag] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Title] varchar(100)
)
GO

CREATE TABLE [ArticleTag] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ArticleId] int NOT NULL,
  [TagId] int NOT NULL
)
GO

CREATE TABLE [FavoriteArticle] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [ArticleId] int NOT NULL
)
GO

CREATE TABLE [FavoriteAuthor] (
  [Int] int PRIMARY KEY IDENTITY(1, 1),
  [SourceUserId] int NOT NULL,
  [FavoriteUserId] int NOT NULL
)
GO

CREATE TABLE [Message] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [SenderId] int,
  [Subject] varchar(50),
  [Content] text,
  [IsDeleted] int NOT NULL DEFAULT (0)
)
GO

CREATE TABLE [MsgReceiver] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [MessageId] int NOT NULL,
  [UserId] int NOT NULL,
  [Read] int NOT NULL DEFAULT (0),
  [IsDeleted] int NOT NULL DEFAULT (0)
)
GO

CREATE TABLE [Circle] (
  [Id] int PRIMARY KEY NOT NULL,
  [UserId] int,
  [FriendId] int
)
GO

ALTER TABLE [UserData] ADD FOREIGN KEY ([UserLevelId]) REFERENCES [UserLevel] ([Id])
GO

ALTER TABLE [UserData] ADD FOREIGN KEY ([CountryId]) REFERENCES [Country] ([Id])
GO

ALTER TABLE [UserImage] ADD FOREIGN KEY ([UserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [Article] ADD FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])
GO

ALTER TABLE [Article] ADD FOREIGN KEY ([UserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [Book] ADD FOREIGN KEY ([ArticleId]) REFERENCES [Article] ([Id])
GO

ALTER TABLE [Article] ADD FOREIGN KEY ([VisibilityId]) REFERENCES [Visibility] ([Id])
GO

ALTER TABLE [ArticleImage] ADD FOREIGN KEY ([ArticleId]) REFERENCES [Article] ([Id])
GO

ALTER TABLE [Comment] ADD FOREIGN KEY ([ArticleId]) REFERENCES [Article] ([Id])
GO

ALTER TABLE [Comment] ADD FOREIGN KEY ([UserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [UserBlock] ADD FOREIGN KEY ([SourceUserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [UserBlock] ADD FOREIGN KEY ([BlockedUserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [ArticleTag] ADD FOREIGN KEY ([ArticleId]) REFERENCES [Article] ([Id])
GO

ALTER TABLE [ArticleTag] ADD FOREIGN KEY ([TagId]) REFERENCES [Tag] ([Id])
GO

ALTER TABLE [FavoriteArticle] ADD FOREIGN KEY ([ArticleId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [FavoriteArticle] ADD FOREIGN KEY ([ArticleId]) REFERENCES [Article] ([Id])
GO

ALTER TABLE [FavoriteAuthor] ADD FOREIGN KEY ([SourceUserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [FavoriteAuthor] ADD FOREIGN KEY ([FavoriteUserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [Message] ADD FOREIGN KEY ([SenderId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [MsgReceiver] ADD FOREIGN KEY ([MessageId]) REFERENCES [Message] ([Id])
GO

ALTER TABLE [MsgReceiver] ADD FOREIGN KEY ([UserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [Circle] ADD FOREIGN KEY ([UserId]) REFERENCES [UserData] ([Id])
GO

ALTER TABLE [Circle] ADD FOREIGN KEY ([FriendId]) REFERENCES [UserData] ([Id])
GO
