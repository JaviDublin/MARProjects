﻿-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Insert_CMS_Forecast_History] 
	
	--@theDay DATETIME
	
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @theDay DATETIME = DATEADD(dd, DATEDIFF(dd, 0, GETDATE() - 1), 0) 
	
	-- Change by Javi 24/Sep/2012
	------------------------------------------------------------
	MERGE [MARS_CMS_FORECAST_HISTORY] AS [TARGET]
	USING 
	(
		SELECT 
			REP_DATE , CMS_LOCATION_GROUP_ID , CAR_CLASS_ID , COUNTRY
		FROM [MARS_CMS_FORECAST] 
		WHERE 
			REP_DATE BETWEEN @theDay 
		AND DATEADD(d, 56, @theDay)
		GROUP BY 
			REP_DATE , CMS_LOCATION_GROUP_ID , CAR_CLASS_ID , COUNTRY
		
	) AS [SOURCE]
	ON 
	(
		[TARGET].REP_DATE				= [SOURCE].REP_DATE 
	AND [TARGET].CMS_LOCATION_GROUP_ID	= [SOURCE].CMS_LOCATION_GROUP_ID 
	AND [TARGET].CAR_CLASS_ID			= [SOURCE].CAR_CLASS_ID
	AND [TARGET].COUNTRY				= [SOURCE].COUNTRY 
	)

	WHEN NOT MATCHED BY TARGET THEN
	INSERT 
	(
		[REP_DATE],
		[COUNTRY],
		[CMS_LOCATION_GROUP_ID],
		[CAR_CLASS_ID],
		[FLEET_UTILISATION_FROZEN] , [FLEET_NONREV_FROZEN] , [FLEET_MOVEMENT_FROZEN]
	)
	
	VALUES 
	(
		[SOURCE].REP_DATE, 
		[SOURCE].COUNTRY, 
		[SOURCE].CMS_LOCATION_GROUP_ID, 
		[SOURCE].CAR_CLASS_ID, 
		100.0, 0.0, 0
	);
	
	--MERGE [MARS_CMS_FORECAST_HISTORY] AS [TARGET]
	--USING 
	--(
	--	SELECT * 
	--	FROM [MARS_CMS_FORECAST] 
	--	WHERE 
	--		REP_DATE BETWEEN @theDay 
	--	AND DATEADD(d, 56, @theDay)
		
	--) AS [SOURCE]
	--ON 
	--(
	--	[TARGET].REP_DATE				= [SOURCE].REP_DATE 
	--AND [TARGET].CMS_LOCATION_GROUP_ID	= [SOURCE].CMS_LOCATION_GROUP_ID 
	--AND [TARGET].CAR_CLASS_ID			= [SOURCE].CAR_CLASS_ID
	--)

	--WHEN NOT MATCHED BY TARGET THEN
	--INSERT 
	--([REP_DATE],[COUNTRY],[CMS_LOCATION_GROUP_ID],[CAR_CLASS_ID],[CURRENT_ONRENT], [ONRENT_LY]
	--,[AVAILABLE_FLEET],[OPERATIONAL_FLEET],[TOTAL_FLEET]
	--,[FLEET_UTILISATION_FROZEN],[FLEET_NONREV_FROZEN],[FLEET_MOVEMENT_FROZEN]
	--,[CMS_CONSTRAINED_FROZEN],[CMS_UNCONSTRAINED_FROZEN]
	--,[CMS_CONSTRAINED],[CMS_UNCONSTRAINED]
	--,[CMS_CONSTRAINED_WK1],[CMS_UNCONSTRAINED_WK1],[CMS_CONSTRAINED_WK2],[CMS_UNCONSTRAINED_WK2]
	--,[CMS_CONSTRAINED_WK3],[CMS_UNCONSTRAINED_WK3],[CMS_CONSTRAINED_WK4],[CMS_UNCONSTRAINED_WK4]
	--,[CMS_CONSTRAINED_WK5],[CMS_UNCONSTRAINED_WK5],[CMS_CONSTRAINED_WK6],[CMS_UNCONSTRAINED_WK6]
	--,[CMS_CONSTRAINED_WK7],[CMS_UNCONSTRAINED_WK7],[CMS_CONSTRAINED_Wk8],[CMS_UNCONSTRAINED_Wk8]
	--,[ADJUSTMENT_TD],[ADJUSTMENT_BU1],[ADJUSTMENT_BU2],[ADJUSTMENT_RC])
	
	--VALUES 
	--(
	--	[SOURCE].REP_DATE, 
	--	[SOURCE].COUNTRY, 
	--	[SOURCE].CMS_LOCATION_GROUP_ID, 
	--	[SOURCE].CAR_CLASS_ID, 
	--	NULL, NULL
	--	, NULL, NULL, NULL
	--	, 100.0, 0.0, 0
	--	, NULL, NULL
	--	, NULL, NULL
	--	, NULL, NULL, NULL, NULL
	--	, NULL, NULL, NULL, NULL
	--	, NULL, NULL, NULL, NULL
	--	, NULL, NULL, NULL, NULL
	--	, NULL, NULL, NULL, NULL
	--);

    
END