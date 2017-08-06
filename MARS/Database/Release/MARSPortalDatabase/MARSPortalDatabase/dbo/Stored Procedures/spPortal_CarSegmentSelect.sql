-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarSegmentSelect] 
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
		rowId				INT IDENTITY (1,1) NOT NULL,
		car_segment_id		INT NULL,
		car_segment			VARCHAR(50) NULL,
		sort_car_segment	INT NULL,
		country				VARCHAR(10) NULL
	)

	INSERT INTO @QUERYTABLE
		(car_segment_id, car_segment, sort_car_segment, country)
	SELECT
		car_segment_id, car_segment, sort_car_segment, country
	FROM CAR_SEGMENTS 
	WHERE (country = ISNULL(@country,country))

	--Select Results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='car_segment' THEN car_segment END ASC,
	CASE WHEN @sortExpression ='car_segment DESC' THEN car_segment END DESC,
	CASE WHEN @sortExpression ='sort_car_segment' THEN sort_car_segment END ASC,
	CASE WHEN @sortExpression ='sort_car_segment DESC' THEN sort_car_segment END DESC,
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC

	SELECT
		car_segment_id, car_segment, sort_car_segment, country
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;
	
END