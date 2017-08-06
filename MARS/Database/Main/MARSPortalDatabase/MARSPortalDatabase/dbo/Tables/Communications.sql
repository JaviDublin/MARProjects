CREATE TABLE [dbo].[Communications] (
    [CommunicationsID] INT            IDENTITY (1, 1) NOT NULL,
    [CommDate]         DATE           NULL,
    [UpdatedBy]        VARCHAR (50)   NULL,
    [Heading]          VARCHAR (250)  NULL,
    [Details]          VARCHAR (1000) NULL,
    [IsActive]         BIT            NULL,
    [Priority]         BIT            NULL,
    CONSTRAINT [PK_Communications] PRIMARY KEY CLUSTERED ([CommunicationsID] ASC)
);



