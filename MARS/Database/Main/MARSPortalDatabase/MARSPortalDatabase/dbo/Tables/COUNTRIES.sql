CREATE TABLE [dbo].[COUNTRIES] (
    [country]             VARCHAR (10) NOT NULL,
    [country_dw]          VARCHAR (50) NOT NULL,
    [country_description] VARCHAR (50) NOT NULL,
    [active]              BIT          NOT NULL,
    [CountryId]           INT          IDENTITY (1, 1) NOT NULL,
    [GmtDifference]       TINYINT      CONSTRAINT [DF_COUNTRIES_GmtDifference] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_COUNTRY_AREACODES] PRIMARY KEY CLUSTERED ([country] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UniqueCountryId]
    ON [dbo].[COUNTRIES]([CountryId] ASC);

