CREATE TABLE [dbo].[MarsUserType] (
    [MarsUserTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [UserType]       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MarsUserType] PRIMARY KEY CLUSTERED ([MarsUserTypeId] ASC)
);

