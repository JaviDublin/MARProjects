-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSAreaDelete] 
(
	@ops_area_id int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE
	FROM OPS_AREAS
	WHERE ops_area_id = @ops_area_id
	
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE ops_area_id=ops_area_id
END TRY

BEGIN CATCH
	SELECT -1
END CATCH


END