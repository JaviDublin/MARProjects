-- =============================================
-- Author:		Gavin Williams
-- Create date: 11-12-12
-- Description:	The control for the generation of the Forecast export to be exported to \\hescft02\EURawData\GEMARS
-- =============================================
CREATE PROCEDURE [Export].[GEForecastController]

AS 
BEGIN
	SET NOCOUNT ON;

declare
	@logEntry int = 0
	,@errorCode int = 0
	,@cmd varchar(500)

exec [Export].[GenerateGEForecast]

SELECT TOP 1 @logEntry = [Id], @errorCode = ErrorLog_Id
FROM [Export].[Log]
order by Id desc

if @errorCode is null
begin
	set @cmd='sqlcmd -S HESCMARSQ01 -d MARSPortal -E -Q "exec Export.GEForecast" -o '
	set @cmd+='"\\hescft02\EURawData\GEMARS\GEForecastData'
	--set @cmd+='"C:\TEMP\GEForecastData'
	set @cmd+=cast(@logEntry as varchar(5))
	set @cmd+='.csv" -s"," '
	EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
	EXEC master..xp_cmdshell @cmd

	INSERT INTO [Export].[Log]
		   ([Message],[DateTime])
	 VALUES('[Export].[GenerateGEForecast] automated upload end - Successful.',GETDATE())
end

END