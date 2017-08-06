CREATE TABLE [dbo].[MarsUserRole] (
    [MarsUserRoleId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (20) NOT NULL,
    [Description]    VARCHAR (50) NULL,
    [BaseAccess]     BIT          CONSTRAINT [DF_MarsUserRole_BaseAccess] DEFAULT ((0)) NOT NULL,
    [AdminAccess]    BIT          CONSTRAINT [DF_MarsUserRole_AdminAccess] DEFAULT ((0)) NOT NULL,
    [CompanyTypeId]  INT          NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([MarsUserRoleId] ASC),
    CONSTRAINT [FK_MarsUserRole_CompanyType] FOREIGN KEY ([CompanyTypeId]) REFERENCES [dbo].[CompanyType] ([CompanyTypeId])
);

