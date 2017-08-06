CREATE TABLE [dbo].[CompanyType] (
    [CompanyTypeId]   INT          IDENTITY (1, 1) NOT NULL,
    [CompanyTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CompanyType] PRIMARY KEY CLUSTERED ([CompanyTypeId] ASC)
);

