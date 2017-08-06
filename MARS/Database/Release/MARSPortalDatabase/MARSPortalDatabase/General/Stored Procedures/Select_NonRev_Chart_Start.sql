-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Chart_Start]

	@groupby				VARCHAR(20)		= NULL,
	@country				VARCHAR(10)		= NULL,
	@cms_pool_id			INT				= NULL,
	@cms_location_group_id	INT				= NULL, 
	@ops_region_id			INT				= NULL,
	@ops_area_id			INT				= NULL,	
	@location				VARCHAR(50)		= NULL,
	@fleet_name				VARCHAR(50)		= NULL,
	@car_segment_id			INT				= NULL,
	@car_class_id			INT				= NULL,
	@car_group_id			INT				= NULL,
	@daygroupcode			VARCHAR(10)		= NULL	
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @totaFleet INT , @totalFleetNonRev INT
	
	SET @totaFleet = (SELECT COUNT(*) FROM FLEET_EUROPE_ACTUAL)
	
	SET @totalFleetNonRev = 
		(
			SELECT 
				COUNT(*) 
			FROM 
				[General].vw_Fleet_NonRevLog_Report FNRL
			
			WHERE 
				FNRL.IsOpen = 1
			AND ((@country IS NULL) OR (FNRL.CountryCar = @country))		
			AND ((FNRL.cms_pool_id				= @cms_pool_id)				OR @cms_pool_id				IS NULL) -- CMS_POOLS
			AND ((FNRL.cms_location_group_id	= @cms_location_group_id)	OR @cms_location_group_id	IS NULL) -- CMS_LOCATION_GROUPS
			AND ((FNRL.ops_region_id			= @ops_region_id)			OR @ops_region_id			IS NULL) -- OPS_REGIONS
			AND ((FNRL.ops_area_id				= @ops_area_id)				OR @ops_area_id				IS NULL) -- OPS_AREAS
			AND (FNRL.Lstwwd					= @location					OR @location				IS NULL) 
			AND ((FNRL.car_segment_id			= @car_segment_id)			OR @car_segment_id			IS NULL) --@car_segment_id
			AND ((FNRL.car_class_id				= @car_class_id)			OR @car_class_id			IS NULL) --@car_class_id
			AND ((FNRL.car_group_id				= @car_group_id)			OR @car_group_id			IS NULL) --@car_group_id
			AND (	(	@fleet_name = 'CARSALES'
						AND FNRL.Fleet_carsales > 0
						)
					OR	(	@fleet_name = 'RAC OPS'
						AND FNRL.Fleet_rac_ops > 0
						)
					OR	(	@fleet_name = 'RAC TTL'
						AND FNRL.Fleet_rac_ttl > 0
						)
					OR	(	@fleet_name = 'ADVANTAGE'
						AND FNRL.Fleet_adv > 0
						)
					OR	(	@fleet_name = 'HERTZ ON DEMAND'
						AND FNRL.Fleet_hod > 0
						)
					OR	(	@fleet_name = 'LICENSEE'
						AND FNRL.Fleet_licensee > 0
						)	
					OR	(	@fleet_name IS NULL
						AND	(	FNRL.Fleet_carsales > 0
							OR FNRL.Fleet_rac_ttl > 0
							)
						)
					)--Fleet
		)
		
	
	DECLARE @TABLE_START TABLE 
	(
		rowId				INT IDENTITY (1,1) NOT NULL,
		GroupCode			VARCHAR(10) , TotalVehicles INT , PctNonRev NUMERIC(5,2) ,
		TotalFleetNonRev	INT			, PctTotal NUMERIC(5,2), TotalFleet INT
	)
	
	select * from @TABLE_START
	return;
	
	IF @groupby IS NULL or @groupby = 'KCI'
	BEGIN
		INSERT INTO @TABLE_START
		(GroupCode , TotalVehicles , PctNonRev , TotalFleetNonRev , PctTotal , TotalFleet)
		SELECT
			FNRL.KCICode , 
			COUNT(*) AS [TotalVehicles] , 
			CONVERT(NUMERIC(5,2),
				ROUND(
					CONVERT(NUMERIC(20,2),(COUNT(*) * 100))
					/
					CONVERT(NUMERIC(20,2), @totalFleetNonRev),2)) 
					AS [PCT_NonRev] , 
			@totalFleetNonRev as [TotalFleetNonRev],
			
			CONVERT(NUMERIC(5,2),
				ROUND(
					CONVERT(NUMERIC(20,2),(COUNT(*) * 100))
					/
					CONVERT(NUMERIC(20,2), @totaFleet),2)) 
					AS [PCT_Total] , 
			
			@totaFleet as [TotalFleet]
			
		FROM 
				[General].vw_Fleet_NonRevLog_Report FNRL
			
		WHERE 
			FNRL.IsOpen = 1
		AND ((@country IS NULL) OR (FNRL.CountryCar = @country))	
		--AND ((@daygroupcode IS NULL) OR (FNRL.DayGroupCode = @daygroupcode))	
		
		AND ((@daygroupcode IS NULL) OR (FNRL.DayGroupCode IN (select Items from dbo.fSplit(@daygroupcode,','))))	
		AND ((FNRL.cms_pool_id				= @cms_pool_id)				OR @cms_pool_id				IS NULL) -- CMS_POOLS
		AND ((FNRL.cms_location_group_id	= @cms_location_group_id)	OR @cms_location_group_id	IS NULL) -- CMS_LOCATION_GROUPS
		AND ((FNRL.ops_region_id			= @ops_region_id)			OR @ops_region_id			IS NULL) -- OPS_REGIONS
		AND ((FNRL.ops_area_id				= @ops_area_id)				OR @ops_area_id				IS NULL) -- OPS_AREAS
		AND (FNRL.Lstwwd					= @location					OR @location				IS NULL) 
		AND ((FNRL.car_segment_id			= @car_segment_id)			OR @car_segment_id			IS NULL) --@car_segment_id
		AND ((FNRL.car_class_id				= @car_class_id)			OR @car_class_id			IS NULL) --@car_class_id
		AND ((FNRL.car_group_id				= @car_group_id)			OR @car_group_id			IS NULL) --@car_group_id
		AND (	(	@fleet_name = 'CARSALES'
						AND FNRL.Fleet_carsales > 0
						)
					OR	(	@fleet_name = 'RAC OPS'
						AND FNRL.Fleet_rac_ops > 0
						)
					OR	(	@fleet_name = 'RAC TTL'
						AND FNRL.Fleet_rac_ttl > 0
						)
					OR	(	@fleet_name = 'ADVANTAGE'
						AND FNRL.Fleet_adv > 0
						)
					OR	(	@fleet_name = 'HERTZ ON DEMAND'
						AND FNRL.Fleet_hod > 0
						)
					OR	(	@fleet_name = 'LICENSEE'
						AND FNRL.Fleet_licensee > 0
						)	
					OR	(	@fleet_name IS NULL
						AND	(	FNRL.Fleet_carsales > 0
							OR FNRL.Fleet_rac_ttl > 0
							)
						)
					)--Fleet
		GROUP BY
			FNRL.KCICode	
		ORDER BY COUNT(*) DESC
		
		SELECT 
			CS.GroupCode , CS.TotalVehicles , CS.PctNonRev , CS.PctTotal 
		FROM 
			@TABLE_START CS
		ORDER BY CS.rowId
	END
	ELSE IF @groupby = 'STAT'
	BEGIN
		INSERT INTO @TABLE_START
		(GroupCode , TotalVehicles , PctNonRev , TotalFleetNonRev , PctTotal , TotalFleet)
		SELECT
			FNRL.OperStat , 
			COUNT(*) AS [TotalVehicles] , 
			CONVERT(NUMERIC(5,2),
				ROUND(
					CONVERT(NUMERIC(20,2),(COUNT(*) * 100))
					/
					CONVERT(NUMERIC(20,2), @totalFleetNonRev),2)) 
					AS [PCT_NonRev] , 
			@totalFleetNonRev as [TotalFleetNonRev],
			
			CONVERT(NUMERIC(5,2),
				ROUND(
					CONVERT(NUMERIC(20,2),(COUNT(*) * 100))
					/
					CONVERT(NUMERIC(20,2), @totaFleet),2)) 
					AS [PCT_Total] , 
			
			@totaFleet as [TotalFleet]
			
		FROM 
				[General].vw_Fleet_NonRevLog_Report FNRL
			
		WHERE 
			FNRL.IsOpen = 1
		AND ((@country IS NULL) OR (FNRL.CountryCar = @country))	
		AND ((@daygroupcode IS NULL) OR (FNRL.DayGroupCode IN (select Items from dbo.fSplit(@daygroupcode,','))))
		AND ((FNRL.cms_pool_id				= @cms_pool_id)				OR @cms_pool_id				IS NULL) -- CMS_POOLS
		AND ((FNRL.cms_location_group_id	= @cms_location_group_id)	OR @cms_location_group_id	IS NULL) -- CMS_LOCATION_GROUPS
		AND ((FNRL.ops_region_id			= @ops_region_id)			OR @ops_region_id			IS NULL) -- OPS_REGIONS
		AND ((FNRL.ops_area_id				= @ops_area_id)				OR @ops_area_id				IS NULL) -- OPS_AREAS
		AND (FNRL.Lstwwd					= @location					OR @location				IS NULL) 
		AND ((FNRL.car_segment_id			= @car_segment_id)			OR @car_segment_id			IS NULL) --@car_segment_id
		AND ((FNRL.car_class_id				= @car_class_id)			OR @car_class_id			IS NULL) --@car_class_id
		AND ((FNRL.car_group_id				= @car_group_id)			OR @car_group_id			IS NULL) --@car_group_id
		AND (	(	@fleet_name = 'CARSALES'
						AND FNRL.Fleet_carsales > 0
						)
					OR	(	@fleet_name = 'RAC OPS'
						AND FNRL.Fleet_rac_ops > 0
						)
					OR	(	@fleet_name = 'RAC TTL'
						AND FNRL.Fleet_rac_ttl > 0
						)
					OR	(	@fleet_name = 'ADVANTAGE'
						AND FNRL.Fleet_adv > 0
						)
					OR	(	@fleet_name = 'HERTZ ON DEMAND'
						AND FNRL.Fleet_hod > 0
						)
					OR	(	@fleet_name = 'LICENSEE'
						AND FNRL.Fleet_licensee > 0
						)	
					OR	(	@fleet_name IS NULL
						AND	(	FNRL.Fleet_carsales > 0
							OR FNRL.Fleet_rac_ttl > 0
							)
						)
					)--Fleet
		GROUP BY
			FNRL.OperStat
		ORDER BY COUNT(*) DESC
		
		SELECT TOP 5
			CS.GroupCode , CS.TotalVehicles , CS.PctNonRev , CS.PctTotal 
		FROM 
			@TABLE_START CS
		
		UNION
		
		SELECT 
			'OTHER' , sum(CS.TotalVehicles) , sum(CS.PctNonRev) , sum(CS.PctTotal) 
		FROM 
			@TABLE_START CS
		WHERE cs.rowId > 5
		
		ORDER BY CS.TotalVehicles  DESC
	END
	
    
END