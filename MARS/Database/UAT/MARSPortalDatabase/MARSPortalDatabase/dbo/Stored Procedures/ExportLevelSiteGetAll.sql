CREATE procedure [dbo].[ExportLevelSiteGetAll]
	
	
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		PKID, [Description] 
	FROM 
		[dbo].[ExportLevelSite]
END