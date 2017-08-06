-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CMSPoolSelect] 
(
	@country			VARCHAR(50) =NULL,
	@sortExpression		VARCHAR(50) =NULL,
	@startRowIndex		INT=NULL,
	@maximumRows		INT=NULL
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)	

	DECLARE @QUERYTABLE TABLE
	(
		rowId			INT IDENTITY (1,1) NOT NULL,
		cms_pool_id		INT NULL,
		cms_pool		VARCHAR(50) NULL,
		country			VARCHAR(10) NULL
	)

	INSERT INTO @QUERYTABLE
		(cms_pool_id, cms_pool, country )
	SELECT
		cms_pool_id, cms_pool, country 
	FROM CMS_POOLS 
	WHERE (country = ISNULL(@country,country))

	--Select results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='cms_pool' THEN cms_pool END ASC,
	CASE WHEN @sortExpression ='cms_pool DESC' THEN cms_pool END DESC,
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC

	SELECT 
		cms_pool_id, cms_pool, country 
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;
	
END