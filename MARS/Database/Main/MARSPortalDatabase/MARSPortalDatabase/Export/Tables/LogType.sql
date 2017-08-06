CREATE TABLE [Export].[LogType] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (20) NULL,
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

