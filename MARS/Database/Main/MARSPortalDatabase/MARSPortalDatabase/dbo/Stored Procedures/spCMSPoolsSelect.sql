-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spCMSPoolsSelect]
	
	@country	VARCHAR(10)		= NULL,
	@CAL		AS VARCHAR(1)	= '*'
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		cms_pool_id, cms_pool 
	FROM 
		CMS_POOLS AS cp
	WHERE 
		((country = @country) OR (@country IS NULL))
	-- Altered by Gavin for MarsV3 CAL filtering
	AND (@CAL IN (
					SELECT loc.cal 
					FROM [dbo].CMS_LOCATION_GROUPS AS clg
					JOIN [dbo].LOCATIONS AS loc ON loc.cms_location_group_id = clg.cms_location_group_id
					WHERE cp.cms_pool_id = clg.cms_pool_id)
				OR @CAL = '*'
					) 
	ORDER BY 
		country , cms_pool

END