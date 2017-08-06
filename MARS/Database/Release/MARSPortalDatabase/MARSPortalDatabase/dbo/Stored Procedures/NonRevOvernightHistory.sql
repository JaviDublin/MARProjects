
CREATE PROCEDURE [dbo].[NonRevOvernightHistory]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into vehicleHistory
	select v.VehicleId,  v.LastOperationalStatusId, v.VehicleFleetTypeId, v.IsNonRev
		, case when v.IsNonRev = 1 then dbo.[fnGetLatestRemarkForNonRevVehicle](v.VehicleId) else null end as ReasonId
		, cast(getdate() as date) as TimeStamp
		, v.DaysSinceLastRevenueMovement as DaysNonRev
		, v.LastMovementTypeId, v.LastLocationCode, v.IsFleet
	from Vehicle v
	--join LOCATIONS l on v.LastLocationCode = l.location
END