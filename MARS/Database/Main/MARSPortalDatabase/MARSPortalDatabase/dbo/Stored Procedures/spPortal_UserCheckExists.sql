-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_UserCheckExists] 
(
	@racfId VARCHAR(10)
)
AS
BEGIN

	SET NOCOUNT ON

	IF NOT EXISTS(SELECT racfid FROM MARS_Users WHERE racfId = @racfid)	
	BEGIN
		SELECT 0
	END
	ELSE
	BEGIN
		SELECT -1
	END

END