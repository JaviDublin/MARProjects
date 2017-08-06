-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetFleetRACTTLForOwnareaAndLstwwd]
(
	@ownarea varchar(20),
	@lstwwd  varchar(50)	
)
RETURNS bit
AS
BEGIN

DECLARE @isCarsales bit
DECLARE @isLicensee bit
DECLARE @isAdvantage bit
DECLARE @isHertzOnDemand bit

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

RETURN
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
			THEN
				1
			ELSE
				0
		END
)
	

END