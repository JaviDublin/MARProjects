-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSPoolInsert] 
(	
	@cms_pool	VARCHAR(50)= NULL, 
	@country	VARCHAR(10)= NULL
)
AS
BEGIN
	
	SET NOCOUNT ON;

    
	INSERT INTO CMS_POOLS 
	(cms_pool, country)
	VALUES( @cms_pool, @country )

	-- Return Success
	SELECT 0

END