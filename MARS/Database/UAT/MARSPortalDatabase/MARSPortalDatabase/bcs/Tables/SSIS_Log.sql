CREATE TABLE [bcs].[SSIS_Log] (
    [SSIS_Log_id]            INT           IDENTITY (1, 1) NOT NULL,
    [BatchProcessHistory_id] INT           NULL,
    [seq]                    INT           NULL,
    [data]                   VARCHAR (MAX) NULL,
    [z_inserted]             DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([SSIS_Log_id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_SSIS_Log_01]
    ON [bcs].[SSIS_Log]([BatchProcessHistory_id] ASC, [seq] ASC);

