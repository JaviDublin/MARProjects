CREATE TABLE [dbo].[DATA_IMPORT_DETAILS] (
    [importTypeId]      INT           NULL,
    [importTimeStamp]   DATETIME      NULL,
    [importTypeIsDaily] BIT           NULL,
    [recordsImported]   INT           NULL,
    [comment]           VARCHAR (300) NULL
);

