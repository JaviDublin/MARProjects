-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarClassSelectWithCarSegment] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     
		c.car_class_id,
		'(' + s.country + ') - ' + s.car_segment + ' - '+ c.car_class AS car_class 
FROM         
		CAR_CLASSES c
INNER JOIN
        CAR_SEGMENTS s ON c.car_segment_id = s.car_segment_id
ORDER BY
		s.country, s.car_segment, c.car_class

END