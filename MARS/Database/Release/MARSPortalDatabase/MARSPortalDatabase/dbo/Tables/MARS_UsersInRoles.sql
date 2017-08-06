CREATE TABLE [dbo].[MARS_UsersInRoles] (
    [userId]  VARCHAR (10) NOT NULL,
    [roleId]  INT          NOT NULL,
    [country] VARCHAR (50) NULL,
    CONSTRAINT [FK_MARS_UsersInRoles_MARS_Roles] FOREIGN KEY ([roleId]) REFERENCES [dbo].[MARS_Roles] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MARS_UsersInRoles_MARS_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[MARS_Users] ([racfId]) ON DELETE CASCADE
);

