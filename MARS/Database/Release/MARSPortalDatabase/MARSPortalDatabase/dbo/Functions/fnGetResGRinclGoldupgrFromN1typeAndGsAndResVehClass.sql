


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetResGRinclGoldupgrFromN1typeAndGsAndResVehClass]
(
@n1type varchar(2),
@gs varchar(2),
@resvehclass varchar(2)
,@country varchar(2)
)
RETURNS varchar(2)
AS
BEGIN
				RETURN
				(
					SELECT
						CASE 
							WHEN 
								(@gs = 'Y') 
					AND
(@n1type IN ('5', 'E', 'G', 'I', 'J', 'K', 'P'))
							THEN
--(SELECT car_group_gold FROM dbo.CAR_GROUPS WHERE (car_group = @resvehclass))
(
	SELECT CG.car_group_gold 
	FROM dbo.CAR_GROUPS CG
		LEFT JOIN dbo.CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id 
		LEFT JOIN dbo.CAR_SEGMENTS AS CS ON CC.car_segment_id = CS.car_segment_id 
	WHERE (CG.car_group = @resvehclass) and CS.country = @country
)
							ELSE
								@resvehclass 
						END
				)
END