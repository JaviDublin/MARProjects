-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Update_NonRev_Approval]

	@racfid					VARCHAR(20)		= NULL,
	@country				VARCHAR(10)		= NULL,
	@daygroup				VARCHAR(100)	= NULL,
	@operstat_name			VARCHAR(300)	= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @today DATETIME
	
	SET @today = DATEADD(D,0,DATEDIFF(d,0,GETDATE()))
	
	DECLARE @tableApproval TABLE
	(
		[NonRevLogId]	[int] NULL,
		[VehicleId]		[int] NULL,
		[ApprovalDate]	[datetime] NULL,
		[RacfId]		[varchar](20) NULL,
		[ERDate]		[datetime] NULL,
		[Remark]		[varchar](4000) NULL,
		[RemarkId]		[int] NULL
	)
	
	INSERT INTO @tableApproval
	(
		NonRevLogId , VehicleId , ApprovalDate , RacfId , ERDate , Remark , RemarkId
	)
	SELECT
		FNR.NonRevLogId ,
		FNR.VehicleId   ,
		GETDATE()		, 
		@racfid			, 
		FNR.ERDate		,
	    FNR.Remark		, 
	    FNR.RemarkId
	FROM 
		[General].vw_Fleet_NonRevLog FNR
	LEFT JOIN [Settings].[NonRev_Remarks_List] AS NRRL ON
		FNR.RemarkId = NRRL.RemarkId
	WHERE
		FNR.IsOpen = 1 
	AND			
		( FNR.CountryCar = @country OR @country IS NULL)
	AND
		((@daygroup	IS NULL) OR (FNR.DayGroupCode in 
									(SELECT Items FROM dbo.fSplit(@daygroup,','))))	
	AND 
		((@operstat_name IS NULL) OR (FNR.OperStat IN 
									(SELECT Items FROM dbo.fSplit(@operstat_name,','))))
	
	UPDATE [General].Fact_NonRevLog SET
		IsApproved	 = 1 ,
		RacfId		 = @racfid ,
		ApprovalDate = @today
	
	WHERE
		IsOpen = 1 
	AND	
		VehicleId IN
		(
			SELECT VehicleId FROM 	@tableApproval
		)
		
		
	DELETE FROM 
		[General].[Fact_NonRevLog_Approval]
	WHERE 
		RacfId = @racfid 
	AND 
		DATEADD(D,0,DATEDIFF(D,0,ApprovalDate)) = @today
	AND 
		NonRevLogId IN 
			(
				select NonRevLogId 
				from [General].Fact_NonRevLog 
				where CountryCar = @country
			)
	
		
	INSERT INTO [General].[Fact_NonRevLog_Approval]
	(
		NonRevLogId , VehicleId , ApprovalDate , RacfId , ERDate , Remark , RemarkId
	)
	SELECT
			NonRevLogId , VehicleId , ApprovalDate , RacfId , ERDate , Remark , RemarkId
		FROM 
			@tableApproval
	
		
	UPDATE NRLA SET 
		NRLA.ApprovalDateId = C.dim_Calendar_id
	FROM 
		[General].Fact_NonRevLog_Approval NRLA
	INNER JOIN [Inp].[dim_Calendar] C ON  DATEADD(D,0,DATEDIFF(D,0,NRLA.ApprovalDate)) = C.Rep_Date
	WHERE NRLA.ApprovalDateId IS NULL
	
	

    
END