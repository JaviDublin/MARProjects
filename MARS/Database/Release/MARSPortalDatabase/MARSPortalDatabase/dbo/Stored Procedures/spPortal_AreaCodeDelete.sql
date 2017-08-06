-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_AreaCodeDelete] 
(
	@ownarea VARCHAR(5) = NULL
)
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRY
		DELETE
		FROM AreaCodes
		WHERE ownarea = @ownarea
	END TRY

	BEGIN CATCH
		SELECT -1
	END CATCH

END