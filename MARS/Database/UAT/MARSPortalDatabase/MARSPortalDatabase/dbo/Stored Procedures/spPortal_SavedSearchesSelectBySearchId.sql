-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_SavedSearchesSelectBySearchId] 
(
		@searchId	INT = NULL
)

AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT     
		searchId , searchName , userId, 
		country , cms_pool_id , cms_location_group_id , ops_region_id, 
		ops_area_id , location , car_segment_id , car_class_id, 
        car_group_id
	FROM
		MARS_Users_Saved_Searches
	WHERE 
		searchId = @searchId 

END