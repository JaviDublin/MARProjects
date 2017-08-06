
CREATE FUNCTION [dbo].[fnGetLatestRemarkForNonRevVehicle]
(
	@vehicleId INT
)
RETURNS int
AS
BEGIN
	
	declare @lastRemarkId int = 0
	declare @latestPeriodId int = 0;
	
	set @latestPeriodId = (select top 1 p.VechicleNonRevPeriodId
							from VehicleNonRevPeriod p
							where p.VehicleId = @vehicleId
							order by p.VechicleNonRevPeriodId desc
							)
	set @lastRemarkId = (select top 1 per.RemarkId
	from VehicleNonRevPeriodEntry pe
	join VehicleNonRevPeriodEntryRemark per on pe.VehicleNonRevPeriodEntryId = per.VehicleNonRevPeriodEntryId
	where pe.VehicleNonRevPeriodId = @latestPeriodId
	and cast(per.ExpectedResolutionDate as Date) >= cast(getdate() as date)
	order by per.VehicleNonRevPeriodEntryRemarkId desc
	)

	RETURN @lastRemarkId
	

END