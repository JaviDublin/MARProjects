-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_CountrySelect] 
(
	@sortExpression	VARCHAR(50) =NULL,
	@startRowIndex INT=NULL,
	@maximumRows INT=NULL
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
		country				VARCHAR(10) NULL,
		country_dw			VARCHAR(50) NULL,
		country_description	VARCHAR(50) NULL,
		active				BIT
	)
	
	INSERT INTO @QUERYTABLE
		(country, country_dw, country_description, active)
	SELECT 
		 country, country_dw, country_description, active 
	FROM COUNTRIES 

	-- Select Records
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC,
	CASE WHEN @sortExpression ='country_dw' THEN country_dw END ASC,
	CASE WHEN @sortExpression ='country_dw DESC' THEN country_dw END DESC,
	CASE WHEN @sortExpression ='country_description' THEN country_description END ASC,
	CASE WHEN @sortExpression ='country_description DESC' THEN country_description END DESC,
	CASE WHEN @sortExpression ='active' THEN active END ASC,
	CASE WHEN @sortExpression ='active DESC' THEN active END DESC

	SELECT 
		country, country_dw, country_description, active 
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;

END