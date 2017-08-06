CREATE TABLE [dbo].[UserTraining] (
    [UserTrainingId] INT           IDENTITY (1, 1) NOT NULL,
    [Url]            NCHAR (200)   NOT NULL,
    [Description]    VARCHAR (200) NOT NULL,
    [IsActive]       BIT           NOT NULL,
    CONSTRAINT [PK_UserTraining] PRIMARY KEY CLUSTERED ([UserTrainingId] ASC)
);

