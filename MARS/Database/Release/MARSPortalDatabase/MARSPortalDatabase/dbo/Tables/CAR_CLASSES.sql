CREATE TABLE [dbo].[CAR_CLASSES] (
    [car_class_id]   INT          IDENTITY (1, 1) NOT NULL,
    [car_class]      VARCHAR (50) NOT NULL,
    [car_segment_id] INT          NOT NULL,
    [sort_car_class] INT          NOT NULL,
    CONSTRAINT [PK_CAR_CLASSES] PRIMARY KEY CLUSTERED ([car_class_id] ASC),
    CONSTRAINT [FK_CAR_CLASSES_CAR_SEGMENTS] FOREIGN KEY ([car_segment_id]) REFERENCES [dbo].[CAR_SEGMENTS] ([car_segment_id])
);

