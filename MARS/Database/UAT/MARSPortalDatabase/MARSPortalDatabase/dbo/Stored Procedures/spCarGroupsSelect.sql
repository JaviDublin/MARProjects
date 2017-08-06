-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spCarGroupsSelect]
	
	@country			VARCHAR(10) = NULL,
	@car_segment_id		INT			= NULL,
	@car_class_id		INT			= NULL	

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		car_group_id, car_group 
	FROM 
		CAR_GROUPS AS CG
	LEFT JOIN CAR_CLASSES	AS CC	ON CC.car_class_id		= CG.car_class_id
	LEFT JOIN CAR_SEGMENTS	AS CS	ON CS.car_segment_id	= CC.car_segment_id
	LEFT JOIN COUNTRIES		AS C	ON C.country			= CS.country
	WHERE 
		((C.country = @country) OR @country IS NULL)
	AND ((CS.car_segment_id = @car_segment_id) OR @car_segment_id IS NULL)
	AND ((CC.car_class_id = @car_class_id) OR @car_class_id IS NULL)
	ORDER BY 
		CS.sort_car_segment, CC.sort_car_class, CG.sort_car_group, CG.car_group

END