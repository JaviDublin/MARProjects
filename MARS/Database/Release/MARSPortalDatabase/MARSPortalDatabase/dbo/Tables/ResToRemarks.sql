CREATE TABLE [dbo].[ResToRemarks] (
    [ResIdNbr] VARCHAR (14) NOT NULL,
    [ResRmkId] INT          NOT NULL,
    CONSTRAINT [PK_ResToRemarks] PRIMARY KEY CLUSTERED ([ResIdNbr] ASC, [ResRmkId] ASC),
    CONSTRAINT [FK_ResToRemarks_Reservations] FOREIGN KEY ([ResIdNbr]) REFERENCES [dbo].[Reservations] ([RES_ID_NBR]),
    CONSTRAINT [FK_ResToRemarks_ResRemarks] FOREIGN KEY ([ResRmkId]) REFERENCES [dbo].[ResRemarks] ([Id])
);

