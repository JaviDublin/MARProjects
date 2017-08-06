CREATE TABLE [dbo].[COMPANIES] (
    [company]      VARCHAR (10) NOT NULL,
    [country]      VARCHAR (10) NULL,
    [company_name] VARCHAR (50) NULL,
    [opco]         BIT          NULL,
    [fleetco]      BIT          NULL,
    PRIMARY KEY CLUSTERED ([company] ASC),
    CONSTRAINT [FK_COMPANIES_COUNTRIES] FOREIGN KEY ([country]) REFERENCES [dbo].[COUNTRIES] ([country])
);

