CREATE TABLE [dbo].[POOLING_RESERVATION_TYPE] (
    [reservationTypeId] INT          NOT NULL,
    [reservationType]   VARCHAR (50) NULL,
    CONSTRAINT [PK_POOLING_RESERVATIONS_TYPE] PRIMARY KEY CLUSTERED ([reservationTypeId] ASC)
);

