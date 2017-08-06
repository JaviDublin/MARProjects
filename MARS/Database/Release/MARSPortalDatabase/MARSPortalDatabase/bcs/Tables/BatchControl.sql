CREATE TABLE [bcs].[BatchControl] (
    [BatchControl_id]       INT           IDENTITY (1, 1) NOT NULL,
    [ProcessBatch_id]       INT           NULL,
    [PrevProcessControlSeq] INT           NULL,
    [FileProcess_id]        INT           NULL,
    [status]                VARCHAR (100) NULL,
    [Data1]                 VARCHAR (200) NULL,
    [Data2]                 VARCHAR (200) NULL,
    [Data3]                 VARCHAR (200) NULL,
    [Data4]                 VARCHAR (200) NULL,
    [NextRunTime]           DATETIME      NULL,
    [RetryCount]            INT           NULL,
    CONSTRAINT [pk_BatchControl] PRIMARY KEY CLUSTERED ([BatchControl_id] ASC)
);

