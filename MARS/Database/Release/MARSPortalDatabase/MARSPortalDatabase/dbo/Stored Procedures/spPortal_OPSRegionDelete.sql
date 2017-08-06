-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSRegionDelete] 
(
	@ops_region_id int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE
	FROM OPS_Regions
	WHERE ops_region_id = @ops_region_id
	
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE ops_region_id =@ops_region_id
END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END