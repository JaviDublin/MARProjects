﻿CREATE TABLE [dbo].[FLEET_LOOKUP] (
    [fleet_name]  VARCHAR (50) NOT NULL,
    [FleetNameId] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_FLEET_LOOKUP_1] PRIMARY KEY CLUSTERED ([FleetNameId] ASC)
);



