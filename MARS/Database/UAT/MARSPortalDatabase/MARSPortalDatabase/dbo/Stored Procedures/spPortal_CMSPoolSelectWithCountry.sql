-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSPoolSelectWithCountry] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     cms_pool_id, '(' + country + ') - ' + cms_pool AS cms_pool
FROM         CMS_POOLS
ORDER BY country, cms_pool
	
	
END