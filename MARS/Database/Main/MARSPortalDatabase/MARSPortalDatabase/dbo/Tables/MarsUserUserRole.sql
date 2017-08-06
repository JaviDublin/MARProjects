CREATE TABLE [dbo].[MarsUserUserRole] (
    [MarsUserUserRoleId] INT IDENTITY (1, 1) NOT NULL,
    [MarsUserId]         INT NOT NULL,
    [MarsUserRoleId]     INT NOT NULL,
    CONSTRAINT [PK_UserUserRole] PRIMARY KEY CLUSTERED ([MarsUserUserRoleId] ASC),
    CONSTRAINT [FK_UserUserRole_User] FOREIGN KEY ([MarsUserId]) REFERENCES [dbo].[MarsUser] ([MarsUserId]),
    CONSTRAINT [FK_UserUserRole_UserRole] FOREIGN KEY ([MarsUserRoleId]) REFERENCES [dbo].[MarsUserRole] ([MarsUserRoleId])
);

