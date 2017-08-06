CREATE TABLE [Export].[Error_Log] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Message]  NVARCHAR (MAX) NULL,
    [DateTime] DATETIME       NULL,
    CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

