-- =============================================
-- Author:		Javier
-- Create date: July 2012
-- Description:	Get the Area Codes that contains more Locations in Europe
-- =============================================

CREATE procedure [dbo].[spOwnareaSelect] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @area_code TABLE (ownarea VARCHAR(50) , TotalLocations INT , CountryName VARCHAR(50))
	
	INSERT INTO @area_code
	SELECT OWNAREA , COUNT(*) , COUNTRY
	FROM FLEET_EUROPE_ACTUAL 
	GROUP BY OWNAREA , COUNTRY
	ORDER BY COUNT(*) DESC
	
	SELECT ownarea FROM @area_code where CountryName in ('IT','GE','UK','FR','BE','SP', 'LU', 'NE')
	
--===================================================================================================
--===================================================================================================
	--DECLARE @area_code TABLE (ownarea VARCHAR(50) , TotalLocations INT , CountryName VARCHAR(50))
	
	--;WITH Locs AS
	--(
	--	SELECT 
	--		loc.country AS [Country], cou.country_description AS [Country_Name],
	--		loc.location AS [Location_Code], loc.location_name AS [Location_Name], 
	--		loc.active AS [IsActive], loc.ap_dt_rr AS [Location_Type], loc.ownarea AS [Ownarea_Code] , 
	--		clg.cms_location_group_id AS [Location_Group_Id], clg.cms_location_group AS [Location_Group] ,
	--		cmp.cms_pool_id AS [Pool_Id] , cmp.cms_pool AS [Pool_Name],
	--		opa.ops_area_id AS [Area_Id] , opa.ops_area AS [Area],
	--		opr.ops_region_id AS [Region_Id] , opr.ops_region AS [Region]
	--	FROM 
	--		LOCATIONS loc
	--	JOIN COUNTRIES			 AS cou ON loc.country					= cou.country
	--	JOIN CMS_LOCATION_GROUPS AS clg ON loc.cms_location_group_id	= clg.cms_location_group_id
	--	JOIN CMS_POOLS			 AS cmp ON clg.cms_pool_id				= cmp.cms_pool_id 
	--	JOIN OPS_AREAS			 AS opa ON loc.ops_area_id				= opa.ops_area_id
	--	JOIN OPS_REGIONS		 AS opr ON opa.ops_region_id			= opr.ops_region_id
	--)
	--INSERT INTO @area_code
	--SELECT 
	--	Ownarea_Code as [Area_Code], COUNT(*) AS [Total_Locations] , Country_Name
	--FROM 
	--	Locs 
	--WHERE 
	--	Pool_Name <> 'Unmapped'
	--GROUP BY 
	--	Ownarea_Code , Country_Name
	--ORDER BY 
	--	COUNT(*) desc

	--SELECT TOP 50  ownarea AS [ownarea] FROM @area_code

	--SELECT TOP 50  ownarea , CountryName , TotalLocations FROM @area_code
	
--===================================================================================================
--===================================================================================================

	--SELECT
	--	'00000'  AS [ownarea] 
	--	--ownarea AS [ownarea] 
	--FROM 
	--	AREACODES 
	--ORDER BY 
	--	ownarea
--===================================================================================================
--===================================================================================================
	
	
END