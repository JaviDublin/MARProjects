-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Form]

	@serial	VARCHAR(30)		= NULL

AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT 
		TOP 1
		FNR.CarGroup	, FNR.Model		, FNR.ModelDesc , FNR.Unit		, FNR.Serial	,
		FNR.Lstwwd		, FNR.LstDate	, FNR.Duewwd	, FNR.DueDate	, FNR.NRdays	,
		FNR.OperStat	, FNR.MoveType	, FNR.LstMlg	, FNR.LstNo		, FNR.DrvName	,
		FNR.IDate		, FNR.MSODate	, FNR.SDate		, FNR.DepStat	, FNR.CarHold	,
		FNR.OwnArea		, FNR.Prevwwd	, FNR.BDDays	, FNR.MMDays	, NRRL.RemarkId	,
		NRRL.RemarkText , FNR.ERDate	, FNR.Remark	, FNR.Plate
	FROM 
		[General].vw_Fleet_NonRevLog FNR
	
	LEFT JOIN [Settings].NonRev_Remarks_List AS NRRL ON
		FNR.RemarkId = NRRL.RemarkId
	
	WHERE 
		FNR.Serial = @serial
	AND 
		IsOpen = 1


END