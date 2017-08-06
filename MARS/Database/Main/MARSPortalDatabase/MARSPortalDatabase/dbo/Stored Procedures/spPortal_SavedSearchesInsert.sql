-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_SavedSearchesInsert] 
(
	@searchName					VARCHAR(50) = NULL,
	@userId						VARCHAR(10) = NULL,
	@country					VARCHAR(10) = NULL,
	@cms_pool_id				INT =NULL,
	@cms_location_group_id	INT = NULL,--@cms_location_group_code	VARCHAR(10) = NULL,
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
	INSERT INTO dbo.MARS_Users_Saved_Searches
		(searchName, userId,country,cms_pool_id, cms_location_group_id , ops_region_id , 
			ops_area_id ,location, car_segment_id , car_class_id , car_group_id)
	VALUES
		(@searchName, @userId,@country,@cms_pool_id, @cms_location_group_id , @ops_region_id , 
			@ops_area_id ,@location, @car_segment_id , @car_class_id , @car_group_id)
	

	-- Return Success
	SELECT 0

END