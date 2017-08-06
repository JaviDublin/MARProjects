CREATE TABLE [Fao].[CurrencyRate] (
    [CurrencyRateId] INT        IDENTITY (1, 1) NOT NULL,
    [Year]           TINYINT    NOT NULL,
    [UsdRate]        FLOAT (53) NOT NULL,
    [EuroRate]       FLOAT (53) NOT NULL,
    [SterlingRate]   FLOAT (53) NOT NULL,
    CONSTRAINT [PK_CurrencyRate] PRIMARY KEY CLUSTERED ([CurrencyRateId] ASC)
);

