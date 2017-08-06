CREATE TABLE [dbo].[MARS_CountryRoles] (
    [CountryRoleId] INT          IDENTITY (1, 1) NOT NULL,
    [RoleId]        INT          NULL,
    [CountryId]     VARCHAR (10) NULL,
    CONSTRAINT [PK_MARS_CountryRoles] PRIMARY KEY CLUSTERED ([CountryRoleId] ASC)
);

