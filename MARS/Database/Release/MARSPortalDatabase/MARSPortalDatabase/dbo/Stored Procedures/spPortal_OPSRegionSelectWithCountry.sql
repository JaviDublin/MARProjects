-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSRegionSelectWithCountry] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

SELECT     ops_region_id, '(' + country + ') - ' + ops_region AS ops_region
FROM         OPS_REGIONS
ORDER BY
		country, ops_region
END