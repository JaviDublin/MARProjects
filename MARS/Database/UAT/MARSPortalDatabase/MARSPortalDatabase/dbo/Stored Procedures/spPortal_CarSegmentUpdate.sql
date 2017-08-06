-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarSegmentUpdate] 
(	
	@car_segment_id int=NULL,
	@car_segment varchar(50)=NULL, 
	@country varchar(10)=NULL,
	@sort_car_segment int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CAR_SEGMENTS SET  
	car_segment = @car_segment, 
	country = @country, 
	sort_car_segment = @sort_car_segment

	WHERE car_segment_id = @car_segment_id

END