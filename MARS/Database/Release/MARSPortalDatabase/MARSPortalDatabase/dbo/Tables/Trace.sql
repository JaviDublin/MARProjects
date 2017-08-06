CREATE TABLE [dbo].[Trace] (
    [Trace_id]  INT           IDENTITY (1, 1) NOT NULL,
    [EventDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    [Entity]    VARCHAR (100) NOT NULL,
    [key1]      VARCHAR (100) NULL,
    [key2]      VARCHAR (100) NULL,
    [key3]      VARCHAR (100) NULL,
    [data1]     VARCHAR (MAX) NULL,
    [data2]     VARCHAR (MAX) NULL,
    [data3]     VARCHAR (MAX) NULL,
    [UserName]  VARCHAR (128) DEFAULT (suser_sname()) NULL,
    CONSTRAINT [pk_Trace] PRIMARY KEY CLUSTERED ([Trace_id] ASC)
);

