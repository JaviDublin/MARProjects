-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_Approval_UserCars_Excel]

	@racfId					VARCHAR(20)     = NULL,
	@approvalDate			VARCHAR(20)		= NULL,
	@countryCar				VARCHAR(10)		= NULL,
	@sortExpression			VARCHAR(50)		= NULL,
	@startRowIndex			INT				= NULL,
	@maximumRows			INT				= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SET @racfId = LTRIM(RTRIM(@racfId))
	
	SET @approvalDate = LTRIM(RTRIM(@approvalDate))
	
	SET @countryCar = LTRIM(RTRIM(@countryCar))
	
	DECLARE @celendarid INT =
	(SELECT dim_Calendar_id FROM [Inp].[dim_Calendar] WHERE Rep_Date =  CONVERT(DATETIME,@approvalDate,103))
	
	
	-- Create OUTOUT Tables
	--=========================================================================================	
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
	
	DECLARE @CARSEARCH TABLE 
	(
		rowId				INT IDENTITY (1,1) NOT NULL,
		ModelDesc			VARCHAR(255)	NULL,
		Plate				VARCHAR(50)		NULL,
		OperStat			VARCHAR(50)		NULL,
		NRDays				INT				NULL,
		Serial				VARCHAR(30)		NULL,
		RemarkText			VARCHAR(1000)	NULL,
		Remark				VARCHAR(4000)	NULL
	)
	
	INSERT INTO @CARSEARCH
	(
		ModelDesc , Plate , Serial , OperStat , NRdays , 
		RemarkText , Remark 
	)
	
	SELECT 
		df.ModelDesc , df.Plate , df.Serial , nrl.OperStat , nrl.NRdays ,
		nrrl.RemarkText		AS [RemarkText], 
		nrla.Remark			AS [Remark]
	FROM 
		[General].[Fact_NonRevLog_Approval] nrla
	INNER JOIN [General].[Dim_Fleet] df on nrla.VehicleId = df.VehicleId
	INNER JOIN [General].[Fact_NonRevLog] nrl on nrla.NonRevLogId = nrl.NonRevLogId
	LEFT JOIN [Settings].[NonRev_Remarks_List] AS nrrl ON nrla.RemarkId = nrrl.RemarkId
	WHERE 
		nrla.RacfId = @racfId and nrla.ApprovalDateId = @celendarid and df.Country = @countryCar
	GROUP BY 
		df.ModelDesc , df.Plate , df.Serial ,  nrl.OperStat , nrl.NRdays ,
		nrrl.RemarkText , nrla.Remark
	ORDER BY nrl.NRdays DESC
	
	
	--IF @sortExpression IS NULL SET @sortExpression = 'NRDays DESC'
	
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @CARSEARCH
	ORDER BY
	CASE WHEN @sortExpression = 'ModelDesc'				THEN ModelDesc END ASC,
	CASE WHEN @sortExpression = 'ModelDesc DESC'		THEN ModelDesc END DESC,
	CASE WHEN @sortExpression = 'Plate'					THEN Plate END ASC,
	CASE WHEN @sortExpression = 'Plate DESC'			THEN Plate END DESC,
	CASE WHEN @sortExpression = 'OperStat'				THEN OperStat END ASC,
	CASE WHEN @sortExpression = 'OperStat DESC'			THEN OperStat END DESC,
	CASE WHEN @sortExpression = 'NRDays'				THEN NRDays END ASC,
	CASE WHEN @sortExpression = 'NRDays DESC'			THEN NRDays END DESC,
	CASE WHEN @sortExpression = 'RemarkText'			THEN RemarkText END ASC,
	CASE WHEN @sortExpression = 'RemarkText DESC'		THEN RemarkText END DESC,
	CASE WHEN @sortExpression = 'Remark'				THEN Remark END ASC,
	CASE WHEN @sortExpression = 'Remark DESC'			THEN Remark END DESC,
	CASE WHEN @sortExpression IS NULL THEN 
		Remark END ASC ,
		NRDays DESC
	
	SELECT 
		CS.Plate ,
		CS.ModelDesc ,  CS.OperStat , CS.NRDays , 
		CS.RemarkText ,  CS.Remark 
	FROM 
		@CARSEARCH CS
	INNER JOIN 
		@PAGING p ON p.rowId = cs.rowId 
	WHERE 
		(p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY 
		p.pageIndex;
		
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @CARSEARCH;


    
END