-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spOPSAreasSelect]
	
	@country VARCHAR(10) = NULL,
	@ops_region_id INT = NULL
	,@CAL As VarChar(1) = '*' -- Added by Gavin for CAL filtering
AS
BEGIN
	
	SET NOCOUNT ON;

    
	SELECT 
		ops_area_id, ops_area 
	FROM 
		OPS_AREAS
	LEFT JOIN OPS_REGIONS ON OPS_REGIONS.ops_region_id = OPS_AREAS.ops_region_id
	WHERE 
		OPS_AREAS.IsActive=1 AND ((OPS_REGIONS.country = @country) OR (@country IS NULL)) 
	AND 
		((OPS_AREAS.ops_region_id = @ops_region_id) OR (@ops_region_id IS NULL))
	AND 
		(@CAL in (select loc.Cal from LOCATIONS as loc
						where loc.ops_area_id = OPS_AREAS.ops_area_id)
						or @CAL = '*')
	ORDER BY 
		OPS_REGIONS.country, OPS_AREAS.ops_area

END