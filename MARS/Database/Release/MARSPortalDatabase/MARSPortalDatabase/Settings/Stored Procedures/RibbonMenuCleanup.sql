
CREATE PROCEDURE [Settings].[RibbonMenuCleanup] 

AS
BEGIN
	
	SET NOCOUNT ON;

	--Clean up parent and childern id's
	--This is required so as ASP.Net SQL SiteMapProvider will operate correctly

	--Table variable to hold initial values
	DECLARE @menus TABLE
	(
		rowId			INT,
		menuId			INT, 
		parentId		INT,
		urlId			INT
	)

	--Get detaills from menu table
	INSERT INTO @menus
	SELECT
		ROW_NUMBER() OVER (ORDER BY parentId) AS 'RowId',MenuId,
		ParentId,UrlId
	FROM Settings.RibbonMenu
	
	--Update the menu table with cleaned up menuid
	UPDATE Settings.RibbonMenu SET
		MenuId=q.rowId,
		ParentId = q.ParentId
	FROM  Settings.RibbonMenu c
	INNER JOIN
	(
		SELECT 
		t2.UrlId AS 'UrlId', t2.rowId AS 'RowId', t1.rowId AS 'ParentId'
		FROM @menus t1
		INNER JOIN @menus t2 ON t1.RowId = t2.ParentId	
	)q ON q.UrlId = c.UrlId;
	

END