-- =============================================
-- Author:		Javier
-- Create date: September 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Import].[Execute_CMS_ProcessControl]

AS
BEGIN

	SET NOCOUNT ON;

	EXEC [bcs].[s_ProcessControl]
	@ProcessBatchName = 'CMS_Forecast' ,
	@MaxLoop = 4,
	@RunCmd  = 1
END