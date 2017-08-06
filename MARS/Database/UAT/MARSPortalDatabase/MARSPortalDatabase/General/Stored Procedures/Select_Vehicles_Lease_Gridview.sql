
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_Vehicles_Lease_Gridview]
	
	@sortExpression		VARCHAR(50) = NULL,
	@startRowIndex		INT			= NULL,
	@maximumRows		INT			= NULL,
	@country_owner		VARCHAR(2)	= NULL, 
	@country_rent		VARCHAR(2)  = NULL, 
	@start_date			DATETIME	= NULL, 
	@model_description	VARCHAR(2000)= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)	

	DECLARE @QUERYTABLE TABLE
	(
		rowId				INT IDENTITY (1,1) NOT NULL,
		Serial				VARCHAR(25)	,
		Plate				VARCHAR(25)	,
		Unit				VARCHAR(25)	,
		ModelDescription    VARCHAR(255),
		Country_Owner		VARCHAR(2)	,
		Country_Rent		VARCHAR(2)	,
		StartDate			DATETIME	
	)
	
	INSERT INTO @QUERYTABLE
		( Serial , Plate , Unit , ModelDescription ,  Country_Owner , Country_Rent , StartDate )
	SELECT 
		 Serial , Plate , Unit , ModelDescription , Country_Owner , Country_Rent , StartDate
	FROM [General].VEHICLE_LEASE
	WHERE
		(Country_Owner = ISNULL(@country_owner,Country_Owner))
	AND
		(Country_Rent = ISNULL(@country_rent,Country_Rent))
	AND 
		((@model_description IS NULL) OR (ModelDescription IN (SELECT LTRIM(RTRIM(Items)) FROM fSplit(@model_description,','))))
	AND
		(StartDate >= ISNULL(@start_date,StartDate))

	--Select Results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression ='Serial'					THEN Serial END ASC,
	CASE WHEN @sortExpression ='Serial DESC'			THEN Serial END DESC,
	CASE WHEN @sortExpression ='Plate'					THEN Plate END ASC,
	CASE WHEN @sortExpression ='Plate DESC'				THEN Plate END DESC,
	CASE WHEN @sortExpression ='Unit'					THEN Unit END ASC,
	CASE WHEN @sortExpression ='Unit DESC'				THEN Unit END DESC,
	CASE WHEN @sortExpression ='ModelDescription'		THEN ModelDescription END ASC,
	CASE WHEN @sortExpression ='ModelDescription DESC'	THEN ModelDescription END DESC,
	CASE WHEN @sortExpression ='Country_Owner'			THEN Country_Owner END ASC,
	CASE WHEN @sortExpression ='Country_Owner DESC'		THEN Country_Owner END DESC,
	CASE WHEN @sortExpression ='Country_Rent'			THEN Country_Rent END ASC,
	CASE WHEN @sortExpression ='Country_Rent DESC'		THEN Country_Rent END DESC,
	CASE WHEN @sortExpression ='StartDate'				THEN StartDate END ASC,
	CASE WHEN @sortExpression ='StartDate DESC'			THEN StartDate END DESC

	SELECT
		 Serial , Plate , Unit , ModelDescription , Country_Owner , Country_Rent , StartDate 
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;


    
END