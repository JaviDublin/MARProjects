CREATE TABLE [Fao].[PredictionModelMinCommercialSegment] (
    [PredictionModelMinCommercialSegmentId] INT        IDENTITY (1, 1) NOT NULL,
    [PredictionModelId]                     INT        NOT NULL,
    [LocationId]                            INT        NOT NULL,
    [CarSegmentId]                          INT        NOT NULL,
    [CommercialCarSegmentId]                INT        NOT NULL,
    [Percentage]                            FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PredictionModelMinCommercialSegment] PRIMARY KEY CLUSTERED ([PredictionModelMinCommercialSegmentId] ASC),
    CONSTRAINT [FK_PredictionModelMinCommercialSegment_CAR_SEGMENTS] FOREIGN KEY ([CarSegmentId]) REFERENCES [dbo].[CAR_SEGMENTS] ([car_segment_id]),
    CONSTRAINT [FK_PredictionModelMinCommercialSegment_CommercialCarSegment] FOREIGN KEY ([CommercialCarSegmentId]) REFERENCES [Fao].[CommercialCarSegment] ([CommercialCarSegmentId]),
    CONSTRAINT [FK_PredictionModelMinCommercialSegment_LOCATIONS] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[LOCATIONS] ([dim_Location_id]),
    CONSTRAINT [FK_PredictionModelMinCommercialSegment_PredictionModel] FOREIGN KEY ([PredictionModelId]) REFERENCES [Fao].[PredictionModel] ([PredictionModelId])
);

