CREATE procedure [dbo].[proc_FleetPlanEntryUploadArchiveCreate] 
	
	@user varchar(50),
	@originalFileName varchar(50),
	@archiveFileName varchar(500),
	@fleetPlanID int,
	@countryID varchar(3),
	@isAddition	 bit = 1
AS
BEGIN
	
	SET NOCOUNT ON;
	
    -- create archive
	IF NOT EXISTS
	( 
		SELECT * FROM [dbo].[MARS_CMS_FleetPlanEntryUploadArchive] 
		WHERE [fleetPlanID] = @fleetPlanID AND	
		[Country] = @countryID AND
		[isaddition] = @isAddition
	)
	BEGIN
		INSERT INTO [dbo].[MARS_CMS_FleetPlanEntryUploadArchive]
		   ([Country]
		   ,[fleetPlanID]
		   ,[uploadedBy]
		   ,[uploadedDate]
		   ,[uploadedFileName]
		   ,[archiveFileName]
		   ,[isaddition])
		 VALUES
		   (@countryID
		   ,@fleetPlanID
		   ,@user
		   ,GETDATE()
		   ,@originalFileName
		   ,@archiveFileName,
		   @isAddition)	
	END
	ELSE
	BEGIN
		UPDATE [dbo].[MARS_CMS_FleetPlanEntryUploadArchive]
		   SET [uploadedBy] = @user
			  ,[uploadedDate] = GETDATE()
			  ,[uploadedFileName] = @originalFileName
			  ,[archiveFileName] = @archiveFileName
			WHERE 
				[fleetPlanID] = @fleetPlanID AND	
				[Country] = @countryID AND
				[isaddition] = @isAddition
	END
END