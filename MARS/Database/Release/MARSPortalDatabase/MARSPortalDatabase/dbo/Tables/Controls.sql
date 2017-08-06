CREATE TABLE [dbo].[Controls] (
    [ControlId]          INT           NOT NULL,
    [ParentId]           INT           NULL,
    [ControlUrl]         INT           IDENTITY (1, 1) NOT NULL,
    [Position]           INT           NULL,
    [IsActive]           BIT           NULL,
    [HelpEnabled]        BIT           NULL,
    [MenuEnabled]        BIT           NULL,
    [PermissionsEnabled] BIT           NULL,
    [Name]               VARCHAR (25)  NULL,
    [Description]        VARCHAR (100) NULL,
    [ImageUrl]           VARCHAR (100) NULL,
    [Url]                VARCHAR (100) NULL
);

