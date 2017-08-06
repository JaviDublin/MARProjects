-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_ModelCodeSelect] 
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
		model_id			INT NULL,
		country				VARCHAR(10) NULL,
		model				VARCHAR(50) NULL,	
		active				BIT NULL
	)
	
	INSERT INTO @QUERYTABLE
		(model_id, country, model, active)
	SELECT 
		model_id, country, model, active
	FROM dbo.MODELCODES
	WHERE
		(country = ISNULL(@country,country))

	--Select Results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='country' THEN country END ASC,
	CASE WHEN @sortExpression ='country DESC' THEN country END DESC,
	CASE WHEN @sortExpression ='model' THEN model END ASC,
	CASE WHEN @sortExpression ='model DESC' THEN model END DESC,
	CASE WHEN @sortExpression ='active' THEN active END ASC,
	CASE WHEN @sortExpression ='active DESC' THEN active END DESC

	SELECT
		model_id, country, model, active
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;

END