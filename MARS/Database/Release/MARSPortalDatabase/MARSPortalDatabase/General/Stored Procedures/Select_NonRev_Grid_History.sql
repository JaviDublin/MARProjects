-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_History]
	
		@serial					VARCHAR(30)		= NULL,
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
		CarGroup			VARCHAR(50)		NULL,
		ModelDesc			VARCHAR(255)	NULL,
		Unit				VARCHAR(50)		NULL,
		Plate				VARCHAR(50)		NULL,
		Serial				VARCHAR(30)		NULL,
		NRDays				INT				NULL,
		OperStat			VARCHAR(10)		NULL,
		MoveType			VARCHAR(10)		NULL,
		OwnArea				VARCHAR(10)		NULL,
		RemarkCode			VARCHAR(5)		NULL,
		RemarkText			VARCHAR(50)		NULL,
		NextRent			DATETIME		NULL,
		Remark				VARCHAR(4000)	NULL,
		StartDate			DATETIME		NULL,
		EndDate				DATETIME		NULL,
		IsOpen				BIT				NULL
	)
	
	INSERT INTO @CARSEARCH
	(
		CarGroup , ModelDesc , Unit , Plate , Serial ,
		NRDays , OperStat , MoveType ,  OwnArea , RemarkCode , 
		RemarkText , NextRent , Remark , StartDate ,  EndDate , IsOpen
	)
	
	SELECT 
		FNR.CarGroup	, FNR.ModelDesc , FNR.Unit	,FNR.Plate , FNR.Serial ,
		FNR.NRdays	, FNR.OperStat	, FNR.MoveType	, FNR.OwnArea ,  NRRL.RemarkId	,
		NRRL.RemarkText , FNR.ERDate	, FNR.Remark , FNR.StartDate , FNR.EndDate , 
		FNR.IsOpen
	FROM 
		[General].vw_Fleet_NonRevLog FNR
	
	LEFT JOIN [Settings].NonRev_Remarks_List AS NRRL ON
		FNR.RemarkId = NRRL.RemarkId
	
	WHERE 
		FNR.Serial = @serial
	
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @CARSEARCH
	ORDER BY
	CASE WHEN @sortExpression = 'CarGroup'				THEN CarGroup END ASC,
	CASE WHEN @sortExpression = 'CarGroup DESC'			THEN CarGroup END DESC,
	CASE WHEN @sortExpression = 'ModelDesc'				THEN ModelDesc END ASC,
	CASE WHEN @sortExpression = 'ModelDesc DESC'		THEN ModelDesc END DESC,
	CASE WHEN @sortExpression = 'Unit'					THEN Unit END ASC,
	CASE WHEN @sortExpression = 'Unit DESC'				THEN Unit END DESC,	
	CASE WHEN @sortExpression = 'Plate'					THEN Plate END ASC,
	CASE WHEN @sortExpression = 'Plate DESC'			THEN Plate END DESC,
	CASE WHEN @sortExpression = 'Serial'				THEN Serial END ASC,
	CASE WHEN @sortExpression = 'Serial DESC'			THEN Serial END DESC,	
	CASE WHEN @sortExpression = 'NRDays'				THEN NRDays END ASC,
	CASE WHEN @sortExpression = 'NRDays DESC'			THEN NRDays END DESC,
	CASE WHEN @sortExpression = 'OperStat'				THEN OperStat END ASC,
	CASE WHEN @sortExpression = 'OperStat DESC'			THEN OperStat END DESC,
	CASE WHEN @sortExpression = 'MoveType'				THEN MoveType END ASC,
	CASE WHEN @sortExpression = 'MoveType DESC'			THEN MoveType END DESC,
	CASE WHEN @sortExpression = 'OwnArea'				THEN OwnArea END ASC,
	CASE WHEN @sortExpression = 'OwnArea DESC'			THEN OwnArea END DESC,
	CASE WHEN @sortExpression = 'RemarkCode'			THEN RemarkCode END ASC,
	CASE WHEN @sortExpression = 'RemarkCode DESC'		THEN RemarkCode END DESC,
	CASE WHEN @sortExpression = 'RemarkText'			THEN RemarkText END ASC,
	CASE WHEN @sortExpression = 'RemarkText DESC'		THEN RemarkText END DESC,
	CASE WHEN @sortExpression = 'NextRent'				THEN NextRent END ASC,
	CASE WHEN @sortExpression = 'NextRent DESC'			THEN NextRent END DESC,
	CASE WHEN @sortExpression = 'Remark'				THEN Remark END ASC,
	CASE WHEN @sortExpression = 'Remark DESC'			THEN Remark END DESC,
	CASE WHEN @sortExpression = 'StartDate'				THEN StartDate END ASC,
	CASE WHEN @sortExpression = 'StartDate DESC'		THEN StartDate END DESC,
	CASE WHEN @sortExpression = 'EndDate'				THEN EndDate END ASC,
	CASE WHEN @sortExpression = 'EndDate DESC'			THEN EndDate END DESC,
	CASE WHEN @sortExpression = 'IsOpen'				THEN IsOpen END ASC,
	CASE WHEN @sortExpression = 'IsOpen DESC'			THEN IsOpen END DESC
	
	SELECT 
		CS.CarGroup , CS.ModelDesc ,  CS.Unit , CS.Plate , CS.Serial ,
		CS.NRDays , CS.OperStat , CS.MoveType , CS.OwnArea ,  CS.RemarkCode ,
		CS.RemarkText , CS.NextRent , CS.Remark , CS.StartDate , CS.EndDate , 
		CS.IsOpen
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