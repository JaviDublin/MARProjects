-- =============================================
-- Author:		Anthony McClorey
-- Create date: <Create Date,,>
-- Description:	Select Usage Statistics
-- =============================================
CREATE procedure [dbo].[spMARS_UsageStatisticsByDate] 
(
	@marsTool					INT=NULL,
	@logic						INT=NULL,
	@country					VARCHAR(50)=NULL,
	@cms_pool_id				INT=NULL,
	@cms_location_group_id		INT=NULL,--@cms_location_group_id	VARCHAR(50)=NULL,
	@ops_region_id				INT=NULL,
	@ops_area_id				INT=NULL,
	@location					VARCHAR(50)=NULL,
	@start_date					DATETIME=NULL,
	@end_date					DATETIME=NULL,
	@sortExpression				VARCHAR(50)=NULL,	
	@maximumRows				INT=NULL,
	@startRowIndex				INT=NULL,
	@racfId						VARCHAR(50) = NULL
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @EuropeTotals VARCHAR(50)
	SET @EuropeTotals = 'EUROPE - TOTALS'
	DECLARE @Totals VARCHAR(50)
	SET @Totals = ' - TOTALS'
	DECLARE @rowCount	INT
	SET @rowCount = 0
	DECLARE @colName VARCHAR(50)
	SET @colName = NULL

	-- Create a table variable to hold temp query table
	BEGIN

		DECLARE @QUERYTABLE TABLE
		(
	 		reportId				INT NULL,
			reportName				VARCHAR(50) NULL,
			country					VARCHAR(50) NULL,
			cms_pool_id				INT NULL,
			cms_location_group_id	INT NULL,--cms_location_group_id VARCHAR(50) NULL,
			ops_region_id			INT NULL,
			ops_area_id				INT NULL,
			location				VARCHAR(50) NULL,
			racfId					VARCHAR(50) NULL,
			report_date				DATETIME NULL
		)

		DECLARE @PAGING TABLE
		(	
			pageIndex		INT IDENTITY (1,1) NOT NULL,
			rowId			INT NULL
		)
	END
	
	If (@marsTool = 4) -- Pooling Tool
		BEGIN -- Start Off Pooling Tool

			BEGIN -- Create a table variable to retrieve results
				DECLARE @STATISTICSP TABLE
				(
					rowId				INT IDENTITY (1,1) NOT NULL,
					header				VARCHAR(50) NULL,
					alerts				INT NULL,
					status				INT NULL,
					siteComparison		INT NULL,
					fleetComparison		INT NULL,
					reservations		INT NULL
				)

				DECLARE @STATISTICSPTOTALS TABLE
				(
					header				VARCHAR(50) NULL,
					alerts				INT NULL,
					status				INT NULL,
					siteComparison		INT NULL,
					fleetComparison		INT NULL,
					reservations		INT NULL
				)
			
			END	


			IF (@logic = 1) -- CMS Logic
				BEGIN -- Start of CMS Logic
					
					BEGIN -- Get Results to query from
							
							INSERT INTO @QUERYTABLE (reportId, reportName, country, cms_pool_id, cms_location_group_id, location, racfid, report_date)
							SELECT marsReportId, marsReportName, country, cms_pool_id, cms_location_group_id, location, racfid, report_date
							FROM dbo.vw_MARS_Usage_Statistics 
							WHERE ((marsToolId=4) AND (report_date BETWEEN @start_date AND @end_date) AND ((@racfid IS NULL) OR (racfid = @racfid)))
					
							-- Results
							INSERT INTO @STATISTICSP (header)
							SELECT DISTINCT CONVERT(VARCHAR(10), report_date, 103)AS report_date FROM @QUERYTABLE
						
					END
					-- Select All of Europe
					IF ((@country IS NULL) AND (@cms_pool_id IS NULL) AND (@cms_location_group_id IS NULL) AND (@location IS NULL))		
						BEGIN
						
							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header
						

							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN

									-- Totals
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (@EuropeTotals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1)),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)),0)
									WHERE header = @EuropeTotals
				
								END
						END
					

					-- Select CMS_Pools
					IF ((@country IS NOT NULL) AND (@cms_pool_id IS NULL) AND (@cms_location_group_id IS NULL) AND (@location IS NULL))		
						BEGIN
													
							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0),fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SET @colName = @Country
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1) AND (country =@country)),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)  AND (country =@country)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)  AND (country =@country)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)  AND (country =@country)),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)  AND (country =@country)),0)
									WHERE header = (@colName + @Totals)						
								END
						END
					

					-- Select CMS_Location_groups
					IF ((@country IS NOT NULL) AND (@cms_pool_id IS NOT NULL) AND (@cms_location_group_id IS NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0),fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header

							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SELECT @colName = cms_pool FROM dbo.CMS_POOLS WHERE cms_pool_id = @cms_pool_id
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1) AND (cms_pool_id =@cms_pool_id )),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)  AND (cms_pool_id =@cms_pool_id)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)  AND (cms_pool_id =@cms_pool_id)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)  AND (cms_pool_id =@cms_pool_id)),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)  AND (cms_pool_id =@cms_pool_id)),0)
									WHERE header = (@colName + @Totals)		
								
								END
						END


					-- Select Locations
					IF ((@country IS NOT NULL) AND (@cms_pool_id IS NOT NULL) AND (@cms_location_group_id IS NOT NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0),fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header

							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SELECT @colName = cms_location_group FROM dbo.CMS_LOCATION_GROUPS WHERE cms_location_group_id = @cms_location_group_id
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1) AND (cms_location_group_id=@cms_location_group_id)),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)  AND (cms_location_group_id=@cms_location_group_id)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)  AND (cms_location_group_id=@cms_location_group_id)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)  AND (cms_location_group_id=@cms_location_group_id)),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)  AND (cms_location_group_id=@cms_location_group_id)),0)
									WHERE header = (@colName + @Totals)		
								
								END
						END

				END -- End of CMS Logic


			IF (@logic = 2) -- OPS Logic
				BEGIN -- Start of OPS Logic
					
					BEGIN -- Get results to query

							INSERT INTO @QUERYTABLE (reportId, reportName, country, ops_region_id, ops_area_id, location, racfid, report_date)
							SELECT marsReportId, marsReportName, country, ops_region_id, ops_area_id, location, racfid, report_date
							FROM dbo.vw_MARS_Usage_Statistics 
							WHERE ((marsToolId=4) AND (report_date BETWEEN @start_date AND @end_date) AND ((@racfid IS NULL) OR ( racfid = @racfid)) )
							
							-- Results
							INSERT INTO @STATISTICSP (header)
							SELECT DISTINCT CONVERT(VARCHAR(10), report_date, 103)AS report_date FROM @QUERYTABLE				
					END

					-- Select All of Europe
					IF ((@country IS NULL) AND (@ops_region_id IS NULL) AND (@ops_area_id IS NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header
						
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN									
									-- Totals
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (@EuropeTotals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1)),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)),0)
									WHERE header = @EuropeTotals
				
								END
									
						END
					
					-- Select OPS_Regions
					IF ((@country IS NOT NULL) AND (@ops_region_id IS NULL) AND (@ops_area_id IS NULL) AND (@location IS NULL))		
						BEGIN
							
							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0),fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN				
									-- Totals
									SET @colName = @Country		
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1) AND (country =@country)),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)  AND (country =@country)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)  AND (country =@country)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)  AND (country =@country)),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)  AND (country =@country)),0)
									WHERE header = (@colName + @Totals)						
								END
						END

					-- Select OPS_Areas
					IF ((@country IS NOT NULL) AND (@ops_region_id IS NOT NULL) AND (@ops_area_id IS NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0),fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SELECT @colName = ops_region FROM dbo.OPS_REGIONS WHERE ops_region_id = @ops_region_id
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1) AND (ops_region_id =@ops_region_id)),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)  AND (ops_region_id =@ops_region_id)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)  AND (ops_region_id =@ops_region_id)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)  AND (ops_region_id =@ops_region_id)),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)  AND (ops_region_id =@ops_region_id)),0)
									WHERE header = (@colName + @Totals)		
								
								END

						END

					-- Select Locations
					IF ((@country IS NOT NULL) AND (@ops_region_id IS NOT NULL) AND (@ops_area_id IS NOT NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0),fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
							FROM @STATISTICSP s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSP
							IF (@rowCount >=1)
								BEGIN
									--Totals
									SELECT @colName = ops_area FROM dbo.OPS_AREAS WHERE ops_area_id = @ops_area_id 
									INSERT INTO @STATISTICSPTOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSPTOTALS SET 
									alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1) AND (ops_area_id = @ops_area_id )),0),
									status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)  AND (ops_area_id = @ops_area_id )),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)  AND (ops_area_id = @ops_area_id )),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)  AND (ops_area_id = @ops_area_id )),0),
									reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)  AND (ops_area_id = @ops_area_id )),0)
									WHERE header = (@colName + @Totals)		
								
								END
						END
					
				END -- End of OPS Logic


			-- Return a Single Location
			IF (@logic IS NULL)
				BEGIN 
					
					INSERT INTO @QUERYTABLE (reportId, reportName, location, racfid, report_date)
					SELECT marsReportId, marsReportName, location, racfid, report_date
					FROM dbo.vw_MARS_Usage_Statistics 
					WHERE ((marsToolId=4) AND (report_date BETWEEN @start_date AND @end_date) AND (location =@location) AND ((@racfid IS NULL) OR ( racfid = @racfid)))
					
					-- Results
					INSERT INTO @STATISTICSP (header)
					SELECT DISTINCT CONVERT(VARCHAR(10), report_date, 103)AS report_date FROM @QUERYTABLE				

					UPDATE @STATISTICSP SET alerts =ISNULL(a.totals,0),status=ISNULL(su.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),reservations=ISNULL(r.totals,0)
					FROM @STATISTICSP s
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =1) GROUP BY CONVERT(varchar(10), report_date, 103))a ON a.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =2) GROUP BY CONVERT(varchar(10), report_date, 103))su ON su.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =3) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =4) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =5) GROUP BY CONVERT(varchar(10), report_date, 103))r ON r.report_date = s.header
					
					--Report Totals Only run if we have records
					SELECT @rowCount = COUNT(*) FROM @STATISTICSP
						IF (@rowCount >=1)
							BEGIN
								--Totals
								SELECT @colName = @location
								INSERT INTO @STATISTICSPTOTALS (header) VALUES (UPPER(@colName) + @Totals)
								UPDATE @STATISTICSPTOTALS SET 
								alerts = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =1)),0),
								status = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =2)),0),
								siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =3)),0), 
								fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =4)),0),
								reservations = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =5)),0)
								WHERE header = (@colName + @Totals)
									
							END
	
				END

				-- Retrieve Results Paged and sorted
				BEGIN


					-- INSERT INTO Paging Table
					INSERT INTO @PAGING (rowId) SELECT rowId FROM @STATISTICSP
					ORDER BY
					CASE WHEN @sortExpression = 'header' THEN header END ASC,
					CASE WHEN @sortExpression = 'header DESC' THEN header END DESC,
					CASE WHEN @sortExpression = 'alerts' THEN alerts END ASC,
					CASE WHEN @sortExpression = 'alerts DESC' THEN alerts END DESC,
					CASE WHEN @sortExpression = 'status' THEN status END ASC,
					CASE WHEN @sortExpression = 'status DESC' THEN status END DESC,
					CASE WHEN @sortExpression = 'siteComparison' THEN siteComparison END ASC,
					CASE WHEN @sortExpression = 'siteComparison DESC' THEN siteComparison END DESC,
					CASE WHEN @sortExpression = 'fleetComparison' THEN fleetComparison END ASC,
					CASE WHEN @sortExpression = 'fleetComparison DESC' THEN fleetComparison END DESC,
					CASE WHEN @sortExpression = 'reservations' THEN reservations END ASC,
					CASE WHEN @sortExpression = 'reservations DESC' THEN reservations END DESC


					SELECT  s.header, s.alerts, s.status, s.siteComparison, s.fleetComparison, s.reservations
					FROM @STATISTICSP s 
					INNER JOIN @PAGING p ON p.rowId = s.rowId 
					WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
					ORDER BY p.pageIndex;
				
					SELECT header,alerts,status,siteComparison,fleetComparison,reservations 
					FROM @STATISTICSPTOTALS;

					SELECT COUNT(*) AS totalCount FROM @STATISTICSP;
						
					
				END
		

		END	-- End Off Pooling Tool
		
	If (@marsTool = 3) -- Availability Tool
		BEGIN -- Start Off Availability Tool

			BEGIN -- Create a table variable to retrieve results
				DECLARE @STATISTICSA TABLE
				(
					rowId				INT IDENTITY (1,1) NOT NULL,
					header				VARCHAR(50) NULL,
					fleetStatus			INT NULL,
					historicalTrend		INT NULL,
					siteComparison		INT NULL,
					fleetComparison		INT NULL,
					kpi					INT NULL,
					kpiDownload			INT NULL,
					carSearch			INT NULL
				)

				DECLARE @STATISTICSATOTALS TABLE
				(
					header				VARCHAR(50) NULL,
					fleetStatus			INT NULL,
					historicalTrend		INT NULL,
					siteComparison		INT NULL,
					fleetComparison		INT NULL,
					kpi					INT NULL,
					kpiDownload			INT NULL,
					carSearch			INT NULL
				)
			
			END	


			IF (@logic = 1) -- CMS Logic
				BEGIN -- Start of CMS Logic
					
					BEGIN -- Get Results to query from
							
							INSERT INTO @QUERYTABLE (reportId, reportName, country, cms_pool_id, cms_location_group_id, location, racfid, report_date)
							SELECT marsReportId, marsReportName, country, cms_pool_id, cms_location_group_id, location, racfid, report_date
							FROM dbo.vw_MARS_Usage_Statistics 
							WHERE ((marsToolId=3) AND (report_date BETWEEN @start_date AND @end_date) AND ((@racfid IS NULL) OR ( racfid = @racfid)))
					
							-- Results
							INSERT INTO @STATISTICSA (header)
							SELECT DISTINCT CONVERT(VARCHAR(10), report_date, 103)AS report_date FROM @QUERYTABLE
						
					END
					-- Select All of Europe
					IF ((@country IS NULL) AND (@cms_pool_id IS NULL) AND (@cms_location_group_id IS NULL) AND (@location IS NULL))		
						BEGIN
						
							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header

							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN

									-- Totals
									INSERT INTO @STATISTICSATOTALS (header) VALUES (@EuropeTotals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9)),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)),0)
									WHERE header = @EuropeTotals
				
								END
						END
					

					-- Select CMS_Pools
					IF ((@country IS NOT NULL) AND (@cms_pool_id IS NULL) AND (@cms_location_group_id IS NULL) AND (@location IS NULL))		
						BEGIN
													
							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SET @colName = @Country
									INSERT INTO @STATISTICSATOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9) AND (country =@country)),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)  AND (country =@country)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)  AND (country =@country)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)  AND (country =@country)),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)  AND (country =@country)),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)  AND (country =@country)),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)  AND (country =@country)),0)
									WHERE header = (@colName + @Totals)						
								END
						END
					

					-- Select CMS_Location_groups
					IF ((@country IS NOT NULL) AND (@cms_pool_id IS NOT NULL) AND (@cms_location_group_id IS NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) AND (cms_pool_id =@cms_pool_id ) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header

							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SELECT @colName = cms_pool FROM dbo.CMS_POOLS WHERE cms_pool_id = @cms_pool_id
									INSERT INTO @STATISTICSATOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9) AND (cms_pool_id =@cms_pool_id )),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)  AND (cms_pool_id =@cms_pool_id)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)  AND (cms_pool_id =@cms_pool_id)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)  AND (cms_pool_id =@cms_pool_id)),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)  AND (cms_pool_id =@cms_pool_id)),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)  AND (cms_pool_id =@cms_pool_id)),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)  AND (cms_pool_id =@cms_pool_id)),0)
									WHERE header = (@colName + @Totals)		
								
								END
						END


					-- Select Locations
					IF ((@country IS NOT NULL) AND (@cms_pool_id IS NOT NULL) AND (@cms_location_group_id IS NOT NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) AND (cms_location_group_id=@cms_location_group_id) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header

							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SELECT @colName = cms_location_group FROM dbo.CMS_LOCATION_GROUPS WHERE cms_location_group_id = @cms_location_group_id
									INSERT INTO @STATISTICSATOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9) AND (cms_location_group_id=@cms_location_group_id)),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)  AND (cms_location_group_id=@cms_location_group_id)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)  AND (cms_location_group_id=@cms_location_group_id)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)  AND (cms_location_group_id=@cms_location_group_id)),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)  AND (cms_location_group_id=@cms_location_group_id)),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)  AND (cms_location_group_id=@cms_location_group_id)),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)  AND (cms_location_group_id=@cms_location_group_id)),0)
									WHERE header = (@colName + @Totals)		
								
								END
						END

				END -- End of CMS Logic


			IF (@logic = 2) -- OPS Logic
				BEGIN -- Start of OPS Logic
					
					BEGIN -- Get results to query

							INSERT INTO @QUERYTABLE (reportId, reportName, country, ops_region_id, ops_area_id, location, racfid, report_date)
							SELECT marsReportId, marsReportName, country, ops_region_id, ops_area_id, location, racfid, report_date
							FROM dbo.vw_MARS_Usage_Statistics 
							WHERE ((marsToolId=3) AND (report_date BETWEEN @start_date AND @end_date) AND ((@racfid IS NULL) OR ( racfid = @racfid)) )
							
							-- Results
							INSERT INTO @STATISTICSA (header)
							SELECT DISTINCT CONVERT(VARCHAR(10), report_date, 103)AS report_date FROM @QUERYTABLE				
					END

					-- Select All of Europe
					IF ((@country IS NULL) AND (@ops_region_id IS NULL) AND (@ops_area_id IS NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header
						
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN									
									-- Totals
									INSERT INTO @STATISTICSATOTALS (header) VALUES (@EuropeTotals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9)),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)),0)
									WHERE header = @EuropeTotals
				
								END
									
						END
					
					-- Select OPS_Regions
					IF ((@country IS NOT NULL) AND (@ops_region_id IS NULL) AND (@ops_area_id IS NULL) AND (@location IS NULL))		
						BEGIN
							
							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) AND (country =@country) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN				
									-- Totals
									SET @colName = @Country		
									INSERT INTO @STATISTICSATOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9) AND (country =@country)),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)  AND (country =@country)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)  AND (country =@country)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)  AND (country =@country)),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)  AND (country =@country)),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)  AND (country =@country)),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)  AND (country =@country)),0)
									WHERE header = (@colName + @Totals)						
								END
						END

					-- Select OPS_Areas
					IF ((@country IS NOT NULL) AND (@ops_region_id IS NOT NULL) AND (@ops_area_id IS NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) AND (ops_region_id =@ops_region_id) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN
									-- Totals
									SELECT @colName = ops_region FROM dbo.OPS_REGIONS WHERE ops_region_id = @ops_region_id
									INSERT INTO @STATISTICSATOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9) AND (ops_region_id =@ops_region_id)),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)  AND (ops_region_id =@ops_region_id)),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)  AND (ops_region_id =@ops_region_id)),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)  AND (ops_region_id =@ops_region_id)),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)  AND (ops_region_id =@ops_region_id)),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)  AND (ops_region_id =@ops_region_id)),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)  AND (ops_region_id =@ops_region_id)),0)
									WHERE header = (@colName + @Totals)		
								
								END

						END

					-- Select Locations
					IF ((@country IS NOT NULL) AND (@ops_region_id IS NOT NULL) AND (@ops_area_id IS NOT NULL) AND (@location IS NULL))		
						BEGIN

							UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
							FROM @STATISTICSA s
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
							LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) AND (ops_area_id = @ops_area_id) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header
							
							--Report Totals Only run if we have records
							SELECT @rowCount = COUNT(*) FROM @STATISTICSA
							IF (@rowCount >=1)
								BEGIN
									--Totals
									SELECT @colName = ops_area FROM dbo.OPS_AREAS WHERE ops_area_id = @ops_area_id 
									INSERT INTO @STATISTICSATOTALS (header) VALUES (UPPER(@colName) + @Totals)
									UPDATE @STATISTICSATOTALS SET 
									fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9) AND (ops_area_id = @ops_area_id )),0),
									historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)  AND (ops_area_id = @ops_area_id )),0),
									siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)  AND (ops_area_id = @ops_area_id )),0), 
									fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)  AND (ops_area_id = @ops_area_id )),0),
									kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)  AND (ops_area_id = @ops_area_id )),0),
									kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)  AND (ops_area_id = @ops_area_id )),0),
									carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)  AND (ops_area_id = @ops_area_id )),0)
									WHERE header = (@colName + @Totals)		
								
								END
						END
					
				END -- End of OPS Logic


			-- Return a Single Location
			IF (@logic IS NULL)
				BEGIN 
					
					INSERT INTO @QUERYTABLE (reportId, reportName, location, racfid, report_date)
					SELECT marsReportId, marsReportName, location, racfid, report_date
					FROM dbo.vw_MARS_Usage_Statistics 
					WHERE ((marsToolId=3) AND (report_date BETWEEN @start_date AND @end_date) AND (location =@location) AND ((@racfid IS NULL) OR ( racfid = @racfid)))
					
					-- Results
					INSERT INTO @STATISTICSA (header)
					SELECT DISTINCT CONVERT(VARCHAR(10), report_date, 103)AS report_date FROM @QUERYTABLE				

					UPDATE @STATISTICSA SET fleetStatus =ISNULL(fs.totals,0),historicalTrend=ISNULL(ht.totals,0),siteComparison=ISNULL(sc.totals,0), fleetComparison =ISNULL(fc.totals,0),kpi=ISNULL(k.totals,0),kpiDownload =ISNULL(kd.totals,0), carSearch=ISNULL(cs.totals,0)
					FROM @STATISTICSA s
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =9) GROUP BY CONVERT(varchar(10), report_date, 103))fs ON fs.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =10) GROUP BY CONVERT(varchar(10), report_date, 103))ht ON ht.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =7) GROUP BY CONVERT(varchar(10), report_date, 103))sc ON sc.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =8) GROUP BY CONVERT(varchar(10), report_date, 103))fc ON fc.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =11) GROUP BY CONVERT(varchar(10), report_date, 103))k ON k.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =12) GROUP BY CONVERT(varchar(10), report_date, 103))kd ON kd.report_date = s.header
					LEFT JOIN(SELECT CONVERT(varchar(10), report_date, 103)AS report_date, COUNT(*) as totals FROM @QUERYTABLE WHERE (reportid =6) GROUP BY CONVERT(varchar(10), report_date, 103))cs ON cs.report_date = s.header
					
					--Report Totals Only run if we have records
					SELECT @rowCount = COUNT(*) FROM @STATISTICSA
						IF (@rowCount >=1)
							BEGIN
								--Totals
								SELECT @colName = @location
								INSERT INTO @STATISTICSATOTALS (header) VALUES (UPPER(@colName) + @Totals)
								UPDATE @STATISTICSATOTALS SET 
								fleetStatus = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =9)),0),
								historicalTrend = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =10)),0),
								siteComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =7)),0), 
								fleetComparison = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =8)),0),
								kpi = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =11)),0),
								kpiDownload = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =12)),0),
								carSearch = ISNULL((SELECT COUNT(*) FROM @QUERYTABLE WHERE (reportid =6)),0)
								WHERE header = (@colName + @Totals)
									
							END
	
				END

				-- Retrieve Results Paged and sorted
				BEGIN


					-- INSERT INTO Paging Table
					INSERT INTO @PAGING (rowId) SELECT rowId FROM @STATISTICSA
					ORDER BY
					CASE WHEN @sortExpression = 'header' THEN header END ASC,
					CASE WHEN @sortExpression = 'header DESC' THEN header END DESC,
					CASE WHEN @sortExpression = 'fleetStatus' THEN fleetStatus END ASC,
					CASE WHEN @sortExpression = 'fleetStatus DESC' THEN fleetStatus END DESC,
					CASE WHEN @sortExpression = 'historicalTrend' THEN historicalTrend END ASC,
					CASE WHEN @sortExpression = 'historicalTrend DESC' THEN historicalTrend END DESC,
					CASE WHEN @sortExpression = 'siteComparison' THEN siteComparison END ASC,
					CASE WHEN @sortExpression = 'siteComparison DESC' THEN siteComparison END DESC,
					CASE WHEN @sortExpression = 'fleetComparison' THEN fleetComparison END ASC,
					CASE WHEN @sortExpression = 'fleetComparison DESC' THEN fleetComparison END DESC,
					CASE WHEN @sortExpression = 'kpi' THEN kpi END ASC,
					CASE WHEN @sortExpression = 'kpi DESC' THEN kpi END DESC,
					CASE WHEN @sortExpression = 'kpiDownload' THEN kpiDownload END ASC,
					CASE WHEN @sortExpression = 'kpiDownload DESC' THEN kpiDownload END DESC,
					CASE WHEN @sortExpression = 'carSearch' THEN carSearch END ASC,
					CASE WHEN @sortExpression = 'carSearch DESC' THEN carSearch END DESC


					SELECT  s.header, s.fleetStatus, s.historicalTrend, s.siteComparison, s.fleetComparison, s.kpi, s.kpiDownload, s.carSearch
					FROM @STATISTICSA s 
					INNER JOIN @PAGING p ON p.rowId = s.rowId 
					WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
					ORDER BY p.pageIndex;
				
					SELECT header,fleetStatus,historicalTrend,siteComparison,fleetComparison,kpi, kpiDownload, carSearch
					FROM @STATISTICSATOTALS;

					SELECT COUNT(*) AS totalCount FROM @STATISTICSA;
						
					
				END
		

		END	-- End Off Availability Tool

END