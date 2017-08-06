

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetCarClassForVcAndOwnwwd]
(
	@vc varchar(5),
	@ownwwd varchar(10)		
)
RETURNS int
AS
BEGIN
	DECLARE @country_code varchar(2)
	
	SELECT @country_code = LEFT(@ownwwd,(2))

	RETURN 
		(
			SELECT TOP 1 
				car_segment_id 
			FROM 
				CAR_CLASSES c
			WHERE 
				(car_class = @vc)
				
		)
END