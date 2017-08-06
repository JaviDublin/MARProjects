-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarGroupSelectById] 
(
	@car_group_id INT = NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     car_group_id, car_group, car_group_gold, car_group_fivestar, car_group_presidentCircle,  car_group_platinum, cg.car_class_id, sort_car_group, cc.car_class_id AS car_class, cs.country as country
FROM         CAR_GROUPS cg
join CAR_CLASSES cc on cg.car_class_id = cc.car_class_id
join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
WHERE car_group_id = @car_group_id

END