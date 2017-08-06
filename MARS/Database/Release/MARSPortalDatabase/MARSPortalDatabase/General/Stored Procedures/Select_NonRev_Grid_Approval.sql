-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_Approval]

	@country				VARCHAR(10)		= NULL,
	@daygroup				VARCHAR(100)	= NULL,
	@operstat_name			VARCHAR(300)	= NULL,
	@sortExpression			VARCHAR(50)		= NULL,
	@startRowIndex			INT				= NULL,
	@maximumRows			INT				= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
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
		RemarkText			VARCHAR(50)		NULL,
		Remark				VARCHAR(255)	NULL
	)
	
	INSERT INTO @CARSEARCH
	(
		ModelDesc , Plate , Serial , OperStat , NRdays , 
		RemarkText , Remark 
	)
	
	SELECT
			FNR.ModelDesc , FNR.Plate , FNR.Serial , FNR.OperStat , FNR.NRdays ,
			ISNULL(SUBSTRING(NRRL.RemarkText,1,10),'') + '...' AS [RemarkText], 
			ISNULL(SUBSTRING(FNR.Remark,1,10),'') + '...' AS [Remark]
	FROM 
		[General].vw_Fleet_NonRevLog FNR
	LEFT JOIN [Settings].[NonRev_Remarks_List] AS NRRL ON
		FNR.RemarkId = NRRL.RemarkId
	WHERE
		FNR.IsOpen = 1 
	AND			
		( FNR.CountryCar = @country OR @country IS NULL)
	AND
		((@daygroup	IS NULL)	OR (FNR.DayGroupCode in 
									  (SELECT Items FROM dbo.fSplit(@daygroup,','))))	
	AND 
		((@operstat_name		 IS NULL) OR (FNR.OperStat IN 
											(SELECT Items FROM dbo.fSplit(@operstat_name,','))))
	AND 
		(FNR.Fleet_carsales > 0 OR FNR.Fleet_rac_ttl > 0)
		and FNR.MoveType <> '-'
	ORDER BY 
		FNR.NRdays ASC

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