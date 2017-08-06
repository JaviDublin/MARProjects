CREATE TABLE [dbo].[FleetOwner] (
    [FleetOwnerId]   INT          IDENTITY (1, 1) NOT NULL,
    [OwningAreaCode] VARCHAR (5)  NOT NULL,
    [OwnerName]      VARCHAR (50) NULL,
    [CountryId]      INT          NOT NULL,
    [CompanyId]      INT          NULL,
    CONSTRAINT [PK_FleetOwner] PRIMARY KEY CLUSTERED ([FleetOwnerId] ASC),
    CONSTRAINT [FK_FleetOwner_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]),
    CONSTRAINT [FK_FleetOwner_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[COUNTRIES] ([CountryId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UniqueAreaCode]
    ON [dbo].[FleetOwner]([OwningAreaCode] ASC);

