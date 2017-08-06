﻿-- =============================================
-- Author:		Gavin Williams
-- Create date: 18-9-12
-- Description:	Gets the data for the vehicles abroad non rev
-- =============================================
CREATE PROCEDURE [VehiclesAbroad].[GetNonRevData]

	@COUNTRY varchar(10)
	,@DAYSREV float
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN
	CREATE TABLE #TEMP
		(
		OWNING_COUNTRY varchar(10),
		IN_COUNTRY varchar(10),
		SERIAL varchar(25),
		OPERSTAT varchar(10),
		DAYSREV float
		)
END
begin
	if @COUNTRY='' or @COUNTRY is null
	begin
		set @country = '%'
	end
end
BEGIN
	INSERT INTO #TEMP
	SELECT
		COWN.country AS OWNING_COUNTRY,
		CIN.country AS IN_COUNTRY,
		FEA.SERIAL AS SERIAL,
		CASE FEA.OPERSTAT
			WHEN 'RT' THEN 'RENTABLE'
			WHEN 'BD' THEN 'SHOP'
			WHEN 'MM' THEN 'SHOP'
			WHEN 'TW' THEN 'SHOP'
			ELSE 'OTHER'
		END AS OPERSTAT,
		FEA.DAYSREV
	FROM
		dbo.FLEET_EUROPE_ACTUAL FEA
		join dbo.COUNTRIES cin on LEFT(fea.lstwwd, 2) = cin.country
		INNER JOIN dbo.COUNTRIES COWN ON FEA.COUNTRY = COWN.country
	WHERE
		(FEA.TOTAL_FLEET = 1)
		and (FEA.FLEET_RAC_TTL = 1 OR FEA.FLEET_CARSALES = 1)
		and
		(NOT(FEA.ON_RENT = 1))
		AND
		(NOT(FEA.OVERDUE = 1))
		AND
		(fea.COUNTRY != LEFT(fea.Lstwwd, 2))
		AND
		COWN.active = 1
END
BEGIN
	SELECT TOP 2147483647
		Q.OWNING_COUNTRY,
		Q.IN_COUNTRY,
		Q.RENTABLE,
		Q.SHOP,
		Q.OTHER
	FROM
		(
			SELECT
				1 AS SORT,
				T.OWNING_COUNTRY,
				T.IN_COUNTRY,
				(SELECT COUNT(T1.SERIAL) FROM #TEMP T1 WHERE T1.OPERSTAT = 'RENTABLE' AND T1.OWNING_COUNTRY = T.OWNING_COUNTRY AND T1.IN_COUNTRY = T.IN_COUNTRY AND T1.OWNING_COUNTRY LIKE @COUNTRY AND T1.DAYSREV >= @DAYSREV) AS RENTABLE,
				(SELECT COUNT(T1.SERIAL) FROM #TEMP T1 WHERE T1.OPERSTAT = 'SHOP' AND T1.OWNING_COUNTRY = T.OWNING_COUNTRY AND T1.IN_COUNTRY = T.IN_COUNTRY AND T1.OWNING_COUNTRY LIKE @COUNTRY AND T1.DAYSREV >= @DAYSREV) AS SHOP,
				(SELECT COUNT(T1.SERIAL) FROM #TEMP T1 WHERE T1.OPERSTAT = 'OTHER' AND T1.OWNING_COUNTRY = T.OWNING_COUNTRY AND T1.IN_COUNTRY = T.IN_COUNTRY AND T1.OWNING_COUNTRY LIKE @COUNTRY AND T1.DAYSREV >= @DAYSREV) AS OTHER
			FROM
				#TEMP T
			UNION
			SELECT
				2 AS SORT,
				T.OWNING_COUNTRY,
				'TOTAL',
				(SELECT COUNT(T1.SERIAL) FROM #TEMP T1 WHERE T1.OPERSTAT = 'RENTABLE' AND T1.OWNING_COUNTRY = T.OWNING_COUNTRY AND T1.OWNING_COUNTRY LIKE @COUNTRY AND T1.DAYSREV >= @DAYSREV) AS RENTABLE,
				(SELECT COUNT(T1.SERIAL) FROM #TEMP T1 WHERE T1.OPERSTAT = 'SHOP' AND T1.OWNING_COUNTRY = T.OWNING_COUNTRY AND T1.OWNING_COUNTRY LIKE @COUNTRY AND T1.DAYSREV >= @DAYSREV) AS SHOP,
				(SELECT COUNT(T1.SERIAL) FROM #TEMP T1 WHERE T1.OPERSTAT = 'OTHER' AND T1.OWNING_COUNTRY = T.OWNING_COUNTRY AND T1.OWNING_COUNTRY LIKE @COUNTRY AND T1.DAYSREV >= @DAYSREV) AS OTHER
			FROM
				#TEMP T
		) AS Q
	ORDER BY
		Q.OWNING_COUNTRY,
		Q.SORT,
		Q.IN_COUNTRY
END
BEGIN
	DROP TABLE #TEMP
END

END

/*
exec [VehiclesAbroad].[GetNonRevData]
@country='',
@DAYSREV=0
*/