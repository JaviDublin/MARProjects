CREATE TABLE [dbo].[ResFieldToTeradata] (
    [ResStageId]    INT NOT NULL,
    [ResTeradataId] INT NOT NULL,
    CONSTRAINT [PK_ResStageToTeradata] PRIMARY KEY CLUSTERED ([ResStageId] ASC, [ResTeradataId] ASC),
    CONSTRAINT [FK_ResFieldToTeradata_ResFields] FOREIGN KEY ([ResStageId]) REFERENCES [dbo].[ResFields] ([Id]),
    CONSTRAINT [FK_ResFieldToTeradata_ResTeradataFields] FOREIGN KEY ([ResTeradataId]) REFERENCES [dbo].[ResTeradataFields] ([Id])
);










