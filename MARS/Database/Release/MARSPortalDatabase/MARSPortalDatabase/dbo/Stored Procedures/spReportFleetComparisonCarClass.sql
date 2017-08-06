-- =============================================
-- Author:		Gavin Williams
-- Create date: 24-7-2012
-- Description:	Returns the car classes according to the parameters recieved
--				Leaves the processing for the topic to the calling class
-- =============================================
CREATE PROCEDURE [dbo].[spReportFleetComparisonCarClass]
	
	@country				VARCHAR(10) = NULL,
	@cms_pool_id			INT			= NULL,
	@cms_location_group_id	INT			= NULL,
	@ops_region_id			INT			= NULL,
	@ops_area_id			INT			= NULL,	
	@location				VARCHAR(50) = NULL,	
	@fleet_name				VARCHAR(50) = NULL,
	@car_segment_id			INT = NULL,
	@car_class_id			INT = NULL,	
	@start_date				DATETIME,
	@end_date				DATETIME,
	@day_of_week			INT = NULL,	
	@select_by				VARCHAR(10)
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		cc.car_class as car_class,
		SUM(fes.rt) AS rt,
		SUM(fes.cu) AS cu,
		SUM(fes.TOTAL_FLEET)AS total_fleet,
		SUM(fes.CARSALES)as carsales,
		SUM(fes.HA) AS ha,
		SUM(fes.HL) AS hl,
		SUM(fes.LL) AS ll,
		SUM(fes.NC) AS nc,
		SUM(fes.PL) AS pl,
		SUM(fes.TC) AS tc,
		SUM(fes.SV) AS sv,
		SUM(fes.WS) AS ws,
		SUM(fes.OPERATIONAL_FLEET) AS operational_fleet,
		SUM(fes.BD) AS bd,
		SUM(fes.MM) AS mm,
		SUM(fes.TW) AS tw,
		SUM(fes.TB) AS tb,
		SUM(fes.FS) AS fs,
		SUM(fes.RL) AS rl,
		SUM(fes.RP) AS rp,
		SUM(fes.TN) AS tn,
		SUM(fes.AVAILABLE_FLEET) AS available_fleet,
		SUM(fes.SU) AS su,
		SUM(fes.GOLD) AS gold,
		SUM(fes.PREDELIVERY) AS predelivery,
		SUM(fes.OVERDUE) AS overdue,
		SUM(fes.ON_RENT) AS on_rent
	FROM dbo.CAR_SEGMENTS as cs
	JOIN dbo.CAR_CLASSES	AS cc on cs.car_segment_id = cc.car_segment_id
	JOIN dbo.CAR_GROUPS		AS cg on cg.car_class_id = cc.car_class_id
	JOIN dbo.FLEET_EUROPE_SUMMARY_HISTORY AS fes on fes.CAR_GROUP = cg.car_group -- Changed for new DB
	JOIN dbo.LOCATIONS		AS loc on loc.location = fes.LOCATION
	JOIN dbo.CMS_LOCATION_GROUPS AS clg on clg.cms_location_group_id = loc.cms_location_group_id
	JOIN dbo.CMS_POOLS		AS cp on cp.cms_pool_id = clg.cms_pool_id
	JOIN dbo.OPS_AREAS		AS osa on osa.ops_area_id = loc.ops_area_id
	JOIN dbo.OPS_REGIONS	AS osr on osr.ops_region_id = osa.ops_region_id	
	WHERE 
		((@country = fes.COUNTRY and @country = cs.country) or @country is null)
	AND (@cms_location_group_id = loc.cms_location_group_id or @cms_location_group_id is null)
	AND (@cms_pool_id = cp.cms_pool_id or @cms_pool_id is null)
	AND (@ops_area_id = osa.ops_area_id or @ops_area_id is null)
	AND (@ops_region_id = osr.ops_region_id or @ops_region_id is null)
	AND (@location=loc.location or @location is null)
	AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0) AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet	
	AND (@car_class_id = cc.car_class_id or @car_class_id is null)
	AND (@car_segment_id = cs.car_segment_id or @car_segment_id is null)
	AND (FES.REP_DATE >= @start_date AND FES.REP_DATE <= @end_date)
	AND ((FES.REP_DAY_OF_WEEK = @day_of_week) OR (@day_of_week IS NULL))		
	GROUP BY 
		cc.car_class
		
end