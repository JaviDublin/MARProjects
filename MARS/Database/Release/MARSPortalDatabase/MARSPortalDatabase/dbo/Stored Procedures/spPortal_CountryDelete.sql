-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CountryDelete] 
(
	@country VARCHAR(10)=NULL
)		
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY

	DELETE
	FROM COUNTRIES
	WHERE country = @country
	
	DELETE FROM dbo.MARS_Users_Saved_Searches
	WHERE country=@country
	
	SELECT 0
END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END