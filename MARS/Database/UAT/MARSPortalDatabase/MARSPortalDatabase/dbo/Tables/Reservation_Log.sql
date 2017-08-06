CREATE TABLE [dbo].[Reservation_Log] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Message]     NVARCHAR (MAX) NULL,
    [DateTime]    DATETIME       NULL,
    [ErrorLog_Id] INT            NULL,
    CONSTRAINT [PK_ReservationLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reservation_Log_Reservation_Error_Log] FOREIGN KEY ([ErrorLog_Id]) REFERENCES [dbo].[Reservation_Error_Log] ([Id])
);

