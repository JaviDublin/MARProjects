-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarSegmentSelectWithCountry] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     
		car_segment_id, '(' + country + ') - ' + car_segment AS car_segment
FROM         CAR_SEGMENTS
ORDER BY country, car_segment
END