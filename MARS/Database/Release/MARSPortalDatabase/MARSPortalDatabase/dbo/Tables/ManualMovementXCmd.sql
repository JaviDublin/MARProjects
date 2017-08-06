CREATE TABLE [dbo].[ManualMovementXCmd] (
    [Descriptor] VARCHAR (10)  NOT NULL,
    [Command]    VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_ManualMovementXCmd] PRIMARY KEY CLUSTERED ([Descriptor] ASC)
);

