
CREATE PROCEDURE [dbo].[PopulateVehicleTableFromFea]
AS
BEGIN
	SET NOCOUNT ON;
	

    
	MERGE INTO Vehicle as Target
	USING ( 
			select fea.*, vft.VehicleFleetTypeId
					, case when vft.VehicleFleetTypeId  not in (2,3) and TOTAL_FLEET = 1
						then 1				--Everything that isn't Inactive or Sold and has TOTAL_FLEET = 1
						else 0 end as IsFleet
					, os.[OperationalStatusId]
					, mt.[MovementTypeId]
			from Fleet_Europe_Actual fea
			join VehicleFleetType vft on 
				case when fea.SDATE is not null and SDATE > cast(getdate() -1 as date) 
							then 3			--Sold when SoldDate > Yesterday 
					when fea.FLEET_RAC_OPS = 1
							then 4			--RAC OPS
					when fea.FLEET_RAC_TTL = 1 
							then 5			--RAC Non Ops
					when fea.FLEET_CARSALES = 1
							then 6			--CARSALES
					when fea.FLEET_LICENSEE = 1
							then 7			--LICENSEE
				    when fea.FLEET_ADV = 1
							then 8			--FIREFLY
					when fea.FLEET_HOD = 1
							then 9			--HERTZ ON DEMAND
					else 1 end				--Default is Undefined

				= vft.VehicleFleetTypeId
			join [Settings].[Operational_Status] os on fea.OPERSTAT = os.[OperationalStatusCode]
			join [Settings].[Movement_Types] mt on fea.MOVETYPE = mt.[MovementTypeCode]
			)
		as Source
		on Source.SERIAL = Target.Vin
			and Source.Country = Target.OwningCountry
			and Source.IDATE = Target.InstallationDate
	WHEN MATCHED THEN
		UPDATE SET Target.IsFleet = Source.IsFleet
					, Target.VehicleFleetTypeId = Source.VehicleFleetTypeId
					, Target.OwningArea = Source.OWNAREA, Target.OwningLocationCode = Source.OwnWWD, Target.Vin = Source.SERIAL
					, Target.LicensePlate = Source.LICENSE, Target.UnitNumber = Source.UNIT, Target.TasModelCode = Source.Model
					, Target.ModelGroup = Source.MODGROUP, Target.ModelDescription = Source.MODDESC, Target.VisionModelCode = Source.VISMODEL
					, Target.CarGroup = Source.VC, Target.LastArea = Source.LSTAREA, Target.LastLocationCode = Source.LSTWWD
					, Target.LastChangeDateTime = Source.LSTDATE + Source.LSTTIME, Target.LastDocumentNumber = Source.LSTNO
					, Target.LastMilage = Source.LSTMLG, Target.LastDriverName = Source.DRVNAME, Target.DaysInCountry = Source.DAYSCTRY
					, Target.DaysInArea = Source.DAYSAREA, Target.DaysSinceLastMovement = Source.DAYSMOVE, Target.PreviousLocationCode = Source.PREVWWD
					, Target.DaysSinceLastRevenueMovement = Source.DAYSREV, Target.LastRevenueMovementDate = Source.RADATE
					, Target.LastRevenueMovementLocationCode = Source.RALOC, Target.LastOperationalStatusId = Source.[OperationalStatusId]
					, Target.DaysInCurrentOperationalStatus = Source.OPERDAYS, Target.LastDocumentType = Source.LSTTYPE
					, Target.LastDocumentStatus = Source.LSTOORC, Target.LastMovementTypeId = Source.MovementTypeId, Target.ExpectedArea = Source.DUEAREA
					, Target.ExpectedLocationCode = Source.DUEWWD, Target.ExpectedDateTime = isnull(Source.DUEDATE, '1900-01-01') + Source.DUETIME
					, Target.CarGroupCharged = Source.RC, Target.InstallationDate = Source.IDATE, Target.InstallationMsoDate = Source.MSODATE
					, Target.InstallationVendorNumber = Source.VENDNBR, Target.InstallationPoNumber = Source.PONO
					, Target.DepreciationStatus = Source.DEPSTAT, Target.VisionClassCode = Source.VEHCLAS, Target.VisionTypeCode = Source.VEHTYPE
					, Target.HoldFlag1 = Source.CARHOLD1, Target.CapitalCost = Source.CAPCOST, Target.BaseAndOptions = Source.BASEOP
					, Target.DepreciationAmount = Source.DEPAMT, Target.Colour = Source.COLOR, Target.DaysInMm = Source.MMDAYS
					, Target.DaysInBd = Source.BDDAYS, Target.DaysUntilBlockDate = Source.TERMDAYS, Target.BlockDate = Source.CAPDATE
					, Target.SaleProcessDate = Source.SPROCDAT, Target.SaleDate = Source.SDATE
					 
	WHEN NOT MATCHED BY Target THEN
		INSERT ( OwningCountry, OwningArea, DaysInOwningArea, OwningLocationCode, Vin, LicensePlate, UnitNumber, TasModelCode
				, ModelGroup, ModelDescription, ManufacturerCode, VisionManufacturerCode, VisionModelCode, CarGroup, LastArea, LastLocationCode
				, LastChangeDateTime, LastDocumentNumber, LastMilage, LastDriverName, DaysInCountry, DaysInArea, DaysInLocation
				, DaysSinceLastMovement, PreviousLocationCode, DaysSinceLastRevenueMovement, LastRevenueMovementDate, LastRevenueMovementLocationCode
				, LastRevenueMovementStatus, OnRentOrIdleIndicator, LastOperationalStatusId, DaysInCurrentOperationalStatus, LastDocumentType
				, LastDocumentStatus, LastMovementTypeId, ExpectedArea, ExpectedLocationCode, ExpectedDateTime, CarGroupCharged
				, InstallationDate, InstallationDeliveryDate, InstallationMsoDate, DaysInService, FirstRevenueMovementDate
				, FirstRevenueMovementLocationCode, InstallationModelYear, InstallationVendorNumber, InstallationPoNumber, SpiCode
				, DepreciationStatus, VisionClassCode, VisionTypeCode, HoldFlag1, HoldFlag2, HoldFlag3, CapitalCost, BaseAndOptions
				, DepreciationAmount, BookValue, Colour, DaysInMm, DaysInBd, ReturnTime, DaysUntilBlockDate, BlockDate
				, BlockWarningMileage, BlockMileage, SaleProcessDate, SaleDate, VehicleFleetTypeId, IsFleet, IsNonRev)
		VALUES (COUNTRY, OWNAREA, null, OWNWWD, SERIAL, LICENSE, UNIT, MODEL, MODGROUP, MODDESC, null, null, VISMODEL, VC
				, LSTAREA, LSTWWD, LSTDATE + LSTTIME, LSTNO,LSTMLG, DRVNAME, DAYSCTRY, DAYSAREA, null, DAYSMOVE, PREVWWD, DAYSREV
				, RADATE, RALOC, null, null, OperationalStatusId, OPERDAYS, LSTTYPE, LSTOORC, Source.MovementTypeId, DUEAREA, DUEWWD, isnull(DUEDATE, '1900-01-01') + DUETIME
				, RC, IDATE, null, MSODATE, null, null, null, null, VENDNBR, PONO, null, DEPSTAT, VEHCLAS, VEHTYPE, CARHOLD1, null, null
				, CAPCOST, BASEOP, DEPAMT, null, COLOR, MMDAYS, BDDAYS, null, TERMDAYS, CAPDATE, null, null, SPROCDAT, SDATE
				, VehicleFleetTypeId, IsFleet, 0)
	
	WHEN NOT MATCHED BY SOURCE THEN
		UPDATE SET Target.VehicleFleetTypeId = 2		--Set to Inactive when the Entry is not in the FEA table
					, Target.IsFleet = 0
	;

END