-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spReportSiteComparisonCARHOLD_L]
	
	@country				VARCHAR(10) = NULL	,
	@cms_pool_id			INT			= NULL	,
	@cms_location_group_id	INT			= NULL	, 
	@ops_region_id			INT			= NULL	,
	@ops_area_id			INT			= NULL	,			
	@fleet_name				VARCHAR(50) = NULL	,
	@car_segment_id			INT			= NULL	,
	@car_class_id			INT			= NULL	,
	@car_group_id			INT			= NULL	,
	@start_date				DATETIME			,
	@end_date				DATETIME			,
	@day_of_week			INT			= NULL	,		
	@select_by				VARCHAR(10)			,
	@grouping_criteria		VARCHAR(20)
	
AS
BEGIN
	
	SET NOCOUNT ON;

	CREATE TABLE #TMP_LOCATIONS(
		REP_DATE		DATETIME,
		COUNTRY			VARCHAR(2) COLLATE Latin1_General_CI_AS,
		LOCATION		VARCHAR(7) COLLATE Latin1_General_CI_AS,
		TOPIC_COUNTER	INT,	--TOPIC: CARHOLD_L
		GROUP_COUNTER	INT		--GROUP: TOTAL_FLEET
	)
	
	CREATE TABLE #TMP_GROUPS(
		REP_DATE		DATETIME,
		GROUP_ID		VARCHAR(10) COLLATE Latin1_General_CI_AS,
		GROUP_NAME		VARCHAR(50) COLLATE Latin1_General_CI_AS,
		TOPIC_COUNTER	INT,	--TOPIC: RT
		GROUP_COUNTER	INT		--GROUP: AVAILABLE_FLEET
	)

	INSERT INTO #TMP_LOCATIONS(REP_DATE, COUNTRY, LOCATION, TOPIC_COUNTER, GROUP_COUNTER)	
	SELECT 
		FES.REP_DATE, FES.COUNTRY, FES.LOCATION, SUM(FES.CARHOLD_L), SUM(FES.TOTAL_FLEET) 
	FROM 
		FLEET_EUROPE_SUMMARY_HISTORY AS FES
	WHERE
		(FES.COUNTRY = @country OR @country IS NULL) -- Country
	AND ((FES.LOCATION IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
	AND ((FES.LOCATION IN (SELECT location FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
	AND ((FES.LOCATION IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
	AND ((FES.LOCATION IN (SELECT location FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS			
	AND ((@fleet_name = 'CARSALES' AND FES.FLEET_CARSALES > 0) OR (@fleet_name = 'RAC OPS' AND FES.FLEET_RAC_OPS > 0) OR (@fleet_name = 'RAC TTL' AND FES.FLEET_RAC_TTL > 0) OR (@fleet_name = 'ADVANTAGE' AND FES.FLEET_ADV > 0) OR (@fleet_name = 'HERTZ ON DEMAND' AND FES.FLEET_HOD > 0) OR (@fleet_name = 'LICENSEE' AND FES.FLEET_LICENSEE > 0) OR (@fleet_name IS NULL AND (FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0) AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)))--Fleet	
	AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
	AND ((FES.CAR_GROUP IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
	AND ((FES.CAR_GROUP = (SELECT car_group FROM CAR_GROUPS WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
	AND (FES.REP_DATE >= @start_date AND FES.REP_DATE <= @end_date)
	AND ((FES.REP_DAY_OF_WEEK = @day_of_week) OR (@day_of_week IS NULL))
	GROUP BY 
		FES.REP_DATE, FES.LOCATION, FES.COUNTRY	

	

-- CALCULATE sums of cars per group
	IF (@grouping_criteria = 'COUNTRY')
	BEGIN
		INSERT INTO #TMP_GROUPS(REP_DATE, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER)
			SELECT REP_DATE, #TMP_LOCATIONS.COUNTRY, #TMP_LOCATIONS.COUNTRY AS 'location', SUM(TOPIC_COUNTER) AS 'TOPIC_COUNTER', SUM(GROUP_COUNTER) AS 'GROUP_COUNTER' 
			FROM #TMP_LOCATIONS							
			LEFT JOIN COUNTRIES ON COUNTRIES.country = #TMP_LOCATIONS.COUNTRY
			WHERE COUNTRIES.active = 1
			GROUP BY REP_DATE, #TMP_LOCATIONS.COUNTRY
			ORDER BY #TMP_LOCATIONS.COUNTRY
	END
	ELSE 
	BEGIN
		IF (@grouping_criteria = 'CMS_POOL')
		BEGIN
			INSERT INTO #TMP_GROUPS(REP_DATE, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER)
				SELECT REP_DATE, CP.cms_pool_id, CP.cms_pool AS 'location', SUM(TOPIC_COUNTER) AS 'TOPIC_COUNTER', SUM(GROUP_COUNTER) AS 'GROUP_COUNTER' 
				FROM #TMP_LOCATIONS
				LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
				LEFT JOIN CMS_LOCATION_GROUPS AS CLG ON CLG.cms_location_group_id = L.cms_location_group_id		
				LEFT JOIN CMS_POOLS AS CP ON CP.cms_pool_id = CLG.cms_pool_id AND CP.country = #TMP_LOCATIONS.COUNTRY						
				GROUP BY REP_DATE, CP.cms_pool_id, CP.cms_pool
				ORDER BY CP.cms_pool_id
		END 
		ELSE 
		BEGIN				
			IF (@grouping_criteria = 'CMS_LOCATION_GROUP')
			BEGIN
				INSERT INTO #TMP_GROUPS(REP_DATE, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER)
					SELECT REP_DATE, CLG.cms_location_group_id, CLG.cms_location_group AS 'location', SUM(TOPIC_COUNTER) AS 'TOPIC_COUNTER', SUM(GROUP_COUNTER) AS 'GROUP_COUNTER' 
					FROM #TMP_LOCATIONS
					LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
					LEFT JOIN CMS_LOCATION_GROUPS AS CLG ON CLG.cms_location_group_id = L.cms_location_group_id						
					GROUP BY REP_DATE, CLG.cms_location_group_id, CLG.cms_location_group
					ORDER BY CLG.cms_location_group
			END 
			ELSE
			BEGIN				
				IF (@grouping_criteria = 'LOCATION')
				BEGIN
					INSERT INTO #TMP_GROUPS(REP_DATE, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER)
						SELECT REP_DATE, #TMP_LOCATIONS.LOCATION, #TMP_LOCATIONS.LOCATION AS 'location', SUM(TOPIC_COUNTER) AS 'TOPIC_COUNTER', SUM(GROUP_COUNTER) AS 'GROUP_COUNTER' 
						FROM #TMP_LOCATIONS	
						GROUP BY REP_DATE, #TMP_LOCATIONS.LOCATION
						ORDER BY #TMP_LOCATIONS.LOCATION										
				END 
				ELSE
				BEGIN
					IF (@grouping_criteria = 'OPS_REGION')
					BEGIN
						INSERT INTO #TMP_GROUPS(REP_DATE, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER)
							SELECT REP_DATE, ORE.ops_region_id, ORE.ops_region AS 'location', SUM(TOPIC_COUNTER) AS 'TOPIC_COUNTER', SUM(GROUP_COUNTER) AS 'GROUP_COUNTER' 
							FROM #TMP_LOCATIONS
							LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
							LEFT JOIN OPS_AREAS AS OA ON OA.ops_area_id = L.ops_area_id
							LEFT JOIN OPS_Regions AS ORE ON ORE.ops_region_id = OA.ops_region_id AND ORE.country = #TMP_LOCATIONS.COUNTRY	
							GROUP BY REP_DATE, ORE.ops_region_id, ORE.ops_region
							ORDER BY ORE.ops_region							
						
					END 
					ELSE
					BEGIN
						IF (@grouping_criteria = 'OPS_AREA')
						BEGIN
							INSERT INTO #TMP_GROUPS(REP_DATE, GROUP_ID, GROUP_NAME, TOPIC_COUNTER, GROUP_COUNTER)
								SELECT REP_DATE, OA.ops_area_id, OA.ops_area AS 'location', SUM(TOPIC_COUNTER) AS 'TOPIC_COUNTER', SUM(GROUP_COUNTER) AS 'GROUP_COUNTER' 
								FROM #TMP_LOCATIONS
								LEFT JOIN LOCATIONS AS L ON L.location = #TMP_LOCATIONS.location
								LEFT JOIN OPS_AREAS AS OA ON OA.ops_area_id = L.ops_area_id								
								GROUP BY REP_DATE, OA.ops_area_id, OA.ops_area
								ORDER BY OA.ops_area
						END 
					END
				END
			END					
		END
	END

	-- DISPLAY NUMBERS OF CARS
	IF (@select_by = 'VALUE')
	BEGIN		
		SELECT GROUP_NAME AS 'location', AVG(TOPIC_COUNTER) AS 'car_count'
		FROM #TMP_GROUPS
		GROUP BY GROUP_ID, GROUP_NAME
		ORDER BY GROUP_NAME
	END
	-- Display percentage of cars in the TOPIC
	ELSE IF (@select_by = 'PERCENTAGE')
	BEGIN	
		SELECT GROUP_NAME AS 'location', CASE WHEN (AVG(GROUP_COUNTER) = 0) THEN 0 ELSE (AVG(TOPIC_COUNTER)*100.0)/AVG(GROUP_COUNTER) END AS 'car_count' 
		FROM #TMP_GROUPS
		GROUP BY GROUP_ID, GROUP_NAME
		ORDER BY GROUP_NAME
	END
	
	-- Drop the tempory table	
	DROP TABLE #TMP_GROUPS
	-- Drop the tempory table	
	DROP TABLE #TMP_LOCATIONS

END