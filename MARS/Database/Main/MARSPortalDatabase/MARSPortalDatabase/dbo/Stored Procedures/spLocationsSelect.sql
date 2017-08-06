-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spLocationsSelect]
	-- Add the parameters for the stored procedure here
	@country VARCHAR(10) = NULL,
	@ops_region_id INT = NULL,
	@ops_area_id INT = NULL,
	@cms_pool_id INT = NULL,
	@cms_location_group_id int = NULL --@cms_location_group_code VARCHAR(10) = NULL
	,@CAL as Varchar(1) = '*'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here 
    -- Altered by Gavin for MARSV3 cal added --
	SELECT L.location, L.real_location_name, L.location_name, L.location_name_dw, cal FROM LOCATIONS AS L 
	LEFT JOIN OPS_AREAS AS A ON A.ops_area_id = L.ops_area_id
	LEFT JOIN OPS_REGIONS AS R ON R.ops_region_id = A.ops_region_id
	LEFT JOIN CMS_LOCATION_GROUPS AS LG ON LG.cms_location_group_id = L.cms_location_group_id	
	LEFT JOIN CMS_POOLS AS P ON P.cms_pool_id = LG.cms_pool_id
	LEFT JOIN COUNTRIES AS C1 ON C1.country = R.country
	LEFT JOIN COUNTRIES AS C2 ON C2.country = P.country
	WHERE ((R.country = @country) OR (P.country = @country) OR (@country IS NULL)) 
		AND ((R.ops_region_id = @ops_region_id) OR (@ops_region_id IS NULL))
		AND ((A.ops_area_id = @ops_area_id) OR (@ops_area_id IS NULL))
		AND ((P.cms_pool_id = @cms_pool_id) OR (@cms_pool_id IS NULL))
		AND ((LG.cms_location_group_id = @cms_location_group_id) OR (@cms_location_group_id IS NULL))
		AND L.active = 1
		and (L.cal = @CAL OR @CAL = '*')
	ORDER BY C1.country, C2.country, L.location

END