-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	see job MARS_Maintenace_Year [Delete information of 2 years ago]
-- =============================================
CREATE PROCEDURE [System].[Delete_CMS_Forecast_History]
	
AS
BEGIN
	
	SET NOCOUNT ON;

    DELETE FROM [MARS_CMS_FORECAST_HISTORY] 
    WHERE YEAR(REP_DATE) < YEAR(GETDATE()) - 2
END