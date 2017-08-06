CREATE TABLE [dbo].[MarsUser] (
    [MarsUserId]    INT          IDENTITY (1, 1) NOT NULL,
    [EmployeeId]    VARCHAR (10) NOT NULL,
    [CompanyId]     INT          CONSTRAINT [DF_MarsUser_CompanyId] DEFAULT ((1)) NULL,
    [CompanyTypeId] INT          CONSTRAINT [DF_MarsUser_MarsUserTypeId] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([MarsUserId] ASC),
    CONSTRAINT [FK_MarsUser_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]),
    CONSTRAINT [FK_MarsUser_CompanyType] FOREIGN KEY ([CompanyTypeId]) REFERENCES [dbo].[CompanyType] ([CompanyTypeId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueEmployeeId]
    ON [dbo].[MarsUser]([EmployeeId] ASC);

