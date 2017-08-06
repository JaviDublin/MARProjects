


CREATE FUNCTION [dbo].[fnGetFleetAdvantageForOwnareaAndLstwwd]
(
	@ownarea varchar(10),
	@lstwwd  varchar(10)
)
RETURNS bit
AS
BEGIN
	DECLARE @isAdvantage bit

	IF 
		(
			((SELECT Advantage FROM AREACODES WHERE ownarea=@ownarea) = 1) 
			OR
			@lstwwd  IN
			(select location from LOCATIONS where cms_location_group_id in
			(select cms_location_group_id from CMS_LOCATION_GROUPS where cms_pool_id in
			(select cms_pool_id from CMS_POOLS where cms_pool LIKE '%ADVANTAGE%')))
		)
		BEGIN
			SET @isAdvantage = 1
		END
	ELSE
		BEGIN
			SET @isAdvantage = 0
		END
	
	RETURN	@isAdvantage		

END