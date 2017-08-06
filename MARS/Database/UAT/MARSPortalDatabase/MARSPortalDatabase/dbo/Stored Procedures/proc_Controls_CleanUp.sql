-- =============================================
-- Author:		Anthony McClorey
-- Create date: <Create Date,,>
-- Description:	Clean up control menu after insert / update / delete of items
-- =============================================
CREATE procedure [dbo].[proc_Controls_CleanUp] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--Clean up parent and childern id's
	--This is required so as ASP.Net SQL SiteMapProvider will operate correctly

	DECLARE @controls TABLE
	(
		rowId			INT NULL,
		controlUrl		INT NULL,
		controlId		INT NULL,
		parentId		INT NULL
	)

	INSERT INTO @CONTROLS
		(rowId,controlUrl, ControlId, parentId)
	SELECT
		ROW_NUMBER() OVER (ORDER BY parentId) AS 'RowNumber',ControlUrl, ControlId, ParentId
	FROM Controls
	
	UPDATE Controls SET
		ControlId = q.RowId,
		ParentId = q.ParentId
	FROM Controls c
	INNER JOIN
	(
		SELECT 
		T2.ControlUrl AS 'ControlUrl', T2.rowId AS 'RowId', T1.rowId AS 'ParentId'
		FROM @CONTROLS T1
		INNER JOIN @CONTROLS T2 ON T1.RowId = T2.ParentId	
	)q ON q.ControlUrl = c.ControlUrl
	

END