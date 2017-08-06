
CREATE PROCEDURE Export.DashboardData
as
BEGIN
	SET NOCOUNT ON;
		declare @command varchar(2000)
		declare @queryFile varchar(2000)
		declare @outFile varchar(200)

		set @queryFile = 'C:\temp\DashboardQueries\FeaOnRent.sql'
		set @outFile = 'c:\temp\FeaOnRentOut'

		SET @command = 'sqlcmd -S . -d MarsPortal -E -s, -W -i '+ @queryFile  + ' | findstr /V /C:"-" /B > '+ @outFile + '.csv'
		--EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXECUTE AS LOGIN = 'raddbuser'
		EXEC master..xp_cmdshell @command

		
		set @queryFile = 'C:\temp\DashboardQueries\FleetAdjustments.sql'
		set @outFile = 'c:\temp\FleetAdjustmentsOut'

		SET @command = 'sqlcmd -S . -d MarsPortal -E -s, -W -i '+ @queryFile  + ' | findstr /V /C:"-" /B > '+ @outFile + '.csv'
		--EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXECUTE AS LOGIN = 'raddbuser'
		EXEC master..xp_cmdshell @command

		set @queryFile = 'C:\temp\DashboardQueries\FleetOnPeak.sql'
		set @outFile = 'c:\temp\FleetOnPeakOut'

		SET @command = 'sqlcmd -S . -d MarsPortal -E -s, -W -i '+ @queryFile  + ' | findstr /V /C:"-" /B > '+ @outFile + '.csv'
		--EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXECUTE AS LOGIN = 'raddbuser'
		EXEC master..xp_cmdshell @command

		set @queryFile = 'C:\temp\DashboardQueries\Forecasting.sql'
		set @outFile = 'c:\temp\ForecastingOut'

		SET @command = 'sqlcmd -S . -d MarsPortal -E -s, -W -i '+ @queryFile  + ' | findstr /V /C:"-" /B > '+ @outFile + '.csv'
		--EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXECUTE AS LOGIN = 'raddbuser'
		EXEC master..xp_cmdshell @command

		set @queryFile = 'C:\temp\DashboardQueries\Reservations.sql'
		set @outFile = 'c:\temp\ReservationsOut'

		SET @command = 'sqlcmd -S . -d MarsPortal -E -s, -W -i '+ @queryFile  + ' | findstr /V /C:"-" /B > '+ @outFile
		--EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
		EXECUTE AS LOGIN = 'raddbuser'
		EXEC master..xp_cmdshell @command
END