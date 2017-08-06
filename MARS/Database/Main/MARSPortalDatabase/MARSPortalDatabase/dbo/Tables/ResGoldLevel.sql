CREATE TABLE [dbo].[ResGoldLevel] (
    [ResGoldLevelId] INT           IDENTITY (1, 1) NOT NULL,
    [GoldLevelName]  VARCHAR (200) NULL,
    CONSTRAINT [PK_ResGoldLevel] PRIMARY KEY CLUSTERED ([ResGoldLevelId] ASC)
);

