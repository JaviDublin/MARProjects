-- =============================================
-- Author:		Anthony McClorey
-- Create date: <Create Date,,>
-- Description: Select Controls Menu
-- =============================================
CREATE procedure [dbo].[proc_Controls_MenuSelect] 
(
	@UserId UNIQUEIDENTIFIER=NULL
	--@GlobalID VARCHAR(20)=NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--Select the menu
	SELECT c.ControlID AS 'id', c.parentid, c.Url AS 'url', c.Name As 'title', 
		CASE 
			WHEN (c.ImageUrl IS NOT NULL) THEN c.[description] + '|' + c.ImageUrl
			ELSE c.[description] END AS 'description'
	FROM
		Controls c
	WHERE 
		c.isActive =1 AND c.menuEnabled=1
	ORDER BY c.position
END