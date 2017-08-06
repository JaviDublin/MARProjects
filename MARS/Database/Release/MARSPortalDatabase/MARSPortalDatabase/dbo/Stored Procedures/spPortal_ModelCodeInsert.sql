-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_ModelCodeInsert] 
(	
	@country	VARCHAR(10)=NULL,
	@model		VARCHAR(50)=NULL,
	@active		BIT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO dbo.MODELCODES
	(country, model, active)
	VALUES
	(@country, @model, @active)

	-- Return Success
	SELECT 0

END