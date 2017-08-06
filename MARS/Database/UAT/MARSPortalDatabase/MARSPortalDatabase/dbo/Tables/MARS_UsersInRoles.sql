CREATE TABLE [dbo].[MARS_UsersInRoles] (
    [userId]     VARCHAR (20) NOT NULL,
    [roleId]     INT          NOT NULL,
    [country]    VARCHAR (50) NULL,
    [userRoleId] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_MARS_UsersInRoles] PRIMARY KEY CLUSTERED ([userRoleId] ASC),
    CONSTRAINT [FK_MARS_UsersInRoles_MARS_Roles] FOREIGN KEY ([roleId]) REFERENCES [dbo].[MARS_Roles] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MARS_UsersInRoles_MARS_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[MARS_Users] ([racfId]) ON DELETE CASCADE
);





