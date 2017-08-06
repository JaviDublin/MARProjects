-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarClassDelete]
( 
	@car_class_id int=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE
	FROM CAR_CLASSES
	WHERE car_class_id = @car_class_id
	
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE car_class_id = @car_class_id
END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END