-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSPoolDelete] 
(
	@cms_pool_id int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE
	FROM CMS_POOLS
	WHERE cms_pool_id = @cms_pool_id
	
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE cms_pool_id = @cms_pool_id 
END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END