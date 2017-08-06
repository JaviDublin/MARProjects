CREATE TABLE [Settings].[RibbonMenu] (
    [MenuId]      INT            NULL,
    [ParentId]    INT            NULL,
    [UrlId]       INT            IDENTITY (1, 1) NOT NULL,
    [Url]         NVARCHAR (255) NOT NULL,
    [Position]    TINYINT        NULL,
    [Enabled]     BIT            NULL,
    [Title]       NVARCHAR (25)  NULL,
    [Description] NVARCHAR (50)  NULL,
    [IconUrl]     NVARCHAR (255) NULL,
    CONSTRAINT [PK_RibbonMenu] PRIMARY KEY CLUSTERED ([UrlId] ASC, [Url] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_RibbonMenu]
    ON [Settings].[RibbonMenu]([UrlId] ASC);

