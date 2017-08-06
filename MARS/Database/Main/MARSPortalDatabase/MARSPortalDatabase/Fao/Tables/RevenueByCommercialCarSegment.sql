CREATE TABLE [Fao].[RevenueByCommercialCarSegment] (
    [RevenueByCommercialCarSegmentId] INT        IDENTITY (1, 1) NOT NULL,
    [Year]                            SMALLINT   NOT NULL,
    [Month]                           TINYINT    NOT NULL,
    [LocationId]                      INT        NOT NULL,
    [CarGroupId]                      INT        NOT NULL,
    [CommercialCarSegmentId]          INT        NOT NULL,
    [RentalCount]                     INT        NOT NULL,
    [DaysDriven]                      FLOAT (53) NOT NULL,
    [DaysCharged]                     INT        NOT NULL,
    [FinanceDays]                     INT        NOT NULL,
    [GrossRevenue]                    FLOAT (53) NOT NULL,
    [PerformanceRevenue]              FLOAT (53) NOT NULL,
    [MonthDate]                       DATE       NOT NULL,
    CONSTRAINT [PK_CommercialSegmentGrossRev] PRIMARY KEY CLUSTERED ([RevenueByCommercialCarSegmentId] ASC),
    CONSTRAINT [FK_CommercialCarSegmentGrossRev_CAR_GROUPS] FOREIGN KEY ([CarGroupId]) REFERENCES [dbo].[CAR_GROUPS] ([car_group_id]),
    CONSTRAINT [FK_CommercialCarSegmentGrossRev_CommercialCarSegment] FOREIGN KEY ([CommercialCarSegmentId]) REFERENCES [Fao].[CommercialCarSegment] ([CommercialCarSegmentId]),
    CONSTRAINT [FK_CommercialCarSegmentGrossRev_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id])
);




GO
CREATE NONCLUSTERED INDEX [CommericalCarSegmentIndex]
    ON [Fao].[RevenueByCommercialCarSegment]([CommercialCarSegmentId] ASC);


GO
CREATE NONCLUSTERED INDEX [CarGroupIndex]
    ON [Fao].[RevenueByCommercialCarSegment]([CarGroupId] ASC);


GO
CREATE NONCLUSTERED INDEX [LocationIndex]
    ON [Fao].[RevenueByCommercialCarSegment]([LocationId] ASC);

