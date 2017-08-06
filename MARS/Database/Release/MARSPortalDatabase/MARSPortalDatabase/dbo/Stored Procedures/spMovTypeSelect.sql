-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	Non Rev part
-- =============================================
CREATE PROCEDURE [dbo].[spMovTypeSelect]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		MovementTypeCode 
	FROM 
		[Settings].Movement_Types 
	WHERE 
		MovementTypeName <> 'Unknown' or MovementTypeName is null
	ORDER BY 
		MovementTypeCode
    
END