CREATE TABLE [dbo].[ReservationTeradataControlLog] (
    [ReservationTeradataControlLogId] INT      IDENTITY (1, 1) NOT NULL,
    [HertzTimeStamp]                  DATETIME CONSTRAINT [DF_ReservationTeradataControlLog_HertzTimeStamp] DEFAULT (getdate()) NULL,
    [TeradataTimeStamp]               DATETIME NULL,
    [Processed]                       BIT      CONSTRAINT [DF_ReservationTeradataControlLog_Processed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReservationTeradataControlLog] PRIMARY KEY CLUSTERED ([ReservationTeradataControlLogId] ASC)
);

