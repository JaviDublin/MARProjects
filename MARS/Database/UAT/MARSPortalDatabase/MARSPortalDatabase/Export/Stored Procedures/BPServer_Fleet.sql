-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Export].[BPServer_Fleet]

AS
BEGIN
	
	SET NOCOUNT ON;


	DECLARE @targetDate DATETIME	
	SET @targetDate = CAST(CAST(DATEADD(d, -1, GETDATE()) AS VARCHAR(11)) AS DATETIME)

	SELECT 
		--REP_YEAR, REP_MONTH, REP_WEEK_OF_YEAR, 
		DATEADD(d, 0, DATEDIFF(d, 0, REP_DATE)) AS 'REP_DATE', 
		COUNTRY, CAR_GROUP,
		SUM(FES.TOTAL_FLEET) 'TOTAL_FLEET', 
		--SUM(FES.CARSALES) 'CARSALES', 
		--SUM(FES.CU) 'CU', 
		--SUM(FES.HA) 'HA', 
		--SUM(FES.HL) 'HL', 
		--SUM(FES.LL) 'LL', 
		--SUM(FES.NC) 'NC', 
		--SUM(FES.PL) 'PL', 
		--SUM(FES.TC) 'TC', 
		SUM(FES.SV) 'SV', 
		SUM(FES.WS) 'WS', 
		SUM(FES.OPERATIONAL_FLEET) 'OPERATIONAL_FLEET', 
		SUM(FES.BD) 'BD', 
		SUM(FES.MM) 'MM', 
		--SUM(FES.TW) 'TW', 
		SUM(FES.TB) 'TB', 
		--SUM(FES.FS) 'FS', 
		--SUM(FES.RL) 'RL', 
		--SUM(FES.RP) 'RP', 
		--SUM(FES.TN) 'TN', 
		SUM(FES.AVAILABLE_FLEET) 'AVAILABLE_FLEET', 
		SUM(FES.RT) 'RT', 
		SUM(FES.SU) 'SU', 
		--SUM(FES.GOLD) 'GOLD', 
		--SUM(FES.PREDELIVERY) 'PREDELIVERY', 
		SUM(FES.OVERDUE) 'OVERDUE', 
		SUM(FES.ON_RENT) 'ON_RENT' 
	FROM 
		FLEET_EUROPE_SUMMARY_HISTORY AS FES			
	WHERE	(FES.FLEET_CARSALES > 0 OR FES.FLEET_RAC_TTL > 0)--Fleet					
					AND FES.REP_DATE = @targetDate

	GROUP BY 
		REP_YEAR, REP_MONTH, REP_WEEK_OF_YEAR, REP_DATE ,COUNTRY, CAR_GROUP

END