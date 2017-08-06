-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarClassSelect] 
(
	@country			VARCHAR(50) =NULL,
	@car_segment_id		INT=NULL,
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
		car_class_id		INT NULL,
		car_class			VARCHAR(50) NULL,
		sort_car_class		INT NULL,
		car_segment_id		INT NULL,
		car_segment			VARCHAR(50) NULL,
		country				VARCHAR(10) NULL
	)
	
	INSERT INTO @QUERYTABLE
		(car_class_id, car_class, sort_car_class, car_segment_id, car_segment,country)
	SELECT 
		c.car_class_id, c.car_class, c.sort_car_class, 
		c.car_segment_id, s.car_segment, s.country
	FROM CAR_CLASSES c
	INNER JOIN CAR_SEGMENTS s on c.car_segment_id = s.car_segment_id
	WHERE 
		(s.country = ISNULL(@country,s.country)) AND
		(s.car_segment_id = ISNULL(@car_segment_id,s.car_segment_id))

	--Select Results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='car_class' THEN car_class END ASC,
	CASE WHEN @sortExpression ='car_class DESC' THEN car_class END DESC,
	CASE WHEN @sortExpression ='sort_car_class' THEN sort_car_class END ASC,
	CASE WHEN @sortExpression ='sort_car_class DESC' THEN sort_car_class END DESC,
	CASE WHEN @sortExpression ='car_segment' THEN car_segment END ASC,
	CASE WHEN @sortExpression ='car_segment DESC' THEN car_segment END DESC,
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC

	SELECT
		car_class_id, car_class, sort_car_class, car_segment_id, car_segment,country
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;

END