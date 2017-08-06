CREATE procedure [dbo].[proc_MARS_CMS_NecessaryFleet_MultipleUpdate]

	@countryID varchar(3),
	@srcLocationGroupID int,
	@carClassGroupID int,
	@utilisation numeric(5,2),
	@nonRev numeric(5,2)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @sql varchar(2000)
	
    SET @sql = 'UPDATE [MARS_CMS_NECESSARY_FLEET] SET '
    IF (@utilisation <> -1)
		SET @sql = @sql + '[UTILISATION] = ' + CAST(@utilisation AS varchar(10))
	
	IF (@utilisation <> -1 AND @nonRev <> -1)
		SET @sql = @sql + ', '
				
	IF (@nonRev <> -1)	
		SET @sql = @sql + '[NONREV_FLEET] = ' + CAST(@nonRev AS varchar(10))
									
	SET @sql = @sql + ' WHERE [COUNTRY] = ''' + @countryID +''''
 
	IF (@srcLocationGroupID <> -1)
		SET @sql = @sql + ' AND [CMS_LOCATION_GROUP_ID] = ' + CAST(@srcLocationGroupID AS varchar(10))
		
	IF (@carClassGroupID <> -1)
		SET @sql = @sql + ' AND [CAR_CLASS_ID] = ' + CAST(@carClassGroupID AS varchar(3))	
       
    exec (@sql)
END