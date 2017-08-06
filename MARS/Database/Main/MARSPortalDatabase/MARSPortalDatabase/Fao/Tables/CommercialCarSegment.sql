CREATE TABLE [Fao].[CommercialCarSegment] (
    [CommercialCarSegmentId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]                   VARCHAR (50) NOT NULL,
    [Code]                   VARCHAR (50) NULL,
    CONSTRAINT [PK_CommercialCarSegment] PRIMARY KEY CLUSTERED ([CommercialCarSegmentId] ASC)
);

