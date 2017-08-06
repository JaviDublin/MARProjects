-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarSegmentSelectById] 
(
	@car_segment_id INT =NULL
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     car_segment_id, car_segment, country, sort_car_segment
FROM         CAR_SEGMENTS
WHERE car_segment_id=@car_segment_id
	
END