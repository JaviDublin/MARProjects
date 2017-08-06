-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSLocationGroupDelete] 
(
	@cms_location_group_id INT--@cms_location_group_code varchar(10)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE
	FROM CMS_Location_Groups
	WHERE cms_location_group_id = @cms_location_group_id--cms_location_group_code = @cms_location_group_code
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE  cms_location_group_id =@cms_location_group_id --cms_location_group_code =@cms_location_group_code 
	
	---- manage entries in Necessary Fleet
	--DELETE FROM MARS_CMS_NECESSARY_FLEET 
	--WHERE CMS_LOCATION_GROUP_ID = @cms_location_group_id

END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END