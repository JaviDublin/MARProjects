-- =============================================
-- Author:		Javier
-- Create date: December 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[NonRev_Review_DailyReport]

		@days_num INT = NULL

AS
BEGIN

	SET NOCOUNT ON;
	
	IF @days_num IS NULL SET @days_num = 0
	
	DECLARE @current_date DATETIME = DATEADD(D,0,DATEDIFF(D,0,GETDATE() - @days_num))
	
	
	 --Check if the current date exists in the table
	
	IF NOT EXISTS 
		(	SELECT	TOP 1 StatId 
			FROM	[General].[Fact_NonRevLog_DailyReport] 
			WHERE	Rep_Date = @current_date )
	BEGIN
	
	
		;WITH Fleet AS (

			SELECT 
				COUNTRY		AS [Country], 
				LSTWWD		AS [LocationCode], 
				VC			AS [CarGroup], 
				OPERSTAT	AS [OperStat] ,
ISNULL(SUM(CASE WHEN (FLEET_RAC_TTL > 0 or FLEET_CARSALES > 0) AND TOTAL_FLEET = 1 THEN TOTAL_FLEET ELSE NULL END),0) AS [Total Fleet],
ISNULL(SUM(CASE WHEN FLEET_ADV = 1		AND TOTAL_FLEET = 1 THEN TOTAL_FLEET ELSE NULL END),0) AS [Total Fleet Adv],
ISNULL(SUM(CASE WHEN FLEET_HOD = 1		AND TOTAL_FLEET = 1 THEN TOTAL_FLEET ELSE NULL END),0) AS [Total Fleet Hod],
ISNULL(SUM(CASE WHEN FLEET_LICENSEE = 1 AND TOTAL_FLEET = 1 THEN TOTAL_FLEET ELSE NULL END),0) AS [Total Fleet Licensee],
ISNULL(SUM(CASE WHEN FLEET_CARSALES = 1 AND TOTAL_FLEET = 1 THEN TOTAL_FLEET ELSE NULL END),0) AS [Total Fleet Carsales],
ISNULL(SUM(CASE WHEN FLEET_RAC_TTL = 1	AND TOTAL_FLEET = 1 THEN TOTAL_FLEET ELSE NULL END),0) AS [Total Fleet Rac Ttl],
ISNULL(SUM(CASE WHEN FLEET_RAC_OPS = 1	AND TOTAL_FLEET = 1 THEN TOTAL_FLEET ELSE NULL END),0) AS [Total Fleet Rac Ops],
ISNULL(SUM(CASE WHEN (FLEET_RAC_TTL > 0 or FLEET_CARSALES > 0) and TOTAL_FLEET = 1 THEN OPERATIONAL_FLEET ELSE NULL END),0) AS [Operational Fleet],
ISNULL(SUM(CASE WHEN FLEET_ADV = 1		AND TOTAL_FLEET = 1 THEN OPERATIONAL_FLEET ELSE NULL END),0) AS [Operational Fleet Adv],
ISNULL(SUM(CASE WHEN FLEET_HOD = 1		AND TOTAL_FLEET = 1 THEN OPERATIONAL_FLEET ELSE NULL END),0) AS [Operational Fleet Hod],
ISNULL(SUM(CASE WHEN FLEET_LICENSEE = 1 AND TOTAL_FLEET = 1 THEN OPERATIONAL_FLEET ELSE NULL END),0) AS [Operational Fleet Licensee],
ISNULL(SUM(CASE WHEN FLEET_CARSALES = 1 AND TOTAL_FLEET = 1 THEN OPERATIONAL_FLEET ELSE NULL END),0) AS [Operational Fleet Carsales],
ISNULL(SUM(CASE WHEN FLEET_RAC_TTL = 1	AND TOTAL_FLEET = 1 THEN OPERATIONAL_FLEET ELSE NULL END),0) AS [Operational Fleet Rac Ttl],
ISNULL(SUM(CASE WHEN FLEET_RAC_OPS = 1	AND TOTAL_FLEET = 1 THEN OPERATIONAL_FLEET ELSE NULL END),0) AS [Operational Fleet Rac Ops]
			FROM 
				[dbo].FLEET_EUROPE_ACTUAL 
			GROUP BY 
				COUNTRY , LSTWWD , VC , OPERSTAT 
		 )
		 INSERT INTO [General].[Fleet_Actual_Summary]
		 (
			Country , LocationCode , CarGroup , OperStat , TotalFleet , TotalFleet_adv ,
			TotalFleet_hod , TotalFleet_licensee , TotalFleet_carsales , TotalFleet_rac_ttl ,
			TotalFleet_rac_ops ,  OperationalFleet , OperationalFleet_adv , OperationalFleet_hod ,
			OperationalFleet_licensee , OperationalFleet_carsales , OperationalFleet_rac_ttl ,
			OperationalFleet_rac_ops ,  Rep_Date
		)
		 SELECT 
			COUNTRY AS [Country], 
			LocationCode , 
			CarGroup , 
			OperStat ,
			 SUM([Total Fleet])					AS [Total Fleet],
			 SUM([Total Fleet Adv])				AS [Total Fleet Adv],
			 SUM([Total Fleet Hod])				AS [Total Fleet Hod],
			 SUM([Total Fleet Licensee])		AS [Total Fleet Licensee],
			 SUM([Total Fleet Carsales])		AS [Total Fleet Carsales],
			 SUM([Total Fleet Rac Ttl])			AS [Total Fleet Rac Ttl],
			 SUM([Total Fleet Rac Ops])			AS [Total Fleet Rac Ops],
			 SUM([Operational Fleet])			AS [Operational Fleet],
			 SUM([Operational Fleet Adv])		AS [Operational Fleet Adv],
			 SUM([Operational Fleet Hod])		AS [Operational Fleet Hod],
			 SUM([Operational Fleet Licensee])	AS [Operational Fleet Licensee],
			 SUM([Operational Fleet Carsales])	AS [Operational Fleet Carsales],
			 SUM([Operational Fleet Rac Ttl])	AS [Operational Fleet Rac Ttl],
			 SUM([Operational Fleet Rac Ops])	AS [Operational Fleet Rac Ops],
			 @current_date
		 FROM 
			Fleet 
		 GROUP BY 
			COUNTRY ,LocationCode , CarGroup , OperStat  
		 ORDER BY 
			COUNTRY
	
		SET ANSI_WARNINGS OFF
		INSERT INTO [General].[Fact_NonRevLog_DailyReport]
		( 
			Rep_Date , CountryCar , LocationCode , OperStat , CarGroup , DayGroupCode , TotalFleetNR ,
			TotalFleetNR_RacTtl , TotalFleetNR_RacOps , TotalFleetNR_CarSales , TotalFleetNR_Licensee , 
			TotalFleetNR_Adv ,  TotalFleetNR_Hod
		)
		SELECT
			LastDailyFileDate ,
			Country , Lstwwd , OperStat , CarGroup , DayGroupCode , 
			COUNT(*) ,
			COUNT(CASE WHEN Fleet_rac_ttl   = 1 THEN 1 ELSE NULL END),
			COUNT(CASE WHEN Fleet_rac_ops	= 1 THEN 1 ELSE NULL END),
			COUNT(CASE WHEN Fleet_carsales	= 1 THEN 1 ELSE NULL END),
			COUNT(CASE WHEN Fleet_licensee	= 1 THEN 1 ELSE NULL END),
			COUNT(CASE WHEN Fleet_adv		= 1 THEN 1 ELSE NULL END),
			COUNT(CASE WHEN Fleet_hod		= 1 THEN 1 ELSE NULL END)
		FROM 
			[General].Staging_Fleet
		GROUP BY 
			LastDailyFileDate ,
			Country , Lstwwd , OperStat , CarGroup , DayGroupCode 
		ORDER BY 
			LastDailyFileDate ,
			Country , Lstwwd , OperStat , CarGroup , DayGroupCode 
		SET ANSI_WARNINGS ON
		
		UPDATE [General].[Fact_NonRevLog_DailyReport] SET
			LocationId = l.dim_Location_id
		FROM [General].Fact_NonRevLog_DailyReport dr
		INNER JOIN LOCATIONS l ON dr.LocationCode = l.location
		WHERE 
			dr.LocationId IS NULL
			
		UPDATE [General].[Fleet_Actual_Summary] SET
			LocationId = l.dim_Location_id
		FROM [General].[Fleet_Actual_Summary] dr
		INNER JOIN LOCATIONS l ON dr.LocationCode = l.location
		WHERE 
			dr.LocationId IS NULL
			
		UPDATE [General].Fact_NonRevLog_DailyReport SET
			CountryLoc = l.country
		FROM [General].Fact_NonRevLog_DailyReport dr
		INNER JOIN LOCATIONS l ON dr.LocationId = l.dim_Location_id
		WHERE 
			dr.CountryLoc IS NULL

		UPDATE [General].[Fact_NonRevLog_DailyReport] SET
			dim_calendar_id = c.dim_Calendar_id ,
			Rep_Year		= c.Rep_Year,
			Rep_Month		= c.Rep_Month , 
			Rep_WeekOfYear	= c.Rep_WeekOfYear , 
			Rep_DayOfWeek	= c.Rep_DayOfWeek
		FROM [General].[Fact_NonRevLog_DailyReport] dr
		INNER JOIN [Inp].dim_Calendar c ON dr.Rep_Date = c.Rep_Date
		WHERE dr.dim_calendar_id IS NULL
		
		UPDATE [General].[Fleet_Actual_Summary] SET
			dim_calendar_id = c.dim_Calendar_id ,
			Rep_Year		= c.Rep_Year,
			Rep_Month		= c.Rep_Month , 
			Rep_WeekOfYear	= c.Rep_WeekOfYear , 
			Rep_DayOfWeek	= c.Rep_DayOfWeek
		FROM [General].[Fleet_Actual_Summary] dr
		INNER JOIN [Inp].dim_Calendar c ON dr.Rep_Date = c.Rep_Date
		WHERE dr.dim_calendar_id IS NULL
		
		UPDATE [General].[Fact_NonRevLog_DailyReport] SET
			Cms_Group_Id = cms.cms_location_group_id ,
			Cms_Group	 = cms.cms_location_group ,
			Cms_Pool_Id  = cms.cms_pool_id ,
			Cms_Pool	 = cms.cms_pool
		FROM [General].[Fact_NonRevLog_DailyReport] dr
		INNER JOIN [Settings].vw_Locations_CMS cms ON dr.LocationId = cms.dim_Location_id
		WHERE dr.Cms_Group IS NULL
		
		UPDATE [General].[Fact_NonRevLog_DailyReport] SET
			Ops_Area_Id		= ops.ops_area_id ,
			Ops_Area		= ops.ops_area ,
			Ops_Region_Id	= ops.ops_region_id ,
			Ops_Region		= ops.ops_region
		FROM [General].[Fact_NonRevLog_DailyReport] dr
		INNER JOIN [Settings].vw_Locations_OPS ops ON dr.LocationId = ops.dim_Location_id
		WHERE dr.Ops_Area IS NULL
		
		UPDATE [General].[Fact_NonRevLog_DailyReport] SET
			CarGroupId		= cc.car_group_id ,
			Car_Segment_Id	= cc.car_segment_id ,
			Car_Segment		= cc.car_segment,
			Car_Class		= cc.car_class,
			Car_Class_Id	= cc.car_class_id
		FROM [General].[Fact_NonRevLog_DailyReport] dr
		INNER JOIN [Settings].vw_CarGroups cc ON dr.CarGroup = cc.car_group AND dr.CountryCar = cc.country
		WHERE dr.CarGroupId IS NULL
		
		UPDATE [General].[Fleet_Actual_Summary] SET
			Cms_Group_Id = cms.cms_location_group_id ,
			Cms_Pool_Id  = cms.cms_pool_id 
		FROM [General].[Fleet_Actual_Summary] dr
		INNER JOIN [Settings].vw_Locations_CMS cms ON dr.LocationId = cms.dim_Location_id
		WHERE dr.Cms_Group_Id IS NULL

		UPDATE [General].[Fleet_Actual_Summary] SET
			Ops_Area_Id		= ops.ops_area_id ,
			Ops_Region_Id	= ops.ops_region_id 
		FROM [General].[Fleet_Actual_Summary] dr
		INNER JOIN [Settings].vw_Locations_OPS ops ON dr.LocationId = ops.dim_Location_id
		WHERE dr.Ops_Area_Id IS NULL
		
		UPDATE [General].[Fleet_Actual_Summary] SET
			Car_Segment_Id	= cc.car_segment_id ,
			Car_Class_Id	= cc.car_class_id
		FROM [General].[Fleet_Actual_Summary] dr
		INNER JOIN [Settings].vw_CarGroups cc ON dr.CarGroup = cc.car_group AND dr.Country = cc.country
		WHERE dr.Car_Class_Id IS NULL
		
		UPDATE [General].[Fleet_Actual_Summary] SET
			CarGroupId		= cc.car_group_id 
		FROM [General].[Fleet_Actual_Summary] dr
		INNER JOIN [Settings].vw_CarGroups cc ON dr.CarGroup = cc.car_group AND dr.Country = cc.country
		WHERE dr.CarGroupId IS NULL
		
		UPDATE [General].[Fact_NonRevLog_DailyReport] SET
			TotalFleetNR_Remark = ISNULL(r.Total,0)
		FROM 
			[General].[Fact_NonRevLog_DailyReport] dr
		LEFT JOIN
			(
			SELECT 
				nrl.Lstwwd , nrl.OperStat , nrl.DayGroupCode , f.CarGroup ,nrl.RemarkIdDate , 
				COUNT(*) as [Total]
			FROM [General].Fact_NonRevLog nrl
			INNER JOIN [General].Dim_Fleet f on nrl.VehicleId = f.VehicleId
			WHERE 
				nrl.RemarkId > 1
				
				and nrl.ERDate >= DATEADD(D,0,DATEDIFF(D,0,GETDATE()))  -- Change done by Javi 21/03/2013
				
			GROUP BY 
				nrl.Lstwwd , nrl.OperStat , nrl.DayGroupCode , f.CarGroup ,nrl.RemarkIdDate 
			) r ON 
				dr.LocationCode = r.Lstwwd 
			AND dr.OperStat		= r.OperStat 
			AND dr.DayGroupCode = r.DayGroupCode 
			AND dr.CarGroup		= r.CarGroup 
			AND dr.Rep_Date		= r.RemarkIdDate
		
		UPDATE [General].[Fact_NonRevLog_DailyReport] SET
			rns  = t.rns
		FROM [General].Fact_NonRevLog_DailyReport r
		INNER JOIN 
		(
			SELECT 
				Rep_Date , CountryCar , CountryLoc , LocationCode , 
				CarGroup , OperStat , DayGroupCode ,
				ROW_NUMBER() OVER (PARTITION BY 
					Rep_Date , CountryCar , CountryLoc , LocationCode , CarGroup , OperStat
				ORDER BY (SELECT 0)) AS [rns]
			FROM 
				[General].Fact_NonRevLog_DailyReport 
			GROUP BY 
				Rep_Date , CountryCar , CountryLoc , LocationCode , CarGroup , OperStat , DayGroupCode
		) t
			on 
				r.Rep_Date			= t.Rep_Date 
			AND r.CountryCar		= t.CountryCar 
			AND r.CountryLoc		= t.CountryLoc 
			AND r.LocationCode		= t.LocationCode 
			AND r.OperStat			= t.OperStat 
			AND r.CarGroup			= t.CarGroup
			AND r.DayGroupCode		= t.DayGroupCode
		WHERE 
			r.rns IS NULL
				
		UPDATE [General].Fact_NonRevLog_DailyReport SET
			CountryLoc = SUBSTRING(LocationCode,1,2) 
		WHERE CountryLoc IS NULL

/*
		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet					= s.TotalFleet ,
			TotalFleet_CarSales			= s.TotalFleet_carsales ,
			TotalFleet_Licensee			= s.TotalFleet_licensee ,
			TotalFleet_RacOps			= s.TotalFleet_rac_ops ,
			TotalFleet_RacTtl			= s.TotalFleet_rac_ttl ,
			TotalFleet_Adv				= s.TotalFleet_adv ,
			TotalFleet_Hod				= s.TotalFleet_hod ,
			OperationalFleet			= s.OperationalFleet ,
			OperationalFleet_CarSales	= s.OperationalFleet_carsales ,
			OperationalFleet_Licensee	= s.OperationalFleet_licensee ,
			OperationalFleet_RacOps		= s.OperationalFleet_rac_ops ,
			OperationalFleet_RacTtl		= s.OperationalFleet_rac_ttl ,
			OperationalFleet_Adv		= s.OperationalFleet_adv ,
			OperationalFleet_Hod		= s.OperationalFleet_hod
		FROM [General].Fact_NonRevLog_DailyReport dr
		INNER JOIN [General].Fleet_Actual_Summary s ON
			dr.CarGroupId = s.CarGroupId
		AND
			dr.LocationId = s.LocationId
		AND 
			dr.dim_calendar_id = s.dim_calendar_id
		AND 
			dr.OperStat = s.OperStat
		WHERE
			dr.rns = 1
			and dr.TotalFleet IS NULL
		*/
		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet					= s.TotalFleet ,
			TotalFleet_CarSales			= s.TotalFleet_carsales ,
			TotalFleet_Licensee			= s.TotalFleet_licensee ,
			TotalFleet_RacOps			= s.TotalFleet_rac_ops ,
			TotalFleet_RacTtl			= s.TotalFleet_rac_ttl ,
			TotalFleet_Adv				= s.TotalFleet_adv ,
			TotalFleet_Hod				= s.TotalFleet_hod ,
			OperationalFleet			= s.OperationalFleet ,
			OperationalFleet_CarSales	= s.OperationalFleet_carsales ,
			OperationalFleet_Licensee	= s.OperationalFleet_licensee ,
			OperationalFleet_RacOps		= s.OperationalFleet_rac_ops ,
			OperationalFleet_RacTtl		= s.OperationalFleet_rac_ttl ,
			OperationalFleet_Adv		= s.OperationalFleet_adv ,
			OperationalFleet_Hod		= s.OperationalFleet_hod
		FROM [General].Fleet_Actual_Summary s 
		WHERE
			[General].Fact_NonRevLog_DailyReport.rns = 1
			and [General].Fact_NonRevLog_DailyReport.TotalFleet IS NULL
			and	[General].Fact_NonRevLog_DailyReport.CarGroupId = s.CarGroupId
					AND
						[General].Fact_NonRevLog_DailyReport.LocationId = s.LocationId
					AND 
						[General].Fact_NonRevLog_DailyReport.dim_calendar_id = s.dim_calendar_id
					AND 
						[General].Fact_NonRevLog_DailyReport.OperStat = s.OperStat

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet = 0
		WHERE TotalFleet IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet_CarSales			= 0
		WHERE TotalFleet_CarSales IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet_Licensee			= 0
		WHERE TotalFleet_Licensee IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet_RacOps			= 0
		WHERE TotalFleet_RacOps IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet_RacTtl			= 0
		WHERE TotalFleet_RacTtl IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet_Adv				= 0
		WHERE TotalFleet_Adv IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			TotalFleet_Hod				= 0
		WHERE TotalFleet_Hod IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			OperationalFleet			= 0
		WHERE OperationalFleet IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			OperationalFleet_CarSales	= 0
		WHERE OperationalFleet_CarSales IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			OperationalFleet_Licensee	= 0
		WHERE OperationalFleet_Licensee IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			OperationalFleet_RacOps		= 0
		WHERE OperationalFleet_RacOps IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			OperationalFleet_RacTtl		= 0
		WHERE OperationalFleet_RacTtl IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			OperationalFleet_Adv		= 0
		WHERE OperationalFleet_Adv IS NULL

		UPDATE [General].Fact_NonRevLog_DailyReport SET
			OperationalFleet_Hod		= 0
		WHERE OperationalFleet_Hod IS NULL
		
		UPDATE [General].Fleet_Actual_Summary SET
			Cms_Group	 = cms.cms_location_group ,
			Cms_Pool	 = cms.cms_pool
		FROM [General].Fleet_Actual_Summary dr
		INNER JOIN [Settings].vw_Locations_CMS cms ON dr.LocationId = cms.dim_Location_id
		WHERE dr.Cms_Group IS NULL
		
		UPDATE [General].Fleet_Actual_Summary SET
			Ops_Area		= ops.ops_area ,
			Ops_Region		= ops.ops_region
		FROM [General].Fleet_Actual_Summary dr
		INNER JOIN [Settings].vw_Locations_OPS ops ON dr.LocationId = ops.dim_Location_id
		WHERE dr.Ops_Area IS NULL
		
		UPDATE [General].Fleet_Actual_Summary SET
			Car_Segment		= cc.car_segment,
			Car_Class		= cc.car_class
		FROM [General].Fleet_Actual_Summary dr
		INNER JOIN [Settings].vw_CarGroups cc ON dr.CarGroup = cc.car_group AND dr.Country = cc.country
		WHERE dr.Car_Segment IS NULL
		
				
		
	END

END