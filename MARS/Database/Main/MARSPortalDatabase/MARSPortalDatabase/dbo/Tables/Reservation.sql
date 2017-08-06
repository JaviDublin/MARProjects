CREATE TABLE [dbo].[Reservation] (
    [ReservationId]      INT           IDENTITY (1, 1) NOT NULL,
    [ExternalId]         VARCHAR (20)  NOT NULL,
    [Country]            VARCHAR (10)  NOT NULL,
    [PickupLocationId]   INT           NOT NULL,
    [ReturnLocationId]   INT           NOT NULL,
    [PickupDate]         DATETIME      NOT NULL,
    [ReturnDate]         DATETIME      NOT NULL,
    [BookedDate]         DATETIME      NOT NULL,
    [UpgradedCarGroupId] INT           NOT NULL,
    [ReservedCarGroupId] INT           NOT NULL,
    [Tariff]             VARCHAR (20)  NOT NULL,
    [N1Type]             VARCHAR (20)  NULL,
    [NeverLost]          BIT           NULL,
    [CustomerName]       VARCHAR (50)  NULL,
    [FlightNumber]       VARCHAR (20)  NULL,
    [Remark]             VARCHAR (200) NOT NULL,
    [DateAdded]          DATETIME      NULL,
    [Comment]            VARCHAR (200) NULL,
    CONSTRAINT [PK_Reservation] PRIMARY KEY CLUSTERED ([ReservationId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Reservation_RentalLocation2] FOREIGN KEY ([PickupLocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_Reservation_ReturnLocation2] FOREIGN KEY ([ReturnLocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_Reservation_UnUpgradedCarGroup] FOREIGN KEY ([ReservedCarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_Reservation_UpgradedCarGroup] FOREIGN KEY ([UpgradedCarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id])
);


GO
CREATE NONCLUSTERED INDEX [UpgradedCarGroup]
    ON [dbo].[Reservation]([UpgradedCarGroupId] ASC);


GO
CREATE NONCLUSTERED INDEX [ReservedCarGroup]
    ON [dbo].[Reservation]([ReservedCarGroupId] ASC);


GO
CREATE NONCLUSTERED INDEX [ReturnLocation]
    ON [dbo].[Reservation]([ReturnLocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [PickupLocation]
    ON [dbo].[Reservation]([PickupLocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [PickupDate]
    ON [dbo].[Reservation]([PickupDate] ASC);


GO
CREATE NONCLUSTERED INDEX [CountryIndex]
    ON [dbo].[Reservation]([Country] ASC);

