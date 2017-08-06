CREATE TABLE [dbo].[ResRemarks] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [SeqNbr]     INT           NULL,
    [ResRmkType] VARCHAR (50)  NULL,
    [Remark]     VARCHAR (MAX) NULL,
    [ResIdNbr]   VARCHAR (14)  NULL,
    CONSTRAINT [PK_ResRemrks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResRemarks_Reservations] FOREIGN KEY ([ResIdNbr]) REFERENCES [dbo].[Reservations] ([RES_ID_NBR])
);















GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20140121-153953]
    ON [dbo].[ResRemarks]([ResIdNbr] ASC);

