-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_ModelCodeUpdate] 
(	
	@model_id	INT=NULL,
	@country	VARCHAR(10)=NULL,
	@model		VARCHAR(50)=NULL,
	@active		BIT=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE MODELCODES SET
	country = @country,
	model=@model,
	active =@active
	WHERE
	model_id = @model_id

END