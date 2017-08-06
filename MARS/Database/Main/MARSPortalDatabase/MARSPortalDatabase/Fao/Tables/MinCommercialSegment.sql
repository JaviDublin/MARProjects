CREATE TABLE [Fao].[MinCommercialSegment] (
    [MinCommercialSegmentId]         INT          IDENTITY (1, 1) NOT NULL,
    [MinCommercialSegmentScenarioId] INT          NOT NULL,
    [LocationId]                     INT          NOT NULL,
    [CarSegmentId]                   INT          NOT NULL,
    [CommercialCarSegmentId]         INT          NOT NULL,
    [Percentage]                     FLOAT (53)   NOT NULL,
    [UpdatedBy]                      VARCHAR (50) NOT NULL,
    [UpdatedOn]                      DATETIME     NOT NULL,
    CONSTRAINT [PK_MinCommercialSegment] PRIMARY KEY CLUSTERED ([MinCommercialSegmentId] ASC),
    CONSTRAINT [FK_MinCommercialSegment_CAR_SEGMENTS] FOREIGN KEY ([CarSegmentId]) REFERENCES [dbo].[CAR_SEGMENTS] ([car_segment_id]),
    CONSTRAINT [FK_MinCommercialSegment_CommercialCarSegment] FOREIGN KEY ([CommercialCarSegmentId]) REFERENCES [Fao].[CommercialCarSegment] ([CommercialCarSegmentId]),
    CONSTRAINT [FK_MinCommercialSegment_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_MinCommercialSegment_MinCommercialSegmentScenario] FOREIGN KEY ([MinCommercialSegmentScenarioId]) REFERENCES [Fao].[MinCommercialSegmentScenario] ([MinCommercialSegmentScenarioId])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueIndex]
    ON [Fao].[MinCommercialSegment]([MinCommercialSegmentScenarioId] ASC, [LocationId] ASC, [CarSegmentId] ASC, [CommercialCarSegmentId] ASC);



