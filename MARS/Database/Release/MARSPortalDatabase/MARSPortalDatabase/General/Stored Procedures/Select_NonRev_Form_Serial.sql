-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Form_Serial]

	@plate	VARCHAR(30)		= NULL

AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT TOP 1 
		Serial
	FROM 
		[General].[Dim_Fleet]
	WHERE 
		Plate = @plate
	
	


END