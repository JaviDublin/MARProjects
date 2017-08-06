CREATE procedure [dbo].[proc_MarsLog_ClearLogs]
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM Mars_Logging_CategoryLog
	DELETE FROM [Mars_Log]
    DELETE FROM Mars_Logging_Category
END