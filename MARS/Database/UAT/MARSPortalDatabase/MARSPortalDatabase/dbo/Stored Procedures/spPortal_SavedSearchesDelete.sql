-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_SavedSearchesDelete] 
(
	@searchId					INT=NULL
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    DELETE FROM dbo.MARS_Users_Saved_Searches 
	WHERE
		searchId =@searchId 

	-- Return Success
	SELECT 0

END