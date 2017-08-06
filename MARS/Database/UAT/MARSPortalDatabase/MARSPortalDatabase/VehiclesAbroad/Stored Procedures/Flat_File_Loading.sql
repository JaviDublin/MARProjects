-- =============================================
-- Author:		Gavin Williams
-- Create date: 12-8-12
-- Description:	Test to run SSIS package that loads a pre-built package
--				that loads a test flat file into the Reservation_Europe_Staging
--				the test flat file is in I:\Reservation_Test
-- =============================================
CREATE PROCEDURE [VehiclesAbroad].[Flat_File_Loading]

AS
BEGIN
	SET NOCOUNT ON;
	declare @cmd Varchar(4000) = 'dtexec /f I:\"SSIS packages repository"\FlatFile_Load_Reservation_Data.dtsx'
	DECLARE @returncode int
	EXECUTE AS LOGIN = 'DIRDUB01\RAD_DBUSER'
	EXEC @returncode = master..xp_cmdshell @cmd
	select @returncode
	
	-- Return values
	-- 0 	The package executed successfully.
	-- 1 	The package failed.
	-- 3 	The package was cancelled by the user.
	-- 4 	The utility was unable to locate the requested package. The package could not be found.
	-- 5 	The utility was unable to load the requested package. The package could not be loaded.
	-- 6 	The utility encountered an internal error of syntactic or semantic errors in the command line.
	-- \\hescft02\MarsFISdata\Temp\Reservation Data
END