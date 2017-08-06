CREATE TABLE [Fao].[MaxFleetFactor] (
    [MaxFleetFactorId]      INT          IDENTITY (1, 1) NOT NULL,
    [LocationId]            INT          NOT NULL,
    [CarGroupId]            INT          NOT NULL,
    [DayOfWeekId]           TINYINT      NOT NULL,
    [NonRevPercentage]      FLOAT (53)   NULL,
    [UtilizationPercentage] FLOAT (53)   NULL,
    [UpdatedBy]             VARCHAR (50) NOT NULL,
    [UpdatedOn]             DATETIME     NOT NULL,
    CONSTRAINT [PK_MaxFleetFactor] PRIMARY KEY CLUSTERED ([MaxFleetFactorId] ASC),
    CONSTRAINT [FK_MaxFleetFactor_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_MaxFleetFactor_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_MaxFleetFactor_WeekDays] FOREIGN KEY ([DayOfWeekId]) REFERENCES [dbo].[WeekDays] ([DayOfWeekId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueIndex]
    ON [Fao].[MaxFleetFactor]([LocationId] ASC, [CarGroupId] ASC, [DayOfWeekId] ASC);

