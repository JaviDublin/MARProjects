-- =============================================
-- Description:	Get the details of last import depending on tool used
-- =============================================
CREATE procedure [dbo].[spGetLastImportTimeByImportType]
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
		importTimeStamp
	FROM
		DATA_IMPORT_INFORMATION
	WHERE
		importTypeId=@importTypeId
END