-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSRegionSelect] 
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
		ops_region_id	INT NULL,
		ops_region		VARCHAR(50) NULL,			
		country			VARCHAR(10) NULL
	)

	INSERT INTO @QUERYTABLE
		(ops_region_id, ops_region, country )
	SELECT ops_region_id, ops_region, country 
	FROM ops_regions 
	WHERE (country = ISNULL(@country,country))

	-- Select Results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='ops_region' THEN ops_region END ASC,
	CASE WHEN @sortExpression ='ops_region DESC' THEN ops_region END DESC,
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC

	SELECT
	ops_region_id, ops_region, country 
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;
	
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;

END