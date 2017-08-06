CREATE TABLE [dbo].[Users] (
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [GlobalId]  NVARCHAR (20)    NOT NULL,
    [DomainId]  TINYINT          NOT NULL,
    [Email]     NVARCHAR (256)   NULL,
    [IsActive]  BIT              NOT NULL,
    [HasLeft]   BIT              NOT NULL,
    [Firstname] NVARCHAR (50)    NULL,
    [Surname]   NVARCHAR (50)    NULL,
    [Fullname]  NVARCHAR (101)   NULL
);

