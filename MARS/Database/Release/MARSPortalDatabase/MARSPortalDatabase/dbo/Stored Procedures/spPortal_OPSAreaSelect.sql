-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_OPSAreaSelect] 
(
	@country			VARCHAR(50) =NULL,
	@ops_region_id		INT=NULL,
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
		ops_area_id		INT NULL,
		ops_area		VARCHAR(50) NULL,
		ops_region_id	INT NULL,		
		ops_region		VARCHAR(50) NULL,
		country			VARCHAR(10) NULL
	)

	INSERT INTO @QUERYTABLE
		(ops_area_id, ops_area, ops_region_id, ops_region, country)
	SELECT ops_area_id, ops_area, a.ops_region_id, 
			 r.ops_region , r.country
	FROM OPS_AREAS a
		INNER JOIN OPS_REGIONS r on a.ops_region_id = r.ops_region_id
	WHERE 
		(r.country = ISNULL(@country,r.country)) AND
		(r.ops_region_id = ISNULL(@ops_region_id,r.ops_region_id))

	--Select Records
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='ops_area' THEN ops_area END ASC,
	CASE WHEN @sortExpression ='ops_area DESC' THEN ops_area END DESC,
	CASE WHEN @sortExpression ='ops_region' THEN ops_region END ASC,
	CASE WHEN @sortExpression ='ops_region DESC' THEN ops_region END DESC,
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC
	
	SELECT
		ops_area_id, ops_area, ops_region_id, ops_region, country
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;
	
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;

END