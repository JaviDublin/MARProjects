-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_LocationDelete] 
(	
	@location varchar(50)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE
	FROM Locations
	WHERE location = @location
	
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE location=@location

END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END