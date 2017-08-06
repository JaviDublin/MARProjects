-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarClassInsert] 
(	
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
	INSERT INTO CAR_CLASSES
	(car_class, car_segment_id, sort_car_class) 
	VALUES( @car_class, @car_segment_id, @sort_car_class )

	-- Return Success
	SELECT 0

END