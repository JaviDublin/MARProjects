-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarClassSelectById] 
(
	@car_class_id INT = NULL
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     car_class_id, car_class, car_segment_id, NULL AS car_segment, sort_car_class, NULL AS country
FROM         CAR_CLASSES
WHERE car_class_id = @car_class_id

END