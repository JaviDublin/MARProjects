-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSPoolSelectById] 
(
	@cms_pool_id INT = NULL
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

SELECT     cms_pool_id, cms_pool, country
FROM         CMS_POOLS
WHERE cms_pool_id=@cms_pool_id
END