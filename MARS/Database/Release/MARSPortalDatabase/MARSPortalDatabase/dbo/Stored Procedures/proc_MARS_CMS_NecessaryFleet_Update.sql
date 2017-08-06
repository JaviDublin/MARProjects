CREATE procedure [dbo].[proc_MARS_CMS_NecessaryFleet_Update]

	@countryID varchar(3),
	@srcLocationGroupID int,
	@carClassGroupID int,
	@utilisation numeric(5,2),
	@nonRev numeric(5,2)
AS
BEGIN

	SET NOCOUNT ON;

    UPDATE [MARS_CMS_NECESSARY_FLEET]
				SET [UTILISATION] = @utilisation 
				,[NONREV_FLEET] = @nonRev
				WHERE [COUNTRY] = @countryID
			    AND [CMS_LOCATION_GROUP_ID] = @srcLocationGroupID
				AND [CAR_CLASS_ID] = @carClassGroupID
END