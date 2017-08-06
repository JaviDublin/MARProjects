-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].Truncate_CMS_Forecast_cle
	
AS
BEGIN
	
	SET NOCOUNT ON;

    TRUNCATE TABLE [MARS_CMS_Forecast_cle]
END