CREATE TABLE [dbo].[Log] (
    [Log_Id]      INT           IDENTITY (1, 1) NOT NULL,
    [LogDateTime] DATETIME      NULL,
    [LogType]     NVARCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([Log_Id] ASC)
);

