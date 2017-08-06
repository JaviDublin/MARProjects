-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spOperstatsSelect_Settings] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		OperationalStatusCode AS [operstat_name]
		, OperationalStatusCode + ' (' + OperationalStatusCode + ')' as operstat_desc
	FROM 
		[Settings].Operational_Status
	ORDER BY 
		OperationalStatusCode

END