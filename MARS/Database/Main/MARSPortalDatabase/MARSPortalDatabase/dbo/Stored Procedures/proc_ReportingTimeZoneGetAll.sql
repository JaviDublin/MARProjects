CREATE procedure [dbo].[proc_ReportingTimeZoneGetAll]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT zoneID, zoneDescription FROM [dbo].[CMS_Reporting_Time_Zone]
END