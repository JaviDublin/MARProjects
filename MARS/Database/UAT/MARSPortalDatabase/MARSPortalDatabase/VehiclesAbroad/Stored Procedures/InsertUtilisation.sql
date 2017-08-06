-- =============================================
-- Author:	 Uwe Ahrendt (implemented by Gavin Williams)
-- Create date: 03-10-12
-- Description:	Inserts the days data into the VehiclesAbroad.Utilisation table
-- =============================================
CREATE PROCEDURE [VehiclesAbroad].[InsertUtilisation] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
		INSERT INTO VehiclesAbroad.CARS_ABROAD_HISTORY
		SELECT
			DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE())) AS REP_DATE,
			OWNC.country AS COUNTRY_OWN,
			LSTC.country AS COUNTRY_IN,
			DUEC.country AS COUNTRY_DUE,
			SUM(TOTAL_FLEET) AS TOTAL_FLEET,
			SUM(CU) + SUM(HA) + SUM(HL) + SUM(LL) + SUM(NC) + SUM(PL) + SUM(TC) + SUM(SV) + SUM(WS) + SUM(TB) + SUM(FS) + SUM(RL) + SUM(RP) + SUM(TN) AS OTHER,
			SUM(BD) + SUM(MM) + SUM(TW) AS SHOP,
			SUM(RT) + SUM(SU) AS IDLE,
			SUM(OVERDUE) AS OVERDUE,
			SUM(ON_RENT) AS ON_RENT
		FROM
			dbo.FLEET_EUROPE_ACTUAL FEA
			--COUNTRY_OWN
			INNER JOIN dbo.COUNTRIES OWNC ON FEA.COUNTRY = OWNC.country
			--COUNTRY_IN
			INNER JOIN dbo.LOCATIONS LSTL ON FEA.LSTWWD = LSTL.location
			INNER JOIN dbo.CMS_LOCATION_GROUPS LSTCMG ON LSTL.cms_location_group_id = LSTCMG.cms_location_group_id
			INNER JOIN dbo.CMS_POOLS LSTCMP ON LSTCMG.cms_pool_id = LSTCMP.cms_pool_id
			INNER JOIN dbo.COUNTRIES LSTC ON LSTCMP.country = LSTC.country
			--COUNTRY_DUE					
			INNER JOIN dbo.LOCATIONS DUEL ON CASE WHEN FEA.DUEWWD IS NULL THEN FEA.LSTWWD ELSE FEA.DUEWWD END = DUEL.location
			INNER JOIN dbo.CMS_LOCATION_GROUPS DUECMG ON DUEL.cms_location_group_id = DUECMG.cms_location_group_id
			INNER JOIN dbo.CMS_POOLS DUECMP ON DUECMG.cms_pool_id = DUECMP.cms_pool_id
			INNER JOIN dbo.COUNTRIES DUEC ON DUECMP.country = DUEC.country
		WHERE
			NOT(OWNC.country = LSTC.country)
		GROUP BY
			OWNC.country,
			LSTC.country,
			DUEC.country
END