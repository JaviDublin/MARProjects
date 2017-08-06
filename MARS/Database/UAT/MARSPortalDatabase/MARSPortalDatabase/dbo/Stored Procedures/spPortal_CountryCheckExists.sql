-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CountryCheckExists] 
(
	@country VARCHAR(10)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


IF NOT EXISTS(SELECT country 
				FROM COUNTRIES where country = @country)	
	BEGIN
		SELECT 0
	END
ELSE
	SELECT -1

END