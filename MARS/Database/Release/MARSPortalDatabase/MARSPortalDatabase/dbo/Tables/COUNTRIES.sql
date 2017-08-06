CREATE TABLE [dbo].[COUNTRIES] (
    [country]             VARCHAR (10) NOT NULL,
    [country_dw]          VARCHAR (50) NOT NULL,
    [country_description] VARCHAR (50) NOT NULL,
    [active]              BIT          NOT NULL,
    CONSTRAINT [PK_COUNTRY_AREACODES] PRIMARY KEY CLUSTERED ([country] ASC)
);

