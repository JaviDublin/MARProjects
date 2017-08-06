-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_ModelCodeDelete] 
(	
	@model_id INT=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DELETE 
	FROM MODELCODES
	WHERE model_id=@model_id

END TRY

BEGIN CATCH
	SELECT -1
END CATCH

END