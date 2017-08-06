CREATE TABLE [dbo].[Vehicle] (
    [VehicleId]                        INT             IDENTITY (1, 1) NOT NULL,
    [OwningCountry]                    VARCHAR (2)     NULL,
    [OwningArea]                       VARCHAR (5)     NULL,
    [DaysInOwningArea]                 INT             NULL,
    [OwningLocationCode]               VARCHAR (10)    NULL,
    [Vin]                              VARCHAR (20)    NULL,
    [LicensePlate]                     VARCHAR (20)    NULL,
    [UnitNumber]                       INT             NULL,
    [TasModelCode]                     VARCHAR (10)    NULL,
    [ModelGroup]                       VARCHAR (50)    NULL,
    [ModelDescription]                 VARCHAR (50)    NULL,
    [ManufacturerCode]                 VARCHAR (50)    NULL,
    [VisionManufacturerCode]           VARCHAR (10)    NULL,
    [VisionModelCode]                  VARCHAR (50)    NULL,
    [CarGroup]                         VARCHAR (10)    NULL,
    [LastArea]                         VARCHAR (5)     NULL,
    [LastLocationCode]                 VARCHAR (10)    NULL,
    [LastChangeDateTime]               DATETIME        NULL,
    [LastDocumentNumber]               VARCHAR (20)    NULL,
    [LastMilage]                       INT             NULL,
    [LastDriverName]                   VARCHAR (200)   NULL,
    [DaysInCountry]                    INT             NULL,
    [DaysInArea]                       INT             NULL,
    [DaysInLocation]                   INT             NULL,
    [DaysSinceLastMovement]            INT             NULL,
    [PreviousLocationCode]             VARCHAR (10)    NULL,
    [DaysSinceLastRevenueMovement]     INT             NULL,
    [LastRevenueMovementDate]          DATETIME        NULL,
    [LastRevenueMovementLocationCode]  VARCHAR (10)    NULL,
    [LastRevenueMovementStatus]        VARCHAR (10)    NULL,
    [OnRentOrIdleIndicator]            VARCHAR (10)    NULL,
    [LastOperationalStatusId]          INT             NOT NULL,
    [DaysInCurrentOperationalStatus]   INT             NULL,
    [LastDocumentType]                 VARCHAR (10)    NULL,
    [LastDocumentStatus]               VARCHAR (10)    NULL,
    [LastMovementTypeId]               INT             NOT NULL,
    [ExpectedArea]                     INT             NULL,
    [ExpectedLocationCode]             VARCHAR (10)    NULL,
    [ExpectedDateTime]                 DATETIME        NULL,
    [CarGroupCharged]                  VARCHAR (10)    NULL,
    [InstallationDate]                 DATE            NULL,
    [InstallationDeliveryDate]         DATE            NULL,
    [InstallationMsoDate]              DATE            NULL,
    [DaysInService]                    INT             NULL,
    [FirstRevenueMovementDate]         DATE            NULL,
    [FirstRevenueMovementLocationCode] VARCHAR (10)    NULL,
    [InstallationModelYear]            INT             NULL,
    [InstallationVendorNumber]         INT             NULL,
    [InstallationPoNumber]             VARCHAR (20)    NULL,
    [SpiCode]                          VARCHAR (20)    NULL,
    [DepreciationStatus]               VARCHAR (10)    NULL,
    [VisionClassCode]                  VARCHAR (10)    NULL,
    [VisionTypeCode]                   VARCHAR (10)    NULL,
    [HoldFlag1]                        VARCHAR (10)    NULL,
    [HoldFlag2]                        VARCHAR (10)    NULL,
    [HoldFlag3]                        VARCHAR (10)    NULL,
    [CapitalCost]                      DECIMAL (16, 2) NULL,
    [BaseAndOptions]                   DECIMAL (16, 2) NULL,
    [DepreciationAmount]               DECIMAL (16, 2) NULL,
    [BookValue]                        DECIMAL (16, 2) NULL,
    [Colour]                           VARCHAR (20)    NULL,
    [DaysInMm]                         INT             NULL,
    [DaysInBd]                         INT             NULL,
    [ReturnTime]                       VARCHAR (10)    NULL,
    [DaysUntilBlockDate]               INT             NULL,
    [BlockDate]                        DATE            NULL,
    [BlockWarningMileage]              INT             NULL,
    [BlockMileage]                     INT             NULL,
    [SaleProcessDate]                  DATE            NULL,
    [SaleDate]                         DATE            NULL,
    [VehicleFleetTypeId]               INT             NOT NULL,
    [IsFleet]                          BIT             NOT NULL,
    [IsNonRev]                         BIT             NOT NULL,
    CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED ([VehicleId] ASC),
    CONSTRAINT [FK_Vehicle_MovementType] FOREIGN KEY ([LastMovementTypeId]) REFERENCES [Settings].[Movement_Types] ([MovementTypeId]),
    CONSTRAINT [FK_Vehicle_OperationalStatus] FOREIGN KEY ([LastOperationalStatusId]) REFERENCES [Settings].[Operational_Status] ([OperationalStatusId]),
    CONSTRAINT [FK_Vehicle_VehicleFleetType] FOREIGN KEY ([VehicleFleetTypeId]) REFERENCES [dbo].[VehicleFleetType] ([VehicleFleetTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_MovementType]
    ON [dbo].[Vehicle]([LastMovementTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OperationalStatus]
    ON [dbo].[Vehicle]([LastOperationalStatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VehicleFleetType]
    ON [dbo].[Vehicle]([VehicleFleetTypeId] ASC);


GO
CREATE TRIGGER [dbo].[TriggerNonRevStatusInserted] ON  [dbo].[Vehicle]
FOR INSERT
AS  
begin

	select VehicleId, LastChangeDateTime, LastOperationalStatusId, LastMovementTypeId
			, LastLocationCode, DaysSinceLastRevenueMovement
	into #NewNonRevVehicleIds
	from inserted 
	where not (LastMovementTypeId = 10 and LastOperationalStatusId = 12)

	INSERT INTO VehicleNonRevPeriod(VehicleId, EnteredNonRevTimestamp, LeftNonRevTimestamp, EnteredNonRevLastUpdate, LeftNonRevLastUpdate, Active)
	SELECT VehicleId, getdate(), null, LastChangeDateTime, null, 1
	from #NewNonRevVehicleIds

	INSERT INTO VehicleNonRevPeriodEntry(VehicleNonRevPeriodId, OperationalStatusId, MovementTypeId
				, LastLocationCode, [TimeStamp], LastChangeDateTime, DaysNonRev)
	SELECT nrp.VechicleNonRevPeriodId, LastOperationalStatusId, LastMovementTypeId, LastLocationCode, getdate()
			, vid.LastChangeDateTime, vId.DaysSinceLastRevenueMovement
	from #NewNonRevVehicleIds vId
	join VehicleNonRevPeriod nrp on vid.VehicleId = nrp.VehicleId
	where nrp.Active = 1

	--Update NonRev Bit
	Update Vehicle
	set IsNonRev = case when LastOperationalStatusId = 12 and LastMovementTypeId = 10 then 0 else 1 end
	


	
end
GO
CREATE TRIGGER [dbo].[TriggerNonRevStatusChanged] ON  [dbo].[Vehicle]
FOR UPDATE
AS  
begin

	IF NOT EXISTS (SELECT * FROM DELETED)				--We are inserting a new row, handled by another trigger
	BEGIN
		RETURN
	END
		
	--Get the Vehicles that have changed either Operational Status or Movement Type
	select del.VehicleId
			, ins.LastChangeDateTime as NewLastChangeDateTime
			, ins.LastOperationalStatusId as NewLastOperationalStatusId
			, ins.LastMovementTypeId as NewLastMovementTypeId
			, ins.LastLocationCode as NewLastLocationCode
			, del.DaysSinceLastRevenueMovement as DaysSinceRev
			, ins.VehicleFleetTypeId as NewVehicleFleetTypeId
	into #VehiclesThatHaveChangedStatus
	from inserted ins
	join deleted del on ins.VehicleId = del.VehicleId
	where (ins.LastOperationalStatusId <> del.LastOperationalStatusId 
		or del.LastMovementTypeId <> ins.LastMovementTypeId)
		and ins.LastChangeDateTime > del.LastChangeDateTime
		



	IF NOT EXISTS (SELECT * FROM #VehiclesThatHaveChangedStatus)			--Nothing has changed, Return
	BEGIN
		RETURN
	END

	--If a Vehicle doesn't have an active Period of NonRev Create one
	insert into VehicleNonRevPeriod(VehicleId, EnteredNonRevTimestamp, LeftNonRevTimestamp
				, EnteredNonRevLastUpdate, LeftNonRevLastUpdate, Active)
	select v.VehicleId, getdate(), null, NewLastChangeDateTime, null, 1
	from #VehiclesThatHaveChangedStatus v
	where v.VehicleId not in
	(
		select VehicleId
		from VehicleNonRevPeriod p
		where p.Active = 1
	)

	--Insert a history Entry for their current active Period
	insert into VehicleNonRevPeriodEntry(VehicleNonRevPeriodId, OperationalStatusId, MovementTypeId
					, LastLocationCode, TimeStamp, LastChangeDateTime, VehicleFleetTypeId, DaysNonRev)
	select p.VechicleNonRevPeriodId, v.NewLastOperationalStatusId, v.NewLastMovementTypeId, 
		NewLastLocationCode, getdate(), v.NewLastChangeDateTime, v.NewVehicleFleetTypeId, DaysSinceRev
	from #VehiclesThatHaveChangedStatus v
	join VehicleNonRevPeriod p on v.VehicleId = p.VehicleId
	where p.Active = 1

	
	--Update Periods Active Status
	Update p
	set p.Active = 0
		, p.LeftNonRevTimestamp = getdate()
		, p.LeftNonRevLastUpdate = NewLastChangeDateTime
		, p.DaysInNonRev = v.DaysSinceRev
	from VehicleNonRevPeriod p
	join #VehiclesThatHaveChangedStatus v on p.VehicleId = v.VehicleId
	where p.Active = 1 and NewLastOperationalStatusId = 12 and NewLastMovementTypeId = 10

	
	--Update NonRev Bit
	Update Vehicle
	set IsNonRev = case when LastOperationalStatusId = 12 and LastMovementTypeId = 10 then 0 else 1 end
end