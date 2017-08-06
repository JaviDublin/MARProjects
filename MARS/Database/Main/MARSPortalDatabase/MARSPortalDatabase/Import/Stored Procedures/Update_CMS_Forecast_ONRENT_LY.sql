-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Update_CMS_Forecast_ONRENT_LY]
	
AS
BEGIN
	
	SET NOCOUNT ON;

    --UPDATE ON_RENT_LY in CMS_FORECAST table
	DECLARE @REP_DATE_LY_MIN datetime, @REP_DATE_LY_MAX datetime

	--SET @REP_DATE_LY_MIN = (SELECT DATEADD(d, -365, MIN(REP_DATE)) FROM dbo.MARS_CMS_FORECAST)
	--SET @REP_DATE_LY_MAX = (SELECT DATEADD(d, -365, MAX(REP_DATE)) FROM dbo.MARS_CMS_FORECAST)
	SET @REP_DATE_LY_MIN = (SELECT DATEADD(d, -364, MIN(REP_DATE)) FROM dbo.MARS_CMS_FORECAST)
	SET @REP_DATE_LY_MAX = (SELECT DATEADD(d, -364, MAX(REP_DATE)) FROM dbo.MARS_CMS_FORECAST);

	-- update ONRENT_LY from FES History table
	-- Date of LY, subtract 364 days(52 weeks) to get same day of the week matching
	--=====================================================================================
	UPDATE MARS_CMS_FORECAST
	SET ONRENT_LY = FESH.OnRent
	FROM MARS_CMS_FORECAST FC
		INNER JOIN
		(
		--SELECT 
		--	DATEADD(d,364,FESH.REP_DATE) AS REP_DATE, 
		--	FESH.COUNTRY, 
		--	L.CMS_LOCATION_GROUP_ID, 
		--	CC.CAR_CLASS_ID, 
		--	SUM(FESH.ON_RENT_MAX_ABSOLUTE) AS 'ON_RENT'
		--FROM FLEET_EUROPE_SUMMARY_HISTORY FESH
		--INNER JOIN dbo.vw_Mapping_CMS_Location L ON L.COUNTRY = FESH.COUNTRY AND L.location = FESH.LOCATION
		--INNER JOIN dbo.vw_Mapping_CarClass CC    ON CC.COUNTRY = FESH.COUNTRY AND CC.CAR_CLASS = FESH.CAR_GROUP		
		--WHERE 
		--	((FLEET_RAC_TTL = 1 OR FLEET_CARSALES = 1) 
		--AND REP_DATE BETWEEN @REP_DATE_LY_MIN AND @REP_DATE_LY_MAX)
		--GROUP BY 
		--	FESH.REP_DATE, FESH.COUNTRY, L.CMS_LOCATION_GROUP_ID, CC.CAR_CLASS_ID
			SELECT dateadd(day, 364,fh.Timestamp) as Rep_Date
					, l.cms_location_group_id
					, fh.CarGroupId 
					, sum(fh.MaxOnRent) as OnRent
			FROM fleethistory fh
			join LOCATIONS l on fh.LocationId = l.dim_Location_id
			where fh.FleetTypeId in (4,5, 8)
				and fh.Timestamp BETWEEN @REP_DATE_LY_MIN AND @REP_DATE_LY_MAX
			group by fh.Timestamp, l.cms_location_group_id, fh.CarGroupId

		) FESH ON 
			(FC.REP_DATE = FESH.REP_DATE 
		AND FC.CMS_LOCATION_GROUP_ID = FESH.CMS_LOCATION_GROUP_ID 
		AND FC.CAR_CLASS_ID = FESH.CarGroupId) OPTION (MAXDOP 1)





		/*
					
			SELECT dateadd(day, 364,fh.Timestamp) as RepDate, l.cms_location_group_id, fh.CarGroupId, sum(fh.MaxOnRent) as OnRent
			FROM fleethistory fh
			join LOCATIONS l on fh.LocationId = l.dim_Location_id
			where fh.FleetTypeId in (4,5, 8)
				and fh.Timestamp BETWEEN @REP_DATE_LY_MIN AND @REP_DATE_LY_MAX
			group by fh.Timestamp, l.cms_location_group_id, fh.CarGroupId

		*/
			
	SELECT @@rowcount		
			

	
END