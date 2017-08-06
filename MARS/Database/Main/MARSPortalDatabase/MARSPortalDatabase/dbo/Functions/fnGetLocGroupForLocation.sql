CREATE FUNCTION [dbo].[fnGetLocGroupForLocation]
(
	-- Add the parameters for the function here
	@location_id varchar(50)
)
RETURNS VARCHAR(10)
AS
BEGIN
	RETURN (SELECT cms_location_group_id FROM LOCATIONS WHERE location = @location_id)
END