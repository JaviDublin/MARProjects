-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSAreaUpdate] 
	
	@ops_area_id int,
	@ops_area varchar(50), 
	@ops_region_id int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE OPS_AREAS SET  ops_area = @ops_area, ops_region_id = @ops_region_id

	WHERE ops_area_id = @ops_area_id

END