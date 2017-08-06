CREATE TABLE [dbo].[ResTableName] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [DbNameId] INT          NULL,
    CONSTRAINT [PK_ResTableName] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResTableName_ResDbName] FOREIGN KEY ([DbNameId]) REFERENCES [dbo].[ResDbName] ([Id]) ON DELETE CASCADE
);

