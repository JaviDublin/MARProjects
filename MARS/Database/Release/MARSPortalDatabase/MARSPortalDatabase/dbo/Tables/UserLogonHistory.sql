CREATE TABLE [dbo].[UserLogonHistory] (
    [UserLogonHistoryId] INT           IDENTITY (1, 1) NOT NULL,
    [UserName]           VARCHAR (100) NULL,
    [UserId]             VARCHAR (100) NULL,
    [TimeStamp]          DATETIME      NULL,
    CONSTRAINT [PK_UserLogonHistory] PRIMARY KEY CLUSTERED ([UserLogonHistoryId] ASC)
);

