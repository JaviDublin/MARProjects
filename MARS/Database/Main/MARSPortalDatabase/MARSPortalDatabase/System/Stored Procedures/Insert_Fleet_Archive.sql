-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	see job MARS_Maintenace_Year [Archive information of 2 years ago]
-- =============================================
CREATE PROCEDURE [System].[Insert_Fleet_Archive]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO FLEET_EUROPE_SUMMARY_ARCHIVE
	
	SELECT * FROM FLEET_EUROPE_SUMMARY_HISTORY 
	WHERE YEAR(REP_DATE) < YEAR(GETDATE()) - 2

    
END