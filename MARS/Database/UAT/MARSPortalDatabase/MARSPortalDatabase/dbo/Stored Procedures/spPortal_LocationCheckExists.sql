-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_LocationCheckExists] 
(
	@location VARCHAR(50)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


IF NOT EXISTS(SELECT location 
				FROM LOCATIONS where location = @location)	
	BEGIN
		SELECT 0
	END
ELSE
	SELECT -1

END