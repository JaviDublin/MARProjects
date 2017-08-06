CREATE procedure [dbo].[ExportLevelFleetGetAll]
	
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		PKID , [Description] 
	FROM 
		[dbo].[ExportLevelFleet]
END