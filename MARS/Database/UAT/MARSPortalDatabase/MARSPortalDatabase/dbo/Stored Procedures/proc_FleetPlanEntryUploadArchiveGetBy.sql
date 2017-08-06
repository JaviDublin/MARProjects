CREATE procedure [dbo].[proc_FleetPlanEntryUploadArchiveGetBy]
	@countryID varchar(3)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT FPA.[PKID]
      ,C.[country_description]
      ,FP.[planDescription]
      ,FPA.[uploadedBy]
      ,FPA.[uploadedDate]
      ,FPA.[uploadedFileName]
      ,FPA.[archiveFileName]
      ,FPA.[isAddition]
	FROM [dbo].[MARS_CMS_FleetPlanEntryUploadArchive] FPA
	INNER JOIN COUNTRIES C on FPA.Country = C.country
	INNER JOIN CMS_Fleet_Plans FP on FPA.[fleetPlanID] = FP.planID 
	WHERE
	C.Country = @countryID 
END