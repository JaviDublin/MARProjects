-- =============================================
-- Author:		Anthony McClorey
-- Create date: <Create Date,,>
-- Description:	Insert usage Statistics for MARS
-- =============================================
CREATE procedure [dbo].[spMARS_UsageStatisticsInsert] 
(
	@reportId					INT =NULL,
	@country					VARCHAR(50)=NULL,
	@cms_pool_id				INT=NULL,
	@cms_location_group_id		int = NULL,		--@cms_location_group_code	VARCHAR(50)=NULL,
	@ops_region_id				INT=NULL,
	@ops_area_id				INT=NULL,
	@location					VARCHAR(50)=NULL,
	@racfID						VARCHAR(50)=NULL,
	@report_Date				DATETIME=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Declare transaction and set to false
	DECLARE @TranStarted   BIT
	SET @TranStarted = 0

	--Check transaction count and Begin Transaction
	IF( @@TRANCOUNT = 0 )
		BEGIN
			BEGIN TRANSACTION
			SET @TranStarted = 1
		END
	ELSE
		SET @TranStarted = 0

	--Insert statistics	
	BEGIN

		INSERT INTO dbo.MARS_USAGE_STATISTICS
		(
			reportId,
			country,
			cms_pool_id,
			cms_location_group_id,
			ops_region_id,
			ops_area_id,
			location,
			racfId,
			report_date
		)
		VALUES
		(
			@reportId,
			@country,
			@cms_pool_id,
			@cms_location_group_id,
			@ops_region_id,
			@ops_area_id,
			@location,
			@racfId,
			@report_date
		)

		-- Error Occurred --
		IF (@@ERROR <> 0) 
		GOTO CLEANUP

	END

	-- No error Commit transaction --
	IF( @TranStarted = 1 )
		BEGIN
			SET @TranStarted = 0
			COMMIT TRANSACTION
		END

	-- Return 0 Success
	SELECT 0

	-- Rollback transaction --
	CLEANUP:
	IF( @TranStarted = 1 )
		BEGIN
			SET @TranStarted = 0
			ROLLBACK TRANSACTION
		END
	-- Failed
	SELECT -1
		
END