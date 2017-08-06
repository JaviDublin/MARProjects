-- =============================================
-- Author:		Javier
-- Create date: July 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Export].[GenerateFiles_cmd]
	
	@fileSystem VARCHAR(50)
	
AS
BEGIN
	
	SET NOCOUNT ON;

	IF @fileSystem = 'DEReports'
	BEGIN
		
		DECLARE @today VARCHAR(8) , @command VARCHAR(255)	
		SET @today = right('00' + convert(varchar(2), day(getdate())), 2) + right('00' + convert(varchar(2), month(getdate())), 2) + convert(varchar(4),year(getdate())) + '.csv'
		SET @command = 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "EXEC [Export].[DEReports_Fleet]" -o "\\hescft02\mgrdashtran\mars-' + @today + '.csv" -s"," -W'
		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'

		EXEC master..xp_cmdshell @command
		
		REVERT
	
	END

	ELSE IF @fileSystem = 'FleetDaily'
	BEGIN
		DECLARE @today1   VARCHAR(8),
				@command1 VARCHAR(255)

		SET @today1 =  RIGHT('00' + CONVERT(VARCHAR(2), Month(Getdate())), 2)
					  + CONVERT(VARCHAR(4), Year(Getdate()))
					  + '.csv'
		SET @command1 = 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "EXEC [Export].[FleetDaily]" -o "\\hescft04\marsexport\marsdaily-'
						+ @today1 + '.csv" -s"," -W'

		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'

		EXEC master..Xp_cmdshell
		  @command1 
		
		REVERT
		
	END

	ELSE IF @fileSystem = 'BPserver'
	BEGIN
		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		-- Car Segment
		EXEC master..xp_cmdshell 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "SELECT cs.car_segment_id ,  cs.car_segment , cs.country , cs.sort_car_segment FROM CAR_SEGMENTS cs" -o "\\hescft02\BPServer\MARS_Fleet\Car_Segments.csv" -s"," -w 700 '
		-- Car Groups
		EXEC master..xp_cmdshell 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "SELECT cg.car_group_id , cg.car_group , cg.car_group_gold , cg.car_class_id , cg.sort_car_group FROM CAR_GROUPS cg" -o "\\hescft02\BPServer\MARS_Fleet\Car_Groups.csv" -s"," -w 700 '
		-- Car Classes
		EXEC master..xp_cmdshell 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "SELECT cc.car_class_id , cc.car_class, cc.car_segment_id , cc.sort_car_class FROM CAR_CLASSES cc" -o "\\hescft02\BPServer\MARS_Fleet\Car_Classes.csv" -s"," -w 700 '
		-- Fleet Europe Actual
		EXEC master..xp_cmdshell 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "SELECT DAYSREV,BD,MM,RT,TB,WS SV,VC,COUNTRY,On_rent,overdue,su,operational_fleet,available_fleet,total_fleet,ImportTime FROM FLEET_EUROPE_ACTUAL" -o "\\hescft02\BPServer\MARS_Fleet\Fleet_Actual.csv" -s"," -w 700 '
		-- Fleet Europe Stats
		EXEC master..xp_cmdshell 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "EXEC [Export].[BPServer_Fleet]" -o "\\hescft02\BPServer\MARS_Fleet\Fleet_Stats.csv" -s"," -w 700 '
		REVERT
		
	END
	ELSE IF @fileSystem = 'TransferDesk'
	BEGIN
		-- CrossBorderIdle
		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		
		--EXEC master..xp_cmdshell 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "SELECT * FROM qry_TransferDesk" -o "\\hescft02\TransferDesk\HertzTransfer_Fleet.txt" -s"," -w 700 '
		
		EXEC master..xp_cmdshell 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "EXEC [Export].[TransferDesk_Fleet]" -o "\\hescft02\TransferDesk\HertzTransfer_Fleet.txt" -h-1 -s"," -w 700 '		
		
		REVERT
		
	END

	ELSE IF @fileSystem = 'Delete Daily File'
	BEGIN
		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXEC master..xp_cmdshell 'del \\hescft02\MarsFISdata\RP.FCCRD998.DVLT.EXTRACT-EURO'
		REVERT
	END
	ELSE IF @fileSystem = 'Delete Hourly File'
	BEGIN
		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXEC master..xp_cmdshell 'del \\hescft02\MarsFISdata\RP.FCCRD999.DVLT.EXTRACT-EURO'
		REVERT
	END
	ELSE IF @fileSystem = 'Delete CMS File'
	BEGIN
		DECLARE @file_name VARCHAR(50) , @cmd VARCHAR(255)
		SET @file_name = (SELECT TOP 1 Data1 FROM bcs.BatchControl WHERE ProcessBatch_id = 3 ORDER BY BatchControl_id DESC)
		SET @cmd = 'del \\hescft02\MarsFISdata\' + @file_name
		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXEC master..xp_cmdshell @cmd
		REVERT
	END

	ELSE IF @fileSystem = 'Operational Dashboard'
	BEGIN

		DECLARE @day VARCHAR(8) , @sql VARCHAR(255)	
		SET @day = right('00' + convert(varchar(2), day(getdate())), 2) + right('00' + convert(varchar(2), month(getdate())), 2) + convert(varchar(4),year(getdate())) + '.csv'
		
		SET @sql = 'sqlcmd -S HESCMARS01 -d MARSPortal -E -Q "EXEC [Export].[Operational_Dashboard]" -o "\\hescft04\OperationalControlDashboard\mars-' + @day + '.csv" -s"," -W'
		
		EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		
		EXEC master..xp_cmdshell @sql
		
		REVERT


	END

		
    
END