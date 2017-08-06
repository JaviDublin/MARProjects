CREATE TABLE [Fao].[MaxFleetSize] (
    [MaxFleetSizeId]              INT        IDENTITY (1, 1) NOT NULL,
    [LocationId]                  INT        NOT NULL,
    [CarGroupId]                  INT        NOT NULL,
    [WeekNumber]                  TINYINT    NOT NULL,
    [PeakDay]                     DATE       NOT NULL,
    [MaxForecastForLocationGroup] FLOAT (53) NOT NULL,
    [MaxForecastForLocation]      FLOAT (53) NOT NULL,
    [MaxFleet]                    FLOAT (53) NOT NULL,
    CONSTRAINT [PK_MaxFleetSize] PRIMARY KEY CLUSTERED ([MaxFleetSizeId] ASC),
    CONSTRAINT [FK_MaxFleetSize_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_MaxFleetSize_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueIndex]
    ON [Fao].[MaxFleetSize]([LocationId] ASC, [CarGroupId] ASC, [WeekNumber] ASC);

