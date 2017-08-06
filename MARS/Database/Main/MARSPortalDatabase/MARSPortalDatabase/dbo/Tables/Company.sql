CREATE TABLE [dbo].[Company] (
    [CompanyId]     INT          IDENTITY (1, 1) NOT NULL,
    [CompanyName]   VARCHAR (50) NOT NULL,
    [CompanyTypeId] INT          NOT NULL,
    [CountryId]     INT          NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([CompanyId] ASC),
    CONSTRAINT [FK_Company_CompanyType] FOREIGN KEY ([CompanyTypeId]) REFERENCES [dbo].[CompanyType] ([CompanyTypeId]),
    CONSTRAINT [FK_Company_COUNTRIES] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[COUNTRIES] ([CountryId])
);

