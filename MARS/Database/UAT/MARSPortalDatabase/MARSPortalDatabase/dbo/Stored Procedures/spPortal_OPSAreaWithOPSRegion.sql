-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSAreaWithOPSRegion] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     
		a.ops_area_id, 
		'(' + r.country + ') - ' + r.ops_region + ' - ' + a.ops_area AS ops_area
FROM         
		OPS_AREAS a
INNER JOIN
		OPS_REGIONS r ON a.ops_region_id = r.ops_region_id
ORDER BY
		r.country, r.ops_region, a.ops_area
END