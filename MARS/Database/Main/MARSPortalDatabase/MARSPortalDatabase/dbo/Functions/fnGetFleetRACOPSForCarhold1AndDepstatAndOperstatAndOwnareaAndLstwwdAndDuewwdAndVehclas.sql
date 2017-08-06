
CREATE FUNCTION [dbo].[fnGetFleetRACOPSForCarhold1AndDepstatAndOperstatAndOwnareaAndLstwwdAndDuewwdAndVehclas]
(
	-- Add the parameters for the function here
	@carhold1 varchar(5),
	@depstat varchar(5),
	@operstat varchar(10),
	@ownarea varchar(10),
	@lstwwd varchar(10),
	@duewwd varchar(10),
	@vehclas varchar(5)	
)
RETURNS bit
AS
BEGIN
	DECLARE @country varchar(10)
DECLARE @isCarsales bit
DECLARE @isLicensee bit
DECLARE @isAdvantage bit
DECLARE @isHertzOnDemand bit
DECLARE @isRACOPS bit
SELECT @country = country FROM AREACODES WHERE ownarea=@ownarea
SELECT @isCarsales = carsales FROM AREACODES WHERE ownarea=@ownarea
SELECT @isLicensee = licensee FROM AREACODES WHERE ownarea=@ownarea
SELECT @isAdvantage =	(
						SELECT  
							CASE WHEN 
								(SELECT Advantage FROM AREACODES WHERE ownarea=@ownarea) = 1
								OR
								@lstwwd  IN
										(select location from LOCATIONS where cms_location_group_id in
										(select cms_location_group_id from CMS_LOCATION_GROUPS where cms_pool_id in
										(select cms_pool_id from CMS_POOLS where cms_pool LIKE '%ADVANTAGE%')))
							THEN
								1
							ELSE
								0
							END
						)
SELECT @isHertzOnDemand = HertzOnDemand FROM AREACODES WHERE ownarea=@ownarea

IF @country = 'UK'
BEGIN
	SET @isRACOPS = 
	(
		SELECT
			CASE
				WHEN
					(@isCarsales = 0)
					AND
					(@isLicensee = 0)
					AND
					(@isAdvantage = 0)
					AND
					(@isHertzOnDemand = 0)
					AND
					(
						(@depstat IS NULL)
						OR
						(NOT(@depstat IN ('I', 'C', 'S')))
					)
					AND
					(
						(@carhold1 IS NULL)
						OR
						(NOT(@carhold1 IN ('H', 'L', 'V', 'X')))
					)
					AND
					(@operstat IN ('BD', 'MM', 'TW', 'TB', 'FS', 'RL', 'RP', 'TN', 'RT', 'SU'))
					--COUNTRY SPECIFIC PART
					AND	
					(
						(@vehclas IS NULL)
						OR
						(NOT(@vehclas = 'S'))
					)
				THEN
					1
				ELSE
					0
			END
	)
END
ELSE IF @country = 'GE'
BEGIN
	SET @isRACOPS = 
	(
		SELECT
			CASE
				WHEN
					(@isCarsales = 0)
					AND
					(@isLicensee = 0)
					AND
					(@isAdvantage = 0)
					AND
					(@isHertzOnDemand = 0)
					AND
					(
						(@depstat IS NULL)
						OR
						(NOT(@depstat IN ('I', 'C', 'S')))
					)
					AND
					(
						(@carhold1 IS NULL)
						OR
						(NOT(@carhold1 IN ('H', 'L', 'V', 'X')))
					)
					AND
					(@operstat IN ('BD', 'MM', 'TW', 'TB', 'FS', 'RL', 'RP', 'TN', 'RT', 'SU'))
					--COUNTRY SPECIFIC PART
					AND
					(
						(
							(@duewwd IS NULL)
							OR
							(NOT(@duewwd LIKE 'GETB%'))
						)
						AND
						(
							(@lstwwd IS NULL)
							OR
							(NOT(@lstwwd LIKE 'GETB%'))
						)
					)
				THEN
					1
				ELSE
					0
			END
		)
	END
ELSE
BEGIN
	SET @isRACOPS = 
	(
		SELECT
			CASE
				WHEN
					(@isCarsales = 0)
					AND
					(@isLicensee = 0)
					AND
					(@isAdvantage = 0)
					AND
					(@isHertzOnDemand = 0)
					AND
					(
						(@depstat IS NULL)
						OR
						(NOT(@depstat IN ('I', 'C', 'S')))
					)
					AND
					(
						(@carhold1 IS NULL)
						OR
						(NOT(@carhold1 IN ('H', 'L', 'V', 'X')))
					)
					AND
					(@operstat IN ('BD', 'MM', 'TW', 'TB', 'FS', 'RL', 'RP', 'TN', 'RT', 'SU'))
				THEN
					1
				ELSE
					0
			END
		)
	END
RETURN @isRACOPS

END