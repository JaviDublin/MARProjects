-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spOPSRegionsSelect]
	-- Add the parameters for the stored procedure here
	@country VARCHAR(10) = NULL
	,@CAL As VarChar(1) = '*'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ops_region_id, ops_region FROM OPS_REGIONS 
	WHERE ISActive =1 AND (country = @country) OR (@country IS NULL)
	-- Altered by Gavin for MarsV3 CAL filtering
	and (@CAL in (select loc.cal from OPS_AREAS as ops
					join dbo.LOCATIONS as loc on loc.ops_area_id = ops.ops_area_id
					where ops.ops_region_id = OPS_REGIONS.ops_region_id)
					or @CAL = '*')
	ORDER BY country, ops_region
END