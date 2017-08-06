
-- moves to Fact_NonRevLog_DailyReport_History all rows older then 10 days
CREATE PROCEDURE [General].[NonRev_Review_DailyReport_Ensure]
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (
			SELECT [FileProcess_id], [Entity], [FileName], [Status], [Data], [z_inserted], [z_updated]
			FROM [MARSPortal].[bcs].[FileProcess]
			WHERE [z_inserted] > convert(DATE, GETDATE())
				AND [Entity] = 'Mars_Daily'
				AND [Status] = 'Complete'
			)
	BEGIN
		RAISERROR ('The daily file has not been loaded.', 16, 1)
	END
END