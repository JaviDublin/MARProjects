CREATE TABLE [dbo].[Reservation_Error_Log] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Message]        NVARCHAR (MAX) NULL,
    [DateTime]       DATETIME       NULL,
    [ResErrorTypeId] INT            NULL,
    CONSTRAINT [PK_ReservationErrorLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reservation_Error_Log_ResErrorType] FOREIGN KEY ([ResErrorTypeId]) REFERENCES [dbo].[ResErrorType] ([Id])
);



