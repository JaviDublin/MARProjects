CREATE TABLE [dbo].[SqlLog] (
    [SqlLogID]       INT           NOT NULL,
    [LogTime]        DATETIME      NOT NULL,
    [ErrorCode]      INT           NOT NULL,
    [ErrorMessage]   VARCHAR (MAX) NOT NULL,
    [ErrorSeverity]  INT           NOT NULL,
    [ErrorProcedure] VARCHAR (50)  NOT NULL,
    [ErrorLine]      INT           NOT NULL,
    [ErrorState]     INT           NOT NULL
);

