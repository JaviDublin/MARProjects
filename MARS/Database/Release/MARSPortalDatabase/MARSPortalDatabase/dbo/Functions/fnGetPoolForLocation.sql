
CREATE FUNCTION [dbo].[fnGetPoolForLocation]
(
	@lstwwd varchar(10)
)
RETURNS int
AS
BEGIN
	
	-- Return the result of the function
	RETURN (SELECT CLG.cms_pool_id FROM CMS_LOCATION_GROUPS AS CLG
		JOIN LOCATIONS AS L ON L.cms_location_group_id = CLG.cms_location_group_id
		WHERE location = @lstwwd)

END