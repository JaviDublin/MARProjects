-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_SavedSearchesUpdate] 
(
	@searchId					INT=NULL,
	@searchName					VARCHAR(50) = NULL,
	@country					VARCHAR(10) = NULL,
	@cms_pool_id				INT =NULL,
	@cms_location_group_id		INT = NULL,--@cms_location_group_code	VARCHAR(10) = NULL,
	@ops_region_id				INT=NULL,
	@ops_area_id				INT=NULL,
	@location					VARCHAR(50) = NULL,
	@car_segment_id				INT=NULL,
	@car_class_id				INT=NULL,
	@car_group_id				INT=NULL

)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE dbo.MARS_Users_Saved_Searches SET
		searchName =@searchName,
		country = @country,
		cms_pool_id=@cms_pool_id , 
		cms_location_group_id=@cms_location_group_id , --cms_location_group_code=@cms_location_group_code , 
		ops_region_id =@ops_region_id, 
		ops_area_id =@ops_area_id ,
		location=@location , 
		car_segment_id =@car_segment_id , 
		car_class_id =@car_class_id , 
		car_group_id =@car_group_id 
	WHERE
		searchId =@searchId 


	-- Return Success
	SELECT 0

END