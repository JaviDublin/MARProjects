CREATE TABLE [dbo].[ResFields] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    [TypeId] INT           NOT NULL,
    [TblId]  INT           NULL,
    CONSTRAINT [PK_ResStageFields] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResFields_ResFieldsType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[ResFieldsType] ([Id]),
    CONSTRAINT [FK_ResFields_ResTableName] FOREIGN KEY ([TblId]) REFERENCES [dbo].[ResTableName] ([Id]) ON DELETE SET NULL ON UPDATE CASCADE
);



