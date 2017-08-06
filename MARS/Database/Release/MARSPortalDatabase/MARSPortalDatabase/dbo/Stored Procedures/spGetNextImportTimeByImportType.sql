-- =============================================
-- Description:	Get the details of next import depending on tool used
-- =============================================
CREATE procedure [dbo].[spGetNextImportTimeByImportType]
(
	@importTypeId INT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here	
	SELECT 
		nextUpdateDue
	FROM
		DATA_IMPORT_INFORMATION
	WHERE
		importTypeId=@importTypeId
END