-- =============================================
-- Description: Internal procedure to write Log entry, used from SQL Jobs/SPROC's
-- =============================================
CREATE PROCEDURE [dbo].[SqlLogCreate] 

	/* Executing stored procedure name, ie procedure where the error occured. */
	@ErrorCode		INT = NULL,
	@ErrorMessage	VARCHAR(4000) = NULL,
	@ErrorSeverity	INT = NULL,
	@ErrorProcedure	VARCHAR(50) = 'Unknown',
	@ErrorLine		INT = NULL,
	@ErrorState		INT = NULL
	
AS
BEGIN
	
	DECLARE @body1 VARCHAR(4000)
	IF(@ErrorProcedure IS NULL)
		SET @ErrorProcedure = 'Unknown'
		
	INSERT INTO SqlLog
	([LogTime], [ErrorCode], [ErrorMessage], [ErrorSeverity], [ErrorProcedure], [ErrorLine], [ErrorState])
	Values
	(GETUTCDATE(), @ErrorCode, @ErrorMessage, @ErrorSeverity, @ErrorProcedure, @ErrorLine, @ErrorState)

	BEGIN TRY
	  SET @body1 = @ErrorMessage
	  EXEC msdb.dbo.sp_send_dbmail 
		@recipients		= 'ITServiceDesk@hertz.com', 
		@subject		= 'DB error!!!', 
		@body			= @body1, 
		@body_format	= 'HTML'
	END TRY
	BEGIN CATCH
	END CATCH

END