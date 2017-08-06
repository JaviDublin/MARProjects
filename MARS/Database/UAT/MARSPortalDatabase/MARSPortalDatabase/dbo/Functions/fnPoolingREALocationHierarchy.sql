

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnPoolingREALocationHierarchy]
(
	@rent_loc varchar(10) = NULL, 
	@field varchar(20) = NULL
)
	RETURNS varchar(10)
AS
BEGIN

	declare @temp table(country varchar(2), cms_pool int,cms_loc_grp varchar(10),ops_region int, ops_area int)
	declare @return varchar(10)

	INSERT INTO @temp
	SELECT OC.country,CP.cms_pool_id,CL.cms_location_group_id,[OR].ops_region_id,OA.ops_area_id--, L.location
	FROM dbo.LOCATIONS AS L
		INNER JOIN dbo.OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id 
		INNER JOIN dbo.OPS_REGIONS AS [OR] ON OA.ops_region_id = [OR].ops_region_id 
		INNER JOIN dbo.COUNTRIES AS OC ON [OR].country = OC.country
		INNER JOIN dbo.CMS_LOCATION_GROUPS AS CL ON L.cms_location_group_id = CL.cms_location_group_id
		INNER JOIN dbo.CMS_POOLS AS CP ON CL.cms_pool_id = CP.cms_pool_id
		INNER JOIN dbo.COUNTRIES AS CC ON CP.country = CC.country
	where L.location = @rent_loc

	
	IF UPPER(@field) = 'COUNTRY'
		SET @return = (SELECT country from @temp)
	ELSE IF UPPER(@field) = 'CMS_POOL'
		SET @return = (SELECT cms_pool from @temp)
	ELSE IF UPPER(@field) = 'CMS_LOC_GRP'
		SET @return = (SELECT cms_loc_grp from @temp)
	ELSE IF UPPER(@field) = 'OPS_REGION'
		SET @return = (SELECT ops_region from @temp)
	ELSE IF UPPER(@field) = 'OPS_AREA'
		SET @return = (SELECT ops_area from @temp)
	ELSE 
		SET @return = NULL

	-- Return the result of the function
	RETURN @return
	
END