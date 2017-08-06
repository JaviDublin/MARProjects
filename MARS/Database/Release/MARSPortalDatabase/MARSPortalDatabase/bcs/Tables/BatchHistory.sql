CREATE TABLE [bcs].[BatchHistory] (
    [BatchHistory_id] INT           IDENTITY (1, 1) NOT NULL,
    [BatchControl_id] INT           NOT NULL,
    [ProcessBatch_id] INT           NOT NULL,
    [Status]          VARCHAR (100) NULL,
    [DataXML]         XML           DEFAULT ('') NULL,
    [Data2]           VARCHAR (MAX) NULL,
    [Data3]           VARCHAR (MAX) NULL,
    CONSTRAINT [pk_BatchHistory] PRIMARY KEY CLUSTERED ([BatchHistory_id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_BatchHistory_01]
    ON [bcs].[BatchHistory]([Status] ASC);


GO
create trigger [bcs].tr_BatchHistory on [bcs].BatchHistory for insert, update
as
	if exists (select count(*) from [bcs].BatchHistory where Status = 'Running' group by ProcessBatch_id having count(*) > 1)
	begin
		rollback tran
		raiserror('Too many batches for ProcessBatch running', 16, -1)
	end