-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spCountriesSelect] 
		
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		DISTINCT country 
	FROM 
		COUNTRIES 
	WHERE 
		active = 1
END