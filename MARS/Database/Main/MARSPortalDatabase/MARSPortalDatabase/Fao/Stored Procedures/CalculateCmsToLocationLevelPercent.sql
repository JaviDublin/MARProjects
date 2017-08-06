
CREATE PROCEDURE [Fao].[CalculateCmsToLocationLevelPercent]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @fromdate date =  '2014-12-17'
declare @fleetTypeId tinyint = 4
declare @country varchar(10) = 'GE'

truncate table [CmsToLocationLevelPercent]


insert into [Fao].[CmsToLocationLevelPercent] ( CarGroupId, LocationId, LocationGroupId, PercentVehiclesAllocated)

select 
	DailyData.CarGroupId
	, DailyData.dim_Location_id
	, DailyData.cms_location_group_id
	, avg(PercentOfLocationGroup) as PercentVehiclesAllocated
from
(
	SELECT fh.Timestamp
		, fh.CarGroupId
		, l.dim_Location_id
		, cms_location_group_id
		, GroupedOnLocationGroup.TotalPeakOnRent as PeakAtLocationGroup
		, fh.PeakOnRent as PeakAtLocation
		, case when fh.PeakOnRent = 0 then 0 else (fh.PeakTotal ) / cast(GroupedOnLocationGroup.TotalPeakOnRent as float) end as PercentOfLocationGroup
	FROM FleetHistory fh
	join LOCATIONS l on fh.LocationId = l.dim_Location_id
	join CAR_GROUPS cg on fh.CarGroupId = cg.car_group_id
	join (
			SELECT fh.Timestamp
				, cms_location_group_id as LocationGroupId
				, fh.CarGroupId
				, sum(fh.PeakTotal) as TotalPeakOnRent
			FROM FleetHistory fh
			join LOCATIONS l on fh.LocationId = l.dim_Location_id
				
			join CAR_GROUPS cg on fh.CarGroupId = cg.car_group_id
			where fh.Timestamp >  @fromdate
				and l.country = @country
				and FleetTypeId = @fleetTypeId
				--and l.cms_location_group_id = 36	-- Berlin
			group by fh.Timestamp, l.cms_location_group_id, fh.CarGroupId

		) as GroupedOnLocationGroup
			on fh.Timestamp = GroupedOnLocationGroup.Timestamp
				and cms_location_group_id = GroupedOnLocationGroup.LocationGroupId
				and fh.CarGroupId = GroupedOnLocationGroup.CarGroupId
	where fh.Timestamp >  @fromdate
		and l.country = @country
		and FleetTypeId = @fleetTypeId
		--and l.cms_location_group_id = 36	-- Berlin
	
) DailyData
group by DailyData.CarGroupId
		, DailyData.dim_Location_id
		, DailyData.cms_location_group_id
END