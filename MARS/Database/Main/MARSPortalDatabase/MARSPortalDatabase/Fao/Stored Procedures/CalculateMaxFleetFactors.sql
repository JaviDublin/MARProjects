
create PROCEDURE [Fao].[CalculateMaxFleetFactors]

AS
BEGIN
	SET NOCOUNT ON;

	declare @country varchar(10) = 'GE'
	declare @fromDate date = dateadd(w, -7, '2015-01-04') --getdate())

	truncate table [Fao].MaxFleetFactor

	insert into [Fao].MaxFleetFactor( LocationId, CarGroupId, DayOfWeekId, NonRevPercentage, UtilizationPercentage)
	SELECT fh.LocationId
				, fh.CarGroupId
				, datepart(dw, fh.Timestamp) as dow
				, case when sum(fh.PeakTotal) = 0 then 0 else cast(sum(fh.PeakIdle) as float) / sum(fh.PeakTotal) end as AvgOpPercent
				, case when sum(fh.PeakOperationalFleet) = 0 then 0 else cast(sum(fh.PeakOnRent) as float) / sum(fh.PeakOperationalFleet) end as AvgOpPercent
	FROM fleethistory fh
	
	join CAR_GROUPS cg on fh.CarGroupId = cg.car_group_id
	join CAR_CLASSES cc on cg.car_class_id = cc.car_class_id
	join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
	where fh.Timestamp >= @fromDate
		and cs.country = @country
		and fh.FleetTypeId in (4,5)
		group by fh.LocationId
				, fh.CarGroupId
				, datepart(dw, fh.Timestamp) 

END