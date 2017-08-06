-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarClassUpdate] 
(	
	@car_class_id int =NULL,
	@car_class varchar(50)=NULL, 
	@car_segment_id int=NULL,
	@sort_car_class int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CAR_CLASSES SET  
	car_class = @car_class, 
	car_segment_id = @car_segment_id, 
	sort_car_class = @sort_car_class

	WHERE car_class_id = @car_class_id

END