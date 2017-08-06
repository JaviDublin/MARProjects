-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_SavedSearchesSelectDropDownListAllByUserId] 
(
		@userId	VARCHAR(10)=NULL
)

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT
		searchId, searchName
	FROM
		MARS_Users_Saved_Searches
	WHERE 
		userId =@userId 

END