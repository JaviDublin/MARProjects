-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spSSISArchiveToHistoryTable]
AS
BEGIN
--	=============================================
--	This routine archives data older than 30 days from dbo.FLEET_EUROPE_SUMMARY into dbo.FLEET_EUROPE_SUMMARY_HISTORY
--
--	Functionality:
--		
--		1.) Archive data older than 30 days
--		2.) Remove the data from the main summary table
--	==============================================

	SET NOCOUNT ON;

	--Archive data older than 30 days
	INSERT INTO dbo.FLEET_EUROPE_SUMMARY_HISTORY
	SELECT 
		* 
	FROM 
		dbo.FLEET_EUROPE_SUMMARY
	WHERE 
		(REP_DATE < DATEADD(d, -30, DATEDIFF(d, 0, GETDATE())))

	--Remove the data from the main summary table
	DELETE FROM dbo.FLEET_EUROPE_SUMMARY
	WHERE 
		(REP_DATE < DATEADD(d, -30, DATEDIFF(d, 0, GETDATE())))
	
END