CREATE procedure [dbo].[proc_MARS_CMS_FrozenZoneAcceptanceLog_GetByCountry]

	@countryID varchar(3)
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT FZ.[PKID]
      ,FZ.[Country]
      ,FZ.[Year]
      ,FZ.[acceptedBy]
      ,FZ.[acceptedDate]
      ,FZ.[acceptedWeekNumber]
	FROM [dbo].[MARS_CMS_FrozenZoneAcceptanceLog] FZ	
	WHERE
	[Country] = @countryID
	ORDER BY [Year] desc, [acceptedWeekNumber] desc, [acceptedDate]
END