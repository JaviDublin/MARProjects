-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Update_NonRevRemarks]
	
		@serial			VARCHAR(30)		= NULL,
		@remark_code	VARCHAR(3)		= NULL,
		@remark			VARCHAR(4000)   = NULL,
		@er_date		DATETIME		= NULL
		--@er_date		VARCHAR(30)		= NULL
		,@RemarkRacfid  VARCHAR(20)     = NULL
		
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @vehicle_id INT = (SELECT VehicleId FROM [General].Dim_Fleet WHERE Serial = @serial)
	
	DECLARE @nonrevlog_id INT = 
								(	SELECT NonRevLogId 
									FROM [General].Fact_NonRevLog 
									WHERE VehicleId = @vehicle_id AND IsOpen = 1
								)
								
	UPDATE [General].Fact_NonRevLog  SET
		RemarkId = CONVERT(INT,@remark_code),
		Remark   = @remark,
		ERDate   = @er_date ,
		--ERDate   = CONVERT(DATETIME,@er_date,103),
		RemarkIdDate = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
		,RemarkRacfid = @RemarkRacfid
		
	WHERE
		NonRevLogId = @nonrevlog_id

    
END