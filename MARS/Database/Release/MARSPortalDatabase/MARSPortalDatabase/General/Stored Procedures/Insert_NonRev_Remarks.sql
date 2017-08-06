-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Insert_NonRev_Remarks]
	
		@serial		VARCHAR(30)		= NULL,
		@remarkId	VARCHAR(3)		= NULL,
		@remark		VARCHAR(4000)   = NULL,
		@erdate		VARCHAR(30)		= NULL
		,@RemarkRacfid  VARCHAR(20)     = NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @vehicleId INT
	
	SET @vehicleId = 
					(
						SELECT 
							TOP 1 VehicleId 
						FROM 
							[General].Dim_Fleet 
						WHERE 
							Serial = @serial 
						ORDER BY 
							VehicleId DESC
					)
	
	DECLARE @NonRevlogId INT
	
	SET @NonRevlogId = 
					(
						SELECT 
							TOP 1 NonRevLogId 
						FROM 
							[General].[Fact_NonRevLog]
						WHERE
							VehicleId = @vehicleId
						AND 
							IsOpen = 1
						ORDER BY
							NonRevLogId DESC
					)
							
							
	
	UPDATE [General].[Fact_NonRevLog] SET
		Remark		 = @remark , 
		RemarkId	 = ISNULL(CONVERT(INT,@remarkId),1),
		ERDate       = CONVERT(DATETIME , @erdate , 103),
		RemarkIdDate = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
		,RemarkRacfid = @RemarkRacfid
	WHERE
		NonRevLogId = @NonRevlogId
	
	

   
END