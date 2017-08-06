-- =============================================
-- Author:		Javier
-- Create date: June 2012 
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Update_Import_Data_Tables]
		
		@Type	varchar(20) ,	
		@comment			VARCHAR(300) = NULL	
AS
BEGIN
	
	SET NOCOUNT ON;

 
	DECLARE @importDate DATETIME
	SET		@importDate = GETDATE()

	DECLARE @recordsImported INT
	SET		@recordsImported = (SELECT COUNT(*) FROM FLEET_EUROPE_OKCFILE)

	UPDATE dbo.DATA_IMPORT_INFORMATION SET 
		importTimeStamp		= @importDate,
		nextUpdateDue		= DATEADD(hh,1,@importDate)
	WHERE
		importTypeId = 1
		
	-- Once a day clear data older than a month from the INFO table
	IF @Type = 'Daily'
	BEGIN
		DELETE 
		FROM 
			dbo.DATA_IMPORT_DETAILS
		WHERE
			importTypeId=1 
		AND 
			DATEDIFF(day, importTimeStamp, @importDate) > 30
	END	
		
	-- Insert into Details Table
		INSERT INTO dbo.DATA_IMPORT_DETAILS
		(importTypeId, importTimeStamp , importTypeIsDaily ,recordsImported ,comment )
		select 
			1,	
			@importDate, 
			case when @Type = 'Daily' then 1 else 0 end, 
			@recordsImported, 
			@comment
 
END