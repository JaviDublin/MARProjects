CREATE TABLE [dbo].[CAR_SEGMENTS] (
    [car_segment_id]   INT          IDENTITY (1, 1) NOT NULL,
    [car_segment]      VARCHAR (50) NOT NULL,
    [country]          VARCHAR (10) NOT NULL,
    [sort_car_segment] INT          NOT NULL,
    [IsActive]         BIT          NULL,
    CONSTRAINT [PK_SEGMENTS] PRIMARY KEY CLUSTERED ([car_segment_id] ASC),
    CONSTRAINT [FK_CAR_SEGMENTS_COUNTRIES] FOREIGN KEY ([country]) REFERENCES [dbo].[COUNTRIES] ([country])
);



