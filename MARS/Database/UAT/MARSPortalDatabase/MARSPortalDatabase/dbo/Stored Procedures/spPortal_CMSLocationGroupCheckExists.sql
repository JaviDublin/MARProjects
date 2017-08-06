-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSLocationGroupCheckExists] 
(
	@cms_location_group_id INT = NULL
)
AS
BEGIN
	
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT cms_location_group_id
					FROM CMS_LOCATION_GROUPS 
					WHERE cms_location_group_id = @cms_location_group_id)	
	BEGIN
		SELECT 0
	END
	ELSE
	BEGIN
		SELECT -1
	END
END