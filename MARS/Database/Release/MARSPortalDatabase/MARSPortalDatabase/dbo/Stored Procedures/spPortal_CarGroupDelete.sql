-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarGroupDelete] 
(
	@car_group_id int =NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE
	FROM CAR_GROUPS
	WHERE car_group_id = @car_group_id
	
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE car_group_id=@car_group_id 
	
	---- manage entries in Necessary Fleet
	--DELETE FROM MARS_CMS_NECESSARY_FLEET 
	--WHERE CAR_CLASS_ID = @car_group_id
	
END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END