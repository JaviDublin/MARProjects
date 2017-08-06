-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CarGroupSelect] 
(
	@country			VARCHAR(50) =NULL,
	@car_class_id		INT=NULL,
	@sortExpression		VARCHAR(50) =NULL,
	@startRowIndex		INT=NULL,
	@maximumRows		INT=NULL,
	@_carClass as varchar(50) = null -- added by Gavin for MarsV3	
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
		car_group_id		INT NULL,
		car_group			VARCHAR(3) NULL,
		car_group_gold		VARCHAR(3) NULL,
		car_group_fivestar				VARCHAR(3) NULL,
		car_group_presidentCircle		VARCHAR(3) NULL,
		car_group_platinum				VARCHAR(3) NULL,
		sort_car_group		INT NULL,
		car_class_id		INT NULL,
		car_class			VARCHAR(50) NULL,
		country				VARCHAR(50) NULL
	)

	INSERT INTO @QUERYTABLE
		(car_group_id, car_group, car_group_gold,car_group_fivestar,car_group_presidentCircle,car_group_platinum, sort_car_group, car_class_id, car_class, country)
	SELECT 
		g.car_group_id, g.car_group, g.car_group_gold, g.car_group_fivestar, g.car_group_presidentCircle, g.car_group_platinum, g.sort_car_group, g.car_class_id, 
		 c.car_class, s.country
	FROM CAR_GROUPS g
	INNER JOIN CAR_CLASSES c on g.car_class_id = c.car_class_id
	INNER JOIN CAR_SEGMENTS s on c.car_segment_id = s.car_segment_id
	WHERE 
		(s.country = ISNULL(@country,s.country)) AND
		(g.car_class_id = ISNULL(@car_class_id,g.car_class_id))
		and (c.car_class = @_carClass or @_carClass is null) -- altered by Gavin for MarsV3 19-4-12
	
	--Select Results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='car_group' THEN car_group END ASC,
	CASE WHEN @sortExpression ='car_group DESC' THEN car_group END DESC,
	CASE WHEN @sortExpression ='car_group_gold' THEN car_group_gold END ASC,
	CASE WHEN @sortExpression ='car_group_gold DESC' THEN car_group_gold END DESC,
	CASE WHEN @sortExpression ='car_group_fivestar' THEN car_group_gold END ASC,
	CASE WHEN @sortExpression ='car_group_fivestar DESC' THEN car_group_gold END DESC,
	CASE WHEN @sortExpression ='car_group_presidentCircle' THEN car_group_gold END ASC,
	CASE WHEN @sortExpression ='car_group_presidentCircle DESC' THEN car_group_gold END DESC,
	CASE WHEN @sortExpression ='car_group_platinum' THEN car_group_gold END ASC,
	CASE WHEN @sortExpression ='car_group_platinum DESC' THEN car_group_gold END DESC,
	CASE WHEN @sortExpression ='sort_car_group' THEN sort_car_group END ASC,
	CASE WHEN @sortExpression ='sort_car_group DESC' THEN sort_car_group END DESC,
	CASE WHEN @sortExpression ='car_class' THEN car_class END ASC,
	CASE WHEN @sortExpression ='car_class DESC' THEN car_class END DESC,
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC

	SELECT
		car_group_id, car_group, car_group_gold, car_group_fivestar, car_group_presidentCircle, car_group_platinum, sort_car_group, car_class_id, car_class, country
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;
	

END