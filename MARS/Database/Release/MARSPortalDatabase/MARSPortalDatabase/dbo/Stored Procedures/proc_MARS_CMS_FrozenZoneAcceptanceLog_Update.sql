CREATE procedure [dbo].[proc_MARS_CMS_FrozenZoneAcceptanceLog_Update] 
	@user varchar(50),
	@year varchar(10),
	@countryID varchar(3),	
	@weekNumber int 
AS
BEGIN

	SET NOCOUNT ON;

    
	IF NOT EXISTS
	( 
		SELECT * FROM [dbo].[MARS_CMS_FrozenZoneAcceptanceLog] 
		WHERE 	
		[Country] = @countryID AND
		[acceptedWeekNumber] = @weekNumber AND
		[YEAR] = @year
	)
	BEGIN

	INSERT INTO 
	[dbo].[MARS_CMS_FrozenZoneAcceptanceLog]
			   ([Country]
			   ,[Year]
			   ,[acceptedBy]
			   ,[acceptedDate]
			   ,[acceptedWeekNumber])
		 VALUES
			   (@countryID
				,@year
			   ,@user
			   ,GETDATE()
			   ,@weekNumber)
	END
	ELSE
	BEGIN
		UPDATE [dbo].[MARS_CMS_FrozenZoneAcceptanceLog]
		   SET [acceptedBy] = @user
			  ,[acceptedDate] = GETDATE()
			WHERE 
			[Country] = @countryID AND
			[acceptedWeekNumber] = @weekNumber AND
			[YEAR] = @year
	END
           
           
END