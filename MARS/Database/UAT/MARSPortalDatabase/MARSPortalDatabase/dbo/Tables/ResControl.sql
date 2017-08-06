CREATE TABLE [dbo].[ResControl] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ConditionId] INT           NOT NULL,
    [Comment]     VARCHAR (MAX) NULL,
    [ConDateTime] DATETIME      NOT NULL,
    [ErrorLogId]  INT           NULL,
    [TimeStamp]   DATETIME      NOT NULL,
    CONSTRAINT [PK_ResControl] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResControl_ResContCondition] FOREIGN KEY ([ConditionId]) REFERENCES [dbo].[ResContCondition] ([Id]),
    CONSTRAINT [FK_ResControl_Reservation_Error_Log] FOREIGN KEY ([ErrorLogId]) REFERENCES [dbo].[Reservation_Error_Log] ([Id])
);











