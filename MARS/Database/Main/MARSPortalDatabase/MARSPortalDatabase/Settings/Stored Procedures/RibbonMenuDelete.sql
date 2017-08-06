
CREATE PROCEDURE [Settings].[RibbonMenuDelete]
(
	@urld				INT=NULL,
	@ResultCode			INT OUTPUT

)
AS
BEGIN

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
				
		--Get new ControlId
		DELETE FROM Settings.RibbonMenu WHERE UrlId=@urld

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