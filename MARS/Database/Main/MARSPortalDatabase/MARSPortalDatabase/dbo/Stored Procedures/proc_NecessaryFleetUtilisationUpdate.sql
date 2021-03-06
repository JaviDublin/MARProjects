﻿-- =============================================
CREATE procedure [dbo].[proc_NecessaryFleetUtilisationUpdate]

	@Country varchar(2), 
	@dateFrom datetime, 
	@dateTo datetime
	
AS
BEGIN
--	=========================================================================
--	Create data entries in MARS_CMS_FORECAST_HISTORY table based on MARS_CMS_FORECAST table for particular day
--	==========================================================================

	SET NOCOUNT ON;
	
	-- MERGE Necessary Fleet table with re-calculation of Utilisation
	MERGE [MARS_CMS_NECESSARY_FLEET] AS [TARGET]
	USING (
		SELECT T.COUNTRY, T.CMS_LOCATION_GROUP_ID, T.CAR_CLASS_ID, ISNULL(S.Utilisation, 100.0) AS 'Utilisation'
		FROM
		(SELECT U.COUNTRY, U.CMS_LOCATION_GROUP_ID, U.CAR_CLASS_ID, (100.0 - U.Utilisation) AS 'Utilisation'
		FROM
		(		
				SELECT FES.COUNTRY, L.CMS_LOCATION_GROUP_ID, CC.CAR_CLASS_ID			
						, ROUND(ISNULL(SUM(RT)* 100.0 / NULLIF(SUM(AVAILABLE_FLEET), 0),0),2) AS 'Utilisation'
						, Rank() over (Partition BY FES.COUNTRY,CMS_LOCATION_GROUP_ID,CAR_CLASS_ID order by ISNULL(SUM(RT) * 100.0 / NULLIF(SUM(AVAILABLE_FLEET), 0),0) ASC, REP_DATE DESC) AS [RANK]						
				FROM FLEET_EUROPE_SUMMARY AS FES 
					--(
					--	(SELECT * FROM FLEET_EUROPE_SUMMARY WHERE REP_DATE <= @dateTo)
					--	UNION 
					--	(SELECT * FROM FLEET_EUROPE_SUMMARY_HISTORY WHERE REP_DATE >= @dateFrom)
					--) AS FES
						INNER JOIN dbo.vw_Mapping_CMS_Location L ON L.COUNTRY = FES.COUNTRY AND L.location = FES.LOCATION
						INNER JOIN dbo.vw_Mapping_CarClass CC ON CC.COUNTRY = FES.COUNTRY AND CC.CAR_CLASS = FES.CAR_GROUP		
				WHERE ((FLEET_RAC_TTL = 1 OR FLEET_CARSALES = 1) AND (FES.COUNTRY = @COUNTRY OR @COUNTRY IS NULL) AND REP_DATE BETWEEN @dateFrom AND @dateTo )	
				GROUP BY FES.REP_DATE, FES.COUNTRY, L.CMS_LOCATION_GROUP_ID, CC.CAR_CLASS_ID
				HAVING SUM(RT) IS NOT NULL
		) U
		WHERE [RANK] = 1
		) S
		RIGHT JOIN		
		(	SELECT LG.COUNTRY, CMS_LOCATION_GROUP_ID, CAR_CLASS_ID
			FROM vw_Mapping_CMS_Location_Group LG
				INNER JOIN vw_Mapping_CarClass CC ON LG.COUNTRY = CC.COUNTRY
			WHERE LG.COUNTRY = @COUNTRY
		) T ON S.CMS_LOCATION_GROUP_ID = T.CMS_LOCATION_GROUP_ID AND S.CAR_CLASS_ID = T.CAR_CLASS_ID
		
		) AS [SOURCE]
		
	
	ON ([TARGET].COUNTRY = [SOURCE].COUNTRY AND [TARGET].CMS_LOCATION_GROUP_ID = [SOURCE].CMS_LOCATION_GROUP_ID 
		AND [TARGET].CAR_CLASS_ID = [SOURCE].CAR_CLASS_ID)

	-- 1. If not exists in Necessary table, then INSERT	
	WHEN NOT MATCHED BY TARGET THEN
	INSERT ([COUNTRY],[CMS_LOCATION_GROUP_ID],[CAR_CLASS_ID],[UTILISATION], [NONREV_FLEET])
	VALUES ([SOURCE].COUNTRY, [SOURCE].CMS_LOCATION_GROUP_ID, [SOURCE].CAR_CLASS_ID
			, 100.0, 0.0)
			
	-- 2. If the entry exists in Necessary table, then UPDATE
	WHEN MATCHED AND [TARGET].[UTILISATION] <> [SOURCE].[UTILISATION] THEN	
	UPDATE SET [TARGET].[UTILISATION] = [SOURCE].[UTILISATION]
	;
	


END