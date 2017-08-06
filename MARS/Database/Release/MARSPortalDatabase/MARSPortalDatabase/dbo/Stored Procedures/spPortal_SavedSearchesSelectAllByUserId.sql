-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spPortal_SavedSearchesSelectAllByUserId] 
(
	@userId				VARCHAR(10)=NULL,
	@sortExpression		VARCHAR(50)=NULL,
	@maximumRows		INT=NULL,
	@startRowIndex		INT=NULL
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
		rowId						INT IDENTITY (1,1) NOT NULL,
		searchId					INT NULL,
		searchName					VARCHAR(50) NULL,
		userId						VARCHAR (10) NULL,
		country						VARCHAR(10) NULL,
		cms_pool_id					INT NULL,
		cms_location_group_id		INT NULL,--cms_location_group_code		VARCHAR(10) NULL,
		ops_region_id				INT NULL,
		ops_area_id					INT NULL,
		location					VARCHAR(50) NULL,
		car_segment_id				INT NULL,
		car_class_id				INT NULL,
		car_group_id				INT NULL
	)

	INSERT INTO @QUERYTABLE
		(searchId, searchName, userId , country, cms_pool_id, cms_location_group_id, ops_region_id , 
			ops_area_id , location, car_segment_id , car_class_id , car_group_id )
	SELECT
		searchId, searchName, userId, country, cms_pool_id, cms_location_group_id, ops_region_id , 
			ops_area_id , location, car_segment_id , car_class_id , car_group_id
	FROM dbo.MARS_Users_Saved_Searches
	WHERE userId =@userId 
	
	--Select Results
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @QUERYTABLE
	ORDER BY
	CASE WHEN @sortExpression = 'searchName' THEN searchName END ASC,
	CASE WHEN @sortExpression = 'searchName DESC' THEN searchName END DESC
	
	SELECT
		searchId, searchName, userId, country, cms_pool_id, cms_location_group_id, ops_region_id , 
			ops_area_id , location, car_segment_id , car_class_id , car_group_id
	FROM @QUERYTABLE q
	INNER JOIN @PAGING p ON p.rowId = q.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @QUERYTABLE;

END