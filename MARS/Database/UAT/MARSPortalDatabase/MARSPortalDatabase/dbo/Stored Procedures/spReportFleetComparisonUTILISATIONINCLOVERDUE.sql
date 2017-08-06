-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spReportFleetComparisonUTILISATIONINCLOVERDUE]
	
	@country				VARCHAR(10) = NULL	,
	@cms_pool_id			INT			= NULL	,
	@cms_location_group_id	int			= NULL	, 
	@ops_region_id			INT			= NULL	,
	@ops_area_id			INT			= NULL	,	
	@location				VARCHAR(50) = NULL	,	
	@fleet_name				VARCHAR(50) = NULL	,
	@car_segment_id			INT			= NULL	,
	@car_class_id			INT			= NULL	,	
	@start_date				DATETIME			,
	@end_date				DATETIME			,
	@day_of_week			INT			= NULL	,	
	@select_by				VARCHAR(10)
	
AS
BEGIN
	
	SET NOCOUNT ON;

	
	CREATE TABLE #TMP_CAR_GROUPS(
		CAR_GROUP		VARCHAR(3),
		TOPIC_COUNTER	INT,	--TOPIC: ON_RENT+OVERDUE
		GROUP_COUNTER	INT		--GROUP: OPERATIONAL_FLEET
	)

	INSERT INTO #TMP_CAR_GROUPS(CAR_GROUP, TOPIC_COUNTER, GROUP_COUNTER)	
	SELECT 
		FES.CAR_GROUP, SUM(FES.ON_RENT + FES.OVERDUE), SUM(FES.OPERATIONAL_FLEET) 
	FROM 
		FLEET_EUROPE_SUMMARY_HISTORY AS FES
	WHERE
		(FES.COUNTRY = @country OR @country IS NULL) -- Country
	AND ((FES.LOCATION IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
	AND ((FES.LOCATION IN (SELECT location FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
	AND ((FES.LOCATION IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
	AND ((FES.LOCATION IN (SELECT location FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
	AND (FES.LOCATION = @location OR @location IS NULL) -- Location
	AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0) AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet	
	AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
	AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
	AND (FES.REP_DATE >= @start_date AND FES.REP_DATE <= @end_date)
	AND ((FES.REP_DAY_OF_WEEK = @day_of_week) OR (@day_of_week IS NULL))
	GROUP BY 
		FES.REP_DATE, FES.CAR_GROUP
			

	-- DISPLAY NUMBERS OF CARS
	IF (@select_by = 'VALUE')
	BEGIN
		SELECT CAR_GROUP AS 'car_group', AVG(TOPIC_COUNTER) AS 'car_count' 
		FROM #TMP_CAR_GROUPS 
		GROUP BY CAR_GROUP 
		ORDER BY CAR_GROUP
	END
	-- Display percentage of cars in the TOPIC
	ELSE IF (@select_by = 'PERCENTAGE')
	BEGIN			
		SELECT 
			CAR_GROUP AS 'car_group', 
			CASE WHEN (SUM(GROUP_COUNTER) = 0) THEN 0 
			ELSE (SUM(TOPIC_COUNTER)*100.0)/SUM(GROUP_COUNTER) END AS 'car_count'  
		FROM #TMP_CAR_GROUPS 
		GROUP BY CAR_GROUP 
		ORDER BY CAR_GROUP
	END

	-- Drop the tempory table	
	DROP TABLE #TMP_CAR_GROUPS

END