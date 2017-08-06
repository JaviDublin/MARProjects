-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spCarSegmentsSelect]
	-- Add the parameters for the stored procedure here	
	@country VARCHAR(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT car_segment_id, car_segment FROM CAR_SEGMENTS AS CS	
	LEFT JOIN COUNTRIES AS C ON C.country = CS.country
	WHERE ((C.country = @country) OR @country IS NULL)	
	ORDER BY CS.sort_car_segment, CS.car_segment

END