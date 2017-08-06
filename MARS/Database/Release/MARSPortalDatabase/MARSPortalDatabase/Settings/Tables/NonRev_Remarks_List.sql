CREATE TABLE [Settings].[NonRev_Remarks_List] (
    [RemarkId]   INT           IDENTITY (1, 1) NOT NULL,
    [RemarkText] VARCHAR (100) NULL,
    [isActive]   BIT           NULL,
    PRIMARY KEY CLUSTERED ([RemarkId] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-20140425-161220]
    ON [Settings].[NonRev_Remarks_List]([RemarkText] ASC);

