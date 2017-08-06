CREATE TABLE [dbo].[ResLoyaltyProgram] (
    [ResLoyaltyProgramId] INT           IDENTITY (1, 1) NOT NULL,
    [ResGoldLevelId]      INT           NOT NULL,
    [LoyaltyProgramName]  VARCHAR (200) NULL,
    [N1Type]              CHAR (2)      NULL,
    CONSTRAINT [PK_ResLoyaltyProgram] PRIMARY KEY CLUSTERED ([ResLoyaltyProgramId] ASC),
    CONSTRAINT [FK_ResLoyaltyProgram_ResGoldLevel] FOREIGN KEY ([ResGoldLevelId]) REFERENCES [dbo].[ResGoldLevel] ([ResGoldLevelId])
);

