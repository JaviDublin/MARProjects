CREATE procedure [dbo].[proc_FleetPlanEntryBulkInsert]

	@user varchar(50),
	@originalFileName varchar(50),
	@archiveFileName varchar(500),
	@fleetPlanID int,
	@countryID varchar(3)	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   DECLARE @ErrorCode int    
   SET @ErrorCode = 0
		
	BEGIN TRY
		BEGIN TRANSACTION  
			DECLARE @FleetPlanEntryID INT
			
			SET @FleetPlanEntryID =
				(SELECT
					PKID 
				FROM 
					[dbo].[MARS_CMS_FleetPlanEntry]
				WHERE
				Country = @countryID AND
				fleetPlanID = @fleetPlanID)
			
			IF (@FleetPlanEntryID IS NOT NULL)
			BEGIN
				-- update fleet plan entry file
				UPDATE [dbo].[MARS_CMS_FleetPlanEntry]
				SET 
					uploadedBy = @user,
					uploadedDate = GETDATE(),
					uploadedFileName = @originalFileName
				WHERE
					Country = @countryID AND
					fleetPlanID = @fleetPlanID
			END				
			ELSE
			BEGIN
				INSERT INTO [dbo].[MARS_CMS_FleetPlanEntry]
			   ([Country]
			   ,[fleetPlanID]
			   ,[uploadedBy]
			   ,[uploadedDate]
			   ,[uploadedFileName])
				 VALUES
					   (@countryID,
					   @fleetPlanID,
					   @user,
					   GETDATE(),
					   @originalFileName)
					   
				Set @FleetPlanEntryID = SCOPE_IDENTITY()
				
				-- create archive
				INSERT INTO [dbo].[MARS_CMS_FleetPlanEntryUploadArchive]
				   ([Country]
				   ,[fleetPlanID]
				   ,[uploadedBy]
				   ,[uploadedDate]
				   ,[uploadedFileName]
				   ,[archiveFileName])
				 VALUES
				   (@countryID
				   ,@fleetPlanID
				   ,@user
				   ,GETDATE()
				   ,@originalFileName
				   ,@archiveFileName)
	           
			END	
				--Create FleetPlanDetailsTMP table for bulk insert
				IF (OBJECT_ID('tempdb..#FleetPlanDetailsTMP') IS NULL)
				BEGIN
					CREATE TABLE #FleetPlanDetailsTMP
					(fleetPlanID int, 
					carClassID int, 
					targetLocationGroup int,
					targetDate DateTime, 
					amount int) 
				END
				ELSE
				BEGIN
					DELETE FROM #FleetPlanDetailsTMP
				END
								
				--DELETE existing values
				DELETE FROM [dbo].[MARS_CMS_FleetPlanDetails]
				WHERE
					FleetPlanEntryID = 
						(SELECT PKID FROM MARS_CMS_FleetPlanEntry
						WHERE
						Country = @countryID AND
						fleetPlanID = @fleetPlanID)
						
				
				--bulk insert 
				DECLARE @bulkInsertSQL varchar(1000)
				BEGIN
					SET @bulkInsertSQL = 
					'BULK INSERT #FleetPlanDetailsTMP 
					FROM ''' + @archiveFileName +
					''' WITH 
					( 
						MAXERRORS = 0, 
						FIELDTERMINATOR = '','', 
						ROWTERMINATOR = ''\n'' 
					)'
				END
				
				EXEC (@bulkInsertSQL)
								
				-- negative count	
				Insert into [dbo].[MARS_CMS_FleetPlanDetails]
				   ([fleetPlanEntryID]
				   ,[targetDate]
				   ,[car_class_id]
				   ,[cms_Location_Group_ID]
				   ,[amount])
					
					Select 
						fleetPlanID, 
						targetDate,					
						carClassID, 
						targetLocationGroup,
						amount 
					FROM 
					#FleetPlanDetailsTMP
					WHERE
					amount > 0
					
				--positive count	
				Insert into [dbo].[MARS_CMS_FleetPlanDetails]
				   ([fleetPlanEntryID]
				   ,[targetDate]
				   ,[car_class_id]
				   ,[cms_Location_Group_ID]
				   ,[amount])

				 Select 
					fleetPlanID, 
					targetDate,					
					carClassID, 
					targetLocationGroup, 
		 			ABS(amount) 
				FROM 
				#FleetPlanDetailsTMP
				WHERE
				amount < 0
										 
				-- Finished, Drop temp table
				Drop Table #FleetPlanDetailsTMP
			
		COMMIT
	END TRY
	BEGIN CATCH
	  -- An Error occurred, roll back
	  IF @@TRANCOUNT > 0
		 ROLLBACK

		DECLARE @ErrNumber int, @ErrMsg nvarchar(4000), @ErrSeverity int, @ErrProcedure nvarchar(50), @ErrLine int, @ErrState int
		SELECT @ErrNumber = ERROR_NUMBER(), @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY(), @ErrProcedure = ERROR_PROCEDURE(), @ErrLine = ERROR_LINE(), @ErrState = ERROR_STATE()
		-- Log error details
		EXEC [dbo].[SqlLogCreate] @ErrNumber, @ErrMsg, @ErrSeverity, @ErrProcedure, @ErrLine, @ErrState
		-- Raise the error
		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH
END