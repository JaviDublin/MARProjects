CREATE procedure [dbo].[proc_LocationGroupGetByCountryID]
	@countryID varchar(3)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [CMS_LOCATION_GROUPS].[cms_location_group]
      ,[CMS_LOCATION_GROUPS].[cms_pool_id]
      ,[CMS_LOCATION_GROUPS].[cms_location_group_id]
  FROM [dbo].[CMS_LOCATION_GROUPS]
  INNER JOIN [dbo].[CMS_POOLS] ON [CMS_LOCATION_GROUPS].[cms_pool_id] = [CMS_POOLS].[cms_pool_id]
  WHERE [CMS_POOLS].[country] = @countryID
  ORDER BY [CMS_LOCATION_GROUPS].[cms_location_group]
END