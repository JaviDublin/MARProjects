CREATE procedure [dbo].[proc_Controls_Insert]
(
	@ParentId			INT=NULL,
	@Position			INT=NULL,
	@IsActive			BIT=NULL,
	@HelpEnabled		BIT=NULL,
	@MenuEnabled		BIT=NULL,
	@PermissionsEnabled	BIT=NULL,
	@Name				VARCHAR(25)=NULL,
	@Description		VARCHAR(100)=NULL,
	@ImageUrl			VARCHAR(100)=NULL,
	@ControlId			INT OUTPUT,
	@ControlUrl			INT OUTPUT,
	@ResultCode INT OUTPUT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--Set Result code success
	SET @resultCode = 0

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
				
		--Get new ControlId
		SET @ControlId = (SELECT MAX(ControlUrl) FROM Controls ) + 1
		
		--Check that the control does not exist
		IF NOT EXISTS(SELECT ControlId FROM Controls WHERE ControlId = @controlId)
			BEGIN
	
				INSERT INTO Controls
				(ControlId, ParentId, Position, IsActive, HelpEnabled, MenuEnabled,
				PermissionsEnabled,name,[description],imageUrl)
				VALUES
				(@ControlId, @ParentId, @Position, @IsActive, @HelpEnabled, @MenuEnabled,
				@PermissionsEnabled,@name,@description,@imageUrl)
	
				SET @controlUrl  = SCOPE_IDENTITY()		
	
			END
		ELSE
			--Control Exists Duplicate
			SET @ResultCode = 1
	

		-- Clean Up Controls
		EXEC dbo.proc_Controls_CleanUp

		-- Check if error occured --
		IF (@@ERROR <> 0) 
			BEGIN
				SET @resultCode =-1
				GOTO CLEANUP
			END
			
		
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
	SELECT @resultCode AS 'ResultCode',@ControlId AS 'ControlId',@ControlUrl AS 'ControlUrl'
	
END