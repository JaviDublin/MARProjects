CREATE TABLE [bcs].[BatchProcessHistory] (
    [BatchProcessHistory_id] INT           IDENTITY (1, 1) NOT NULL,
    [ProcessBatch_id]        INT           NOT NULL,
    [BatchControl_id]        INT           NOT NULL,
    [ProcessControlSeq]      INT           NOT NULL,
    [Processname]            VARCHAR (50)  NULL,
    [StartDate]              DATETIME      NOT NULL,
    [EndDate]                DATETIME      NOT NULL,
    [Status]                 VARCHAR (100) NULL,
    [Data1]                  VARCHAR (MAX) NULL,
    [Data2]                  VARCHAR (MAX) NULL,
    [Data3]                  VARCHAR (MAX) NULL,
    CONSTRAINT [pk_BatchProcessHistory] PRIMARY KEY CLUSTERED ([BatchProcessHistory_id] ASC)
);

