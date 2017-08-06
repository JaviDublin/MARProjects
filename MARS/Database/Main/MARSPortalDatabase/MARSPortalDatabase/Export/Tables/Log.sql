CREATE TABLE [Export].[Log] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Message]     NVARCHAR (MAX) NULL,
    [DateTime]    DATETIME       NULL,
    [ErrorLog_Id] INT            NULL,
    [Status_Id]   INT            NULL,
    [LogType_Id]  INT            CONSTRAINT [DF_Log_LogType_Id] DEFAULT ((1)) NOT NULL,
    [Timestamp]   ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Log_Error_Log] FOREIGN KEY ([ErrorLog_Id]) REFERENCES [Export].[Error_Log] ([Id]),
    CONSTRAINT [FK_Log_LogType] FOREIGN KEY ([LogType_Id]) REFERENCES [Export].[LogType] ([Id]),
    CONSTRAINT [FK_Log_Status] FOREIGN KEY ([Status_Id]) REFERENCES [Export].[Status] ([Id])
);

