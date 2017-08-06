CREATE TABLE [dbo].[Mars_Logging_CategoryLog] (
    [CategoryLogID] INT IDENTITY (1, 1) NOT NULL,
    [CategoryID]    INT NOT NULL,
    [LogID]         INT NOT NULL,
    CONSTRAINT [PK_CategoryLog] PRIMARY KEY CLUSTERED ([CategoryLogID] ASC)
);

