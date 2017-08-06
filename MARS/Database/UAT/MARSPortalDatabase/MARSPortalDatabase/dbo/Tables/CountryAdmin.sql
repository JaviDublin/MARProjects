CREATE TABLE [dbo].[CountryAdmin] (
    [CountryAdminId] INT           IDENTITY (1, 1) NOT NULL,
    [CountryId]      INT           NOT NULL,
    [CompanyTypeId]  INT           NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [EmailAddress]   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CountryAdmin] PRIMARY KEY CLUSTERED ([CountryAdminId] ASC),
    CONSTRAINT [FK_CountryAdmin_CompanyType] FOREIGN KEY ([CompanyTypeId]) REFERENCES [dbo].[CompanyType] ([CompanyTypeId]),
    CONSTRAINT [FK_CountryAdmin_COUNTRIES] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[COUNTRIES] ([CountryId])
);

