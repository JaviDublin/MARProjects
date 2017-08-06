-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_Approval_User]

		@country				VARCHAR(10)		= NULL,
		@customStartDate		VARCHAR(30)		= NULL,
		@customEndDate			VARCHAR(30)		= NULL,
		@dateComparison			VARCHAR(30)		= NULL,
		@dateRange				VARCHAR(50)		= NULL,
		@dateRangeValue		    VARCHAR(20)		= NULL,
	
		@sortExpression			VARCHAR(50)		= NULL,
		@startRowIndex			INT				= NULL,
		@maximumRows			INT				= NULL
	
AS
BEGIN

-- work--------------
--select * from [General].Fact_NonRevLog where IsOpen = 1
--select * from [General].[Fact_NonRevLog_Approval]
--truncate table [General].[Fact_NonRevLog_Approval]

--select 
--	fnrla.RacfId , DATEADD(D,0,DATEDIFF(d,0,fnrla.ApprovalDate)) , df.Country , COUNT(*)
--from [General].[Fact_NonRevLog_Approval] fnrla
--inner join [General].[Dim_Fleet] df on fnrla.VehicleId = df.VehicleId
--group by fnrla.RacfId , DATEADD(D,0,DATEDIFF(d,0,fnrla.ApprovalDate)) , df.Country

	
	SET NOCOUNT ON;
	
	-- Region : Set the Dates
	--=========================================================================================
	
	DECLARE @calendar_ids TABLE (dim_calendar_id INT)
	
	DECLARE @start_calendar_id INT, @end_calendar_id INT
	 
	IF @customStartDate = '01/01/0001 00:00:00' BEGIN SET @customStartDate	= NULL END
	IF @customEndDate	= '01/01/0001 00:00:00' BEGIN SET @customEndDate	= NULL END
	IF @dateComparison	= '01/01/0001 00:00:00' BEGIN SET @dateComparison	= NULL END
	
	IF @customStartDate IS NULL AND @customEndDate IS NULL AND @dateComparison IS NULL
	BEGIN
		SET @dateComparison = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
	END
	
	DECLARE @day_diff INT
	
	IF @dateRange IS NULL				BEGIN SET @day_diff = 0		END
	IF @dateRange = '1 day'				BEGIN SET @day_diff = 0		END
	IF @dateRange = 'Previous 7 days'	BEGIN SET @day_diff = 6		END
	IF @dateRange = 'Previous 30 days'	BEGIN SET @day_diff = 29	END
	IF @dateRange = 'Previous 90 days'	BEGIN SET @day_diff = 89	END
	IF @dateRange = 'Custom'			BEGIN SET @day_diff = 0		END  -- Initial Value
	
	DECLARE @theDateStart DATETIME
	DECLARE @theDateEnd   DATETIME
		
	IF @dateComparison IS NULL
	BEGIN
		SET @theDateStart = CONVERT(DATETIME,@customStartDate,103)
		SET @theDateEnd   = CONVERT(DATETIME,@customEndDate,103) 
		SET @day_diff	  = DATEDIFF(D,@theDateStart,@theDateEnd)
	END
	ELSE
	BEGIN
		SET @theDateEnd		= CONVERT(DATETIME,@dateComparison,103)
		SET @theDateStart   = @theDateEnd - @day_diff 
	END
	
	SET @start_calendar_id = (SELECT dim_Calendar_id FROM Inp.dim_Calendar WHERE Rep_Date = @theDateStart)
	
	SET @end_calendar_id   = (SELECT dim_Calendar_id FROM Inp.dim_Calendar WHERE Rep_Date = DATEADD(D,0,DATEDIFF(D,0,@theDateEnd)))
	

	INSERT INTO @calendar_ids (dim_calendar_id)
	SELECT 
		dim_Calendar_id 
	FROM 
		Inp.dim_Calendar 
	WHERE 
		dim_Calendar_id BETWEEN @start_calendar_id AND @end_calendar_id
			

	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
	
	DECLARE @APPROVALS TABLE 
	(
		rowId				INT IDENTITY (1,1) NOT NULL,
		racfid				VARCHAR(20),
		approvalDate		DATETIME,
		country				VARCHAR(20),
		TotalVehicles		INT
	)
	
	INSERT INTO @APPROVALS
	(
		racfid , approvalDate , country , TotalVehicles
	)
	
	SELECT 
		fnra.RacfId , DATEADD(D,0,DATEDIFF(D,0,fnra.ApprovalDate)) AS [ApprovalDate],
		df.Country AS [CountryCar], COUNT(*) AS [TotalApproved]
	FROM [General].[Fact_NonRevLog_Approval] fnra
	INNER JOIN [General].[Dim_Fleet] df on fnra.VehicleId = df.VehicleId
	WHERE 
		fnra.ApprovalDateId in (SELECT dim_calendar_id FROM @calendar_ids) 
	AND 
		((@country IS NULL)	OR(df.Country = @country))
	GROUP BY 
		fnra.RacfId , DATEADD(D,0,DATEDIFF(D,0,fnra.ApprovalDate)) , df.Country

	
	

	INSERT INTO @PAGING (rowId) SELECT rowId FROM @APPROVALS
	ORDER BY
	CASE WHEN @sortExpression = 'racfid'			 THEN racfid END ASC,
	CASE WHEN @sortExpression = 'racfid DESC'		 THEN racfid END DESC,
	CASE WHEN @sortExpression = 'approvalDate'		 THEN approvalDate END ASC,
	CASE WHEN @sortExpression = 'approvalDate DESC'	 THEN approvalDate END DESC,
	CASE WHEN @sortExpression = 'country'			 THEN country END ASC,
	CASE WHEN @sortExpression = 'country DESC'		 THEN country END DESC,
	CASE WHEN @sortExpression = 'TotalVehicles'		 THEN TotalVehicles END ASC,
	CASE WHEN @sortExpression = 'TotalVehicles DESC' THEN TotalVehicles END DESC
	
	SELECT 
		CS.racfid ,
		CS.approvalDate ,  CS.country , CS.TotalVehicles 
	FROM 
		@APPROVALS CS
	INNER JOIN 
		@PAGING p ON p.rowId = cs.rowId 
	WHERE 
		(p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY 
		p.pageIndex;
		
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @APPROVALS;


    
END