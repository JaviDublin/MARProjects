CREATE TABLE [dbo].[MarsUserRoleMenuAccess] (
    [MarsUserRoleMenuAccessId] INT IDENTITY (1, 1) NOT NULL,
    [MarsUserRoleId]           INT NOT NULL,
    [UrlId]                    INT NOT NULL,
    CONSTRAINT [PK_RoleMenuAccess] PRIMARY KEY CLUSTERED ([MarsUserRoleMenuAccessId] ASC),
    CONSTRAINT [FK_UserRoleMenuAccess_RibbonMenu] FOREIGN KEY ([UrlId]) REFERENCES [Settings].[RibbonMenu] ([UrlId]),
    CONSTRAINT [FK_UserRoleMenuAccess_UserRole] FOREIGN KEY ([MarsUserRoleId]) REFERENCES [dbo].[MarsUserRole] ([MarsUserRoleId])
);

