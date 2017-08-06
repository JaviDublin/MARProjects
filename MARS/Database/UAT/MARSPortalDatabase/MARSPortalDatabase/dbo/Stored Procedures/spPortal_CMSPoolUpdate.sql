-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSPoolUpdate] 
	
	@cms_pool_id int,
	@cms_pool varchar(50), 
	@country varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CMS_POOLS SET  cms_pool = @cms_pool, country = @country

	WHERE cms_pool_id = @cms_pool_id

END