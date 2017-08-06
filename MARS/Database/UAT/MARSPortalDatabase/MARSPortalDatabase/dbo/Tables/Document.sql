CREATE TABLE [dbo].[Document] (
    [DocumentId]   INT           IDENTITY (1, 1) NOT NULL,
    [Url]          VARCHAR (500) NOT NULL,
    [Active]       BIT           CONSTRAINT [DF_Document_Active] DEFAULT ((0)) NOT NULL,
    [DocumentName] VARCHAR (100) CONSTRAINT [DF_Document_DocumentName] DEFAULT ('') NOT NULL,
    [OrderNumber]  INT           CONSTRAINT [DF_Document_OrderNumber] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED ([DocumentId] ASC)
);

