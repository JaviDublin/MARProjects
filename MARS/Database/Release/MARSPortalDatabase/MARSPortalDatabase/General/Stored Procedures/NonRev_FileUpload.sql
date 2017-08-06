-- =============================================
-- Author:		Javier
-- Create date: February 2013
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[NonRev_FileUpload]

AS
BEGIN
	
	SET NOCOUNT ON;
	
	CREATE TABLE #NonRev_Serial 
	(
		Country VARCHAR(5) , Serial VARCHAR(20) , ExpectedResolutionDate VARCHAR(20),
		ReasonCode VARCHAR(255) , Comments VARCHAR(4000) , VehicleId INT , RemarkId INT , 
		NonRevLogId INT
	)
	
	DECLARE @country	VARCHAR(5) , 
			@serial		VARCHAR(20) , 
			@erDate		VARCHAR(20) ,
			@reasonCode	VARCHAR(255) , 
			@comments	VARCHAR(4000)
	
	DECLARE @min_rowId INT = (SELECT MIN(RowId) FROM [General].[Import_NonRevFile])
	DECLARE @max_rowId INT = (SELECT MAX(RowId) FROM [General].[Import_NonRevFile])
	DECLARE @row_text VARCHAR(4000)

	CREATE TABLE #table_temp (rowid INT IDENTITY, rowText varchar(4000))	

	
	WHILE @min_rowId <= @max_rowId
	BEGIN
		SET @row_text = (SELECT RowText FROM [General].[Import_NonRevFile] WHERE RowId = @min_rowId)
	
		INSERT INTO #table_temp (rowText)
		SELECT Items FROM dbo.fSplit(@row_text, ',')
	
		SET @country	= (SELECT rowText FROM #table_temp WHERE rowid = 1)
		SET @serial		= (SELECT rowText FROM #table_temp WHERE rowid = 2)
		SET @erDate		= (SELECT rowText FROM #table_temp WHERE rowid = 3)
		SET @reasonCode = (SELECT rowText FROM #table_temp WHERE rowid = 4)
		SET @comments	= (SELECT rowText FROM #table_temp WHERE rowid = 5)
		
		
		INSERT INTO #NonRev_Serial 
		(Country , Serial , ExpectedResolutionDate , ReasonCode , Comments)
		VALUES 
		(@country , @serial , @erDate , @reasonCode , @comments)
		
		TRUNCATE TABLE #table_temp
	
		SET @min_rowId = @min_rowId + 1
	
	END
	
	UPDATE #NonRev_Serial SET
		VehicleId = df.VehicleId
	FROM #NonRev_Serial nrs
	INNER JOIN [General].[Dim_Fleet] df ON nrs.Serial =  df.Serial AND nrs.Country = df.Country
	
	UPDATE #NonRev_Serial SET
		RemarkId = rl.RemarkId
	FROM #NonRev_Serial nrs
	INNER JOIN [Settings].[NonRev_Remarks_List] rl ON nrs.ReasonCode = rl.RemarkText
	
	UPDATE #NonRev_Serial SET
		NonRevLogId = nrl.NonRevLogId
	FROM #NonRev_Serial nrs
	INNER JOIN [General].[Fact_NonRevLog] nrl ON nrs.VehicleId  = nrl.VehicleId AND nrl.IsOpen = 1
	
	DELETE FROM #NonRev_Serial WHERE 
		VehicleId IS NULL OR RemarkId IS NULL OR NonRevLogId IS NULL
	
	UPDATE [General].[Fact_NonRevLog] SET
		RemarkId		= nrs.RemarkId , 
		Remark			= nrs.Comments ,
		ERDate			= CONVERT(DATETIME , nrs.ExpectedResolutionDate,103),
		RemarkIdDate	= DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
	FROM [General].[Fact_NonRevLog] nrl
	INNER JOIN #NonRev_Serial nrs ON nrl.NonRevLogId = nrs.NonRevLogId
	
	UPDATE [General].[Fact_NonRevLog_DailyReport] SET
		TotalFleetNR_Remark = ISNULL(r.Total,0)
	FROM 
		[General].[Fact_NonRevLog_DailyReport] dr
	INNER JOIN
		(
		SELECT 
			nrl.Lstwwd , nrl.OperStat , nrl.DayGroupCode , f.CarGroup ,nrl.RemarkIdDate , 
			COUNT(*) as [Total]
		FROM [General].Fact_NonRevLog nrl
		INNER JOIN [General].Dim_Fleet f on nrl.VehicleId = f.VehicleId
		WHERE 
			nrl.RemarkId > 1 AND nrl.IsOpen = 1
		GROUP BY 
			nrl.Lstwwd , nrl.OperStat , nrl.DayGroupCode , f.CarGroup ,nrl.RemarkIdDate 
		) r ON 
			dr.LocationCode = r.Lstwwd 
		AND dr.OperStat		= r.OperStat 
		AND dr.DayGroupCode = r.DayGroupCode 
		AND dr.CarGroup		= r.CarGroup 
		AND dr.Rep_Date		= r.RemarkIdDate
			
	DROP TABLE #table_temp
	
	DROP TABLE #NonRev_Serial
	
END