-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_AreaCodeSelect] 
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
		ownarea			VARCHAR(5) NULL,
		country			VARCHAR(10) NULL,
		area_name		VARCHAR(50) NULL,
		opco			BIT,
		fleetco			BIT,
		carsales		BIT,
		licensee		BIT
	)

	INSERT INTO @QUERYTABLE
		(ownarea, country, area_name, opco, fleetco, carsales, licensee)
	SELECT
		 ownarea, country, area_name, opco, fleetco, carsales, licensee
	FROM AreaCodes
	WHERE (country = ISNULL(@country,country))
	
	--Select results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='ownarea' THEN ownarea END ASC,
	CASE WHEN @sortExpression ='ownarea DESC' THEN ownarea END DESC,
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC,
	CASE WHEN @sortExpression ='area_name' THEN area_name END ASC,
	CASE WHEN @sortExpression ='area_name DESC' THEN area_name END DESC,
	CASE WHEN @sortExpression ='opco' THEN opco END ASC,
	CASE WHEN @sortExpression ='opco DESC' THEN opco END DESC,
	CASE WHEN @sortExpression ='fleetco' THEN fleetco END ASC,
	CASE WHEN @sortExpression ='fleetco DESC' THEN fleetco END DESC,	
	CASE WHEN @sortExpression ='carsales' THEN carsales END ASC,
	CASE WHEN @sortExpression ='carsales DESC' THEN carsales END DESC,
	CASE WHEN @sortExpression ='licensee' THEN licensee END ASC,
	CASE WHEN @sortExpression ='licensee DESC' THEN licensee END DESC


	SELECT 
		ownarea, country, area_name, opco, fleetco, carsales, licensee
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;

END