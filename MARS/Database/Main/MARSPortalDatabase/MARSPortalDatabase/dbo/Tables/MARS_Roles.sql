CREATE TABLE [dbo].[MARS_Roles] (
    [id]          INT            NOT NULL,
    [roleName]    VARCHAR (50)   NOT NULL,
    [description] VARCHAR (50)   NOT NULL,
    [pageAccess]  VARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_MARS_Roles] PRIMARY KEY CLUSTERED ([id] ASC)
);

