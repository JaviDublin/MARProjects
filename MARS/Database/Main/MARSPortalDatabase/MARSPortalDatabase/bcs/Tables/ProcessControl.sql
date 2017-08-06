CREATE TABLE [bcs].[ProcessControl] (
    [ProcessBatch_id]   INT            NOT NULL,
    [ProcessControlSeq] INT            NOT NULL,
    [Processname]       VARCHAR (50)   NOT NULL,
    [ProcessType]       VARCHAR (20)   NOT NULL,
    [Data1]             VARCHAR (1000) NULL,
    [Data2]             VARCHAR (1000) NULL,
    [Data3]             VARCHAR (1000) NULL,
    [ExecuteUser]       VARCHAR (200)  NULL,
    [RetryWaitTime]     DATETIME       NULL,
    [RetryCount]        INT            NULL,
    CONSTRAINT [pk_ProcessControl] PRIMARY KEY CLUSTERED ([ProcessBatch_id] ASC, [ProcessControlSeq] ASC)
);

