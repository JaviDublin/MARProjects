-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarSegmentDelete] 
(
	@car_segment_id INT=NULL
)
AS
BEGIN
	
	SET NOCOUNT ON;

   
	BEGIN TRY
		DELETE FROM CAR_SEGMENTS
		WHERE 
			car_segment_id = @car_segment_id
		
		DELETE FROM dbo.MARS_Users_Saved_Searches
		WHERE 
			car_segment_id = @car_segment_id 
			
	END TRY

	BEGIN CATCH
		SELECT -1
	END CATCH

END