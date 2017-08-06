CREATE TABLE [dbo].[CMS_Fleet_Plans] (
    [planID]          TINYINT      NOT NULL,
    [planDescription] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_CMS_Fleet_Plans] PRIMARY KEY CLUSTERED ([planID] ASC)
);

