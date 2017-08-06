-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSAreaSelectById] 
(
	@ops_area_Id INT=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT     ops_area_id, ops_area, ops_region_id, NULL AS ops_region, NULL AS country
FROM         OPS_AREAS
WHERE ops_area_id=@ops_area_id
  

END