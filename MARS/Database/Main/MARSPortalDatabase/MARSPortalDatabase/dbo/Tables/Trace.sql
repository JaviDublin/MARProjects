CREATE TABLE [dbo].[Trace] (
    [Trace_id]  INT           NOT NULL,
    [EventDate] DATETIME      NOT NULL,
    [Entity]    VARCHAR (100) NOT NULL,
    [key1]      VARCHAR (100) NULL,
    [key2]      VARCHAR (100) NULL,
    [key3]      VARCHAR (100) NULL,
    [data1]     VARCHAR (MAX) NULL,
    [data2]     VARCHAR (MAX) NULL,
    [data3]     VARCHAR (MAX) NULL,
    [UserName]  VARCHAR (128) NULL
);



