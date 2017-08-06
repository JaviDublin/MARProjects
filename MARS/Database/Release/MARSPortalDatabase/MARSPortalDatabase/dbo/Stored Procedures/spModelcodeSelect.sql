-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spModelcodeSelect] 
	-- Add the parameters for the stored procedure here	
	@country VARCHAR(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MODEL AS 'model' FROM MODELCODES 
	WHERE (COUNTRY = @country) AND
			(ACTIVE=1)
		
	ORDER BY MODEL
END