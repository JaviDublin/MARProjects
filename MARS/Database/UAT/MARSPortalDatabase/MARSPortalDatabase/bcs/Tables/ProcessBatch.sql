CREATE TABLE [bcs].[ProcessBatch] (
    [ProcessBatch_id]  INT           NOT NULL,
    [ProcessBatchName] VARCHAR (100) NOT NULL,
    CONSTRAINT [pk_ProcessBatch] PRIMARY KEY CLUSTERED ([ProcessBatch_id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_ProcessBatch_01]
    ON [bcs].[ProcessBatch]([ProcessBatchName] ASC);

