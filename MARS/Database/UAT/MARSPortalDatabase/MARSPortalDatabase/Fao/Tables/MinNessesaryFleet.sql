CREATE TABLE [Fao].[MinNessesaryFleet] (
    [MinNessesaryFleetId] INT        IDENTITY (1, 1) NOT NULL,
    [LocationId]          INT        NOT NULL,
    [CarGroupId]          INT        NOT NULL,
    [AverageOnRent]       INT        NOT NULL,
    [MinFleet]            FLOAT (53) NOT NULL,
    CONSTRAINT [PK_MinNessesaryFleet] PRIMARY KEY CLUSTERED ([MinNessesaryFleetId] ASC),
    CONSTRAINT [FK_MinNessesaryFleet_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_MinNessesaryFleet_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);

