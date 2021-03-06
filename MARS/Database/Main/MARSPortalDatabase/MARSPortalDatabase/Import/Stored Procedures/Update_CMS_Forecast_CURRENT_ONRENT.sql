﻿-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Update_CMS_Forecast_CURRENT_ONRENT]
	
	--@theDay DATETIME
	
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @theDay DATETIME = DATEADD(dd, DATEDIFF(dd, 0, GETDATE()), 0) 

	--declare @the_day datetime = @theDay

    --UPDATE CURRENT_ONRENT in CMS_FORECAST table	
	UPDATE MARS_CMS_FORECAST SET 
		CURRENT_ONRENT			= FES.ON_RENT			, 
		AVAILABLE_FLEET			= FES.AVAILABLE_FLEET	, 
		OPERATIONAL_FLEET		= FES.OPERATIONAL_FLEET	, 
		OPERATIONAL_FLEET_S1	= FES.OPERATIONAL_FLEET	, 
		OPERATIONAL_FLEET_S2	= FES.OPERATIONAL_FLEET	, 
		OPERATIONAL_FLEET_S3	= FES.OPERATIONAL_FLEET	, 
		OPERATIONAL_FLEET_AV	= FES.OPERATIONAL_FLEET	, 
		TOTAL_FLEET				= FES.TOTAL_FLEET
	FROM MARS_CMS_FORECAST FC
		INNER JOIN
		(
			SELECT 
				FES.REP_DATE, FES.COUNTRY, L.CMS_LOCATION_GROUP_ID, CC.CAR_CLASS_ID, 
				SUM(FES.ON_RENT)			AS 'ON_RENT', 
				SUM(FES.AVAILABLE_FLEET)	AS 'AVAILABLE_FLEET', 
				SUM(FES.OPERATIONAL_FLEET)	AS 'OPERATIONAL_FLEET'	, 
				SUM(FES.TOTAL_FLEET)		AS 'TOTAL_FLEET'
			FROM 
				FLEET_EUROPE_SUMMARY_HISTORY FES
			
			INNER JOIN dbo.vw_Mapping_CMS_Location L	ON 
					L.COUNTRY		= FES.COUNTRY 
				AND L.location		= FES.LOCATION
			
			INNER JOIN dbo.vw_Mapping_CarClass CC		ON 
					CC.COUNTRY		= FES.COUNTRY 
				AND CC.CAR_CLASS	= FES.CAR_GROUP		
			WHERE 
				((FLEET_RAC_TTL = 1 OR FLEET_CARSALES = 1) AND REP_DATE = @theDay )
			GROUP BY 
				FES.REP_DATE, FES.COUNTRY, L.CMS_LOCATION_GROUP_ID, CC.CAR_CLASS_ID

			/*
			SELECT fh.Timestamp, l.cms_location_group_id
				, fh.CarGroupId as CAR_CLASS_ID
				, cast( sum(fh.AvgOnRent) as numeric(9,2)) as ON_RENT
				, cast( sum(fh.AvgAvailableFleet)as numeric(9,2)) as AVAILABLE_FLEET
				, cast( sum(fh.AvgOperationalFleet)as numeric(9,2)) as OPERATIONAL_FLEET
				, cast( sum(fh.AvgTotal)as numeric(9,2)) as TOTAL_FLEET
			FROM fleethistory fh
			join LOCATIONS l on fh.LocationId = l.dim_Location_id
			where fh.FleetTypeId in (4,5, 8)
				and fh.Timestamp = cast( (getdate() -1) as date )
			group by fh.Timestamp, l.cms_location_group_id, fh.CarGroupId

			*/
			
		) FES ON FC.CMS_LOCATION_GROUP_ID = FES.CMS_LOCATION_GROUP_ID AND FC.CAR_CLASS_ID = FES.CAR_CLASS_ID
		
	
	-- Update Current_Onrent in History table for the day (from average, but it will update with correct On_Rent a day after)
	--UPDATE MARS_CMS_FORECAST_HISTORY SET 
	--	CURRENT_ONRENT = FC.CURRENT_ONRENT
	--FROM 
	--	MARS_CMS_FORECAST FC
	--INNER JOIN MARS_CMS_FORECAST_HISTORY FCH ON 
	--		FCH.REP_DATE				= FC.REP_DATE 
	--	AND FCH.CMS_LOCATION_GROUP_ID	= FC.CMS_LOCATION_GROUP_ID 
	--	AND FCH.CAR_CLASS_ID			= FC.CAR_CLASS_ID
	--WHERE 
	--	FC.REP_DATE = @theDay

	-- Added 14/04/2014
	UPDATE FCH SET 
		FCH.CURRENT_ONRENT = FC.CURRENT_ONRENT
	FROM 
		MARS_CMS_FORECAST FC
	INNER JOIN MARS_CMS_FORECAST_HISTORY FCH ON 
			FCH.REP_DATE				= FC.REP_DATE 
		AND FCH.CMS_LOCATION_GROUP_ID	= FC.CMS_LOCATION_GROUP_ID 
		AND FCH.CAR_CLASS_ID			= FC.CAR_CLASS_ID
	WHERE 
		FC.REP_DATE = @theDay


	
END