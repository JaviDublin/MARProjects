



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnPoolingREACarGroupHierarchy]
(
	@car_group varchar(2) = NULL, 
	@country varchar(2) = NULL,
	@field varchar(20) = NULL
)
	RETURNS varchar(10)
AS
BEGIN

	declare @temp table(car_segment int, car_class int, carvan varchar(2))
	declare @return varchar(10)

	INSERT INTO @temp
	SELECT CS.car_segment_id, CC.car_class_id, left(car_segment,1)
	FROM dbo.CAR_GROUPS AS CG 
		INNER JOIN dbo.CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id 
		INNER JOIN dbo.CAR_SEGMENTS AS CS ON CC.car_segment_id = CS.car_segment_id 
		INNER JOIN dbo.COUNTRIES AS C ON CS.country = C.country
	where CG.car_group = @car_group and C.country = @country

	
	IF UPPER(@field) = 'CAR_SEGMENT'
		SET @return = (SELECT car_segment from @temp)
	ELSE IF UPPER(@field) = 'CAR_CLASS'
		SET @return = (SELECT car_class from @temp)
	ELSE IF UPPER(@field) = 'CARVAN'
		SET @return = (SELECT carvan from @temp)	
	ELSE 
		SET @return = NULL

	-- Return the result of the function
	RETURN @return
	
END