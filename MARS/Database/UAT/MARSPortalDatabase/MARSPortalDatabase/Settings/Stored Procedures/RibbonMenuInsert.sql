
CREATE PROCEDURE [Settings].[RibbonMenuInsert]
(
	@ParentId			INT=NULL,
	@Position			INT=NULL,
	@Enabled			BIT=NULL,
	@Title				NVARCHAR(25)=NULL,
	@Description		NVARCHAR(50)=NULL,
	@Url				NVARCHAR(255)=NULL,
	@IconUrl			NVARCHAR(255)=NULL,
	@ResultCode			INT OUTPUT
)
AS
BEGIN

	-------------------------------------
	--Result Codes
	-------------------------------------
	--Success = 0,
	--Failed = -1,
	--Duplicate = 1,
	-- NotAuthenticated = 2,
	--Initialized = -2
	
	SET NOCOUNT ON;
	
	--Set Result code failed
	SET @resultCode = -1

	-- Declare transaction and set to false
	DECLARE @TranStarted   BIT
	SET @TranStarted = 0


	--Check transaction count and Begin Transaction
	IF( @@TRANCOUNT = 0 )
		BEGIN
			BEGIN TRANSACTION -- Start transaction
			SET @TranStarted = 1
		END
	ELSE
			SET @TranStarted = 0
	
	BEGIN	-- (Start of insert)
		
		DECLARE @MenuId INT
				
		--Get new MenuId
		SET @MenuId = (SELECT MAX(UrlId) FROM Settings.RibbonMenu) + 1
		
		IF (@MenuId IS NULL)
			--First Menu Item
			SET @MenuId=1
		
		
		--Check that the menu does not exist
		IF NOT EXISTS(SELECT MenuId FROM Settings.RibbonMenu WHERE MenuId = @MenuId)
			BEGIN
				--Insert the new menu item
				INSERT INTO Settings.RibbonMenu
				(MenuId, ParentId, Url, Position, IconUrl,[Enabled],
					Title,[Description])
				VALUES
				(@MenuId, @ParentId,@Url, @Position, @IconUrl,@Enabled,
					@Title,@Description)	
	
			END
		ELSE
			--Menu Exists Duplicate
			SET @ResultCode = 1
	

		-- Clean Up Controls
		EXEC Settings.RibbonMenuCleanup

		-- Check if error occured --
		IF (@@ERROR <> 0) 
			BEGIN
				SET @resultCode =-1
				GOTO CLEANUP
			END
		ELSE
			--Set Result code success
			SET @resultCode = 0
			
		
	END	-- (End of insert)
	
	
	-- No error Commit transaction --
	IF( @TranStarted = 1 )
	BEGIN
		SET @TranStarted = 0
		COMMIT TRANSACTION
	END

		-- ROLLBACK TRANSACTION --
	CLEANUP:
	IF (@TranStarted = 1)
	BEGIN
		SET @TranStarted = 0
		ROLLBACK TRANSACTION
	END

	-- Select Output Parameters
	SELECT @resultCode AS 'ResultCode';
	
END