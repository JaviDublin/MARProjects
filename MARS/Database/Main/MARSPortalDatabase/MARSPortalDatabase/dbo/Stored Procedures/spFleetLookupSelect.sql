-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spFleetLookupSelect]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		fleet_name 
	FROM 
		FLEET_LOOKUP

END