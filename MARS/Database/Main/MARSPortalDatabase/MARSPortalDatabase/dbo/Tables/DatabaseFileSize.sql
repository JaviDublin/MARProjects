CREATE TABLE [dbo].[DatabaseFileSize] (
    [rowId]             UNIQUEIDENTIFIER NULL,
    [database_id]       INT              NULL,
    [file_id]           INT              NULL,
    [file_type_desc]    NVARCHAR (120)   NULL,
    [name]              NVARCHAR (128)   NULL,
    [physical_name]     NVARCHAR (520)   NULL,
    [state_desc]        NVARCHAR (120)   NULL,
    [size]              INT              NULL,
    [max_size]          INT              NULL,
    [growth]            INT              NULL,
    [is_sparse]         BIT              NULL,
    [is_percent_growth] BIT              NULL,
    [collect_date]      DATETIME         NULL
);

