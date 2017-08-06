CREATE TABLE [dbo].[ResTeradataFields] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (200) NOT NULL,
    [TblId] INT            NULL,
    CONSTRAINT [PK_ResTeradataFields] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResTeradataFields_ResTableName] FOREIGN KEY ([TblId]) REFERENCES [dbo].[ResTableName] ([Id]) ON DELETE SET NULL ON UPDATE CASCADE
);

