
CREATE PROCEDURE [dbo].[NonRevApproveVehicles]
(
	@csvEntryIds as varchar(1000),
	@approvalId as int,
	@userId as varchar(20)
)
AS
BEGIN
	
	insert into [dbo].[VehicleNonRevApprovalEntry] (VehicleNonRevApprovalId, DaysNonRevAtApproval, OwningCountry, OwningArea, LicencePlate, Vin, Reason, ExpectedResolutionDate, Remark, UserId, ApprovedDateTime, ModelDescription, OperationalStatus, MovementType,  [FleetType], [CarGroup], [UnitNumber])
	select @approvalId
		, v.DaysSinceLastRevenueMovement
		, v.OwningCountry
		, v.OwningArea
		, v.LicensePlate
		, v.Vin
		, r.Reason
		, r.ExpectedResolutionDate
		, r.Remark
		, @userId
		, getdate()
		, v.ModelDescription
		, os.OperationalStatusCode
		, mt.MovementTypeCode
		, ft.FleetTypeName
		, v.CarGroup
		, v.UnitNumber
	from dbo.[fn_CSVToTable](@csvEntryIds)
	join [dbo].[VehicleNonRevPeriodEntry] pe on ColumnData = pe.VehicleNonRevPeriodEntryId
	join settings.Operational_Status os on pe.OperationalStatusId = os.OperationalStatusId
	join settings.Movement_Types mt on pe.MovementTypeId = mt.MovementTypeId
	join [dbo].[VehicleNonRevPeriod] p on pe.VehicleNonRevPeriodId = p.VechicleNonRevPeriodId
	join [dbo].[Vehicle] v on p.VehicleId = v.VehicleId
	join VehicleFleetType ft on v.[VehicleFleetTypeId] = ft.VehicleFleetTypeId
	left join (
		select [VehicleNonRevPeriodEntryId], [ExpectedResolutionDate]
				, rl.RemarkText as Reason, r.Remark
				
		from [dbo].[VehicleNonRevPeriodEntryRemark] r
		join settings.NonRev_Remarks_List rl on r.RemarkId = rl.RemarkId
		where r.VehicleNonRevPeriodEntryRemarkId in 
		(
			select max(r.VehicleNonRevPeriodEntryRemarkId)
			from [dbo].[VehicleNonRevPeriodEntryRemark] r
			join dbo.[fn_CSVToTable](@csvEntryIds) entries on r.VehicleNonRevPeriodEntryId = entries.ColumnData
			group by r.VehicleNonRevPeriodEntryId
		) 
	) r on pe.VehicleNonRevPeriodEntryId = r.VehicleNonRevPeriodEntryId
	
	
	
	

END