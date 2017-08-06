
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid_Start_Remarks]

	@groupby				VARCHAR(20)		= NULL,
	@country				VARCHAR(10)		= NULL,
	@cms_pool_id			INT				= NULL,
	@cms_location_group_id	INT				= NULL, 
	@ops_region_id			INT				= NULL,
	@ops_area_id			INT				= NULL,	
	@location				VARCHAR(50)		= NULL,
	@fleet_name				VARCHAR(50)		= NULL,
	@car_segment_id			INT				= NULL,
	@car_class_id			INT				= NULL,
	@car_group_id			INT				= NULL,
	@daygroupcode			VARCHAR(255)	= NULL,	
	@sortExpression			VARCHAR(50)		= NULL,
	@startRowIndex			INT				= NULL,
	@maximumRows			INT				= NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;
	-- Change 1
	---------------------------------------------------------------------------------------------
	IF @daygroupcode IS NULL SET @daygroupcode = N'0-2,3,4-6,7,8-10,11-15,16-30,31-60,60+'
	---------------------------------------------------------------------------------------------
	
	DECLARE @calendar_id INT = (SELECT MAX(dim_calendar_id) FROM [General].[Fact_NonRevLog_Summary])
	
	DECLARE @calendar_date DATETIME = 
		(SELECT TOP 1 Rep_Date FROM [Inp].dim_Calendar WHERE dim_Calendar_id = @calendar_id)
	
	DECLARE @fleetname VARCHAR(20)
	
	IF @fleet_name IS NULL 				BEGIN SET @fleetname = 'all' END
	IF @fleet_name = 'CARSALES'			BEGIN SET @fleetname = 'cs' END
	IF @fleet_name = 'RAC OPS'			BEGIN SET @fleetname = 'op' END
	IF @fleet_name = 'RAC TTL'			BEGIN SET @fleetname = 'tt' END
	IF @fleet_name = 'ADVANTAGE'		BEGIN SET @fleetname = 'ad' END
	IF @fleet_name = 'HERTZ ON DEMAND'	BEGIN SET @fleetname = 'hd' END
	IF @fleet_name = 'LICENSEE'			BEGIN SET @fleetname = 'lc' END
	
	
	CREATE TABLE #REMARKS
	(
		RowId INT IDENTITY , GroupCode_Stats VARCHAR(20) , GroupCode_Kci VARCHAR(20) , TotalFleet INT,
		TotalVehicles INT , PctNonRev NUMERIC(5,2), PctTotal NUMERIC(5,2),
		RemarksFill INT , RemarksNoFill INT , PctRemarksFill NUMERIC(5,2),
		DayGroup1 INT , DayGroup2 INT , DayGroup3 INT , DayGroup4 INT , DayGroup5 INT , 
		DayGroup6 INT , DayGroup7 INT , DayGroup8 INT , DayGroup9 INT
	)
	
	INSERT INTO #REMARKS
	(
		GroupCode_Stats , GroupCode_Kci , TotalFleet ,  TotalVehicles ,
		DayGroup1 , DayGroup2 , DayGroup3 , DayGroup4 , DayGroup5 , 
		DayGroup6 , DayGroup7 , DayGroup8 , DayGroup9
	)
	
	SELECT 
		nrls.OperationalStatusCode	AS [GroupCode_Stats], 
		os.KCICode					AS [GroupCode_Kci], 
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.total_fleet_tt) + SUM(nrls.total_fleet_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.total_fleet_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.total_fleet_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.total_fleet_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.total_fleet_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.total_fleet_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.total_fleet_op)
	END AS [TotalFleet],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.non_rev_fleet_tt) + SUM(nrls.non_rev_fleet_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.non_rev_fleet_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.non_rev_fleet_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.non_rev_fleet_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.non_rev_fleet_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.non_rev_fleet_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.non_rev_fleet_op)
	END AS [TotalVehicles],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup1_tt) + SUM(nrls.daygroup1_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup1_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup1_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup1_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup1_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup1_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup1_op)
	END		
	AS [Daygroup1],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup2_tt) + SUM(nrls.daygroup2_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup2_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup2_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup2_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup2_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup2_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup2_op)
	END AS [Daygroup2],
			
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup3_tt) + SUM(nrls.daygroup3_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup3_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup3_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup3_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup3_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup3_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup3_op)
	END
	AS [Daygroup3],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup4_tt) + SUM(nrls.daygroup4_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup4_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup4_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup4_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup4_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup4_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup4_op)
	END
	AS [Daygroup4],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup5_tt) + SUM(nrls.daygroup5_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup5_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup5_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup5_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup5_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup5_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup5_op)
	END
	AS [Daygroup5],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup6_tt) + SUM(nrls.daygroup6_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup6_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup6_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup6_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup6_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup6_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup6_op)
	END
	AS [Daygroup6],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup7_tt) + SUM(nrls.daygroup7_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup7_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup7_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup7_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup7_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup7_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup7_op)
	END
	AS [Daygroup7],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup8_tt) + SUM(nrls.daygroup8_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup8_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup8_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup8_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup8_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup8_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup8_op)
	END
	AS [Daygroup8],
	CASE
		WHEN @fleetname = 'all' THEN SUM(nrls.daygroup9_tt) + SUM(nrls.daygroup9_cs)
		WHEN @fleetname = 'cs'  THEN SUM(nrls.daygroup9_cs)
		WHEN @fleetname = 'lc'  THEN SUM(nrls.daygroup9_lc)
		WHEN @fleetname = 'ad'  THEN SUM(nrls.daygroup9_ad)
		WHEN @fleetname = 'hd'  THEN SUM(nrls.daygroup9_hd)
		WHEN @fleetname = 'tt'  THEN SUM(nrls.daygroup9_tt)
		WHEN @fleetname = 'op'  THEN SUM(nrls.daygroup9_op)
	END
	AS [Daygroup9]
	
	FROM 
		[General].[Fact_NonRevLog_Summary] nrls
	INNER JOIN [Settings].Operational_Status os ON nrls.OperationalStatusCode = os.OperationalStatusCode
	WHERE 
		nrls.dim_calendar_id = @calendar_id
	AND ((@country is null) or (nrls.country_car = @country))
	AND ((@cms_pool_id is null) or (nrls.location_pool_id = @cms_pool_id))
	AND ((@cms_location_group_id is null) or (nrls.location_group_id = @cms_location_group_id))
	AND ((@ops_region_id is null) or (nrls.location_ops_region_id = @ops_region_id))
	AND ((@ops_area_id is null) or (nrls.location_ops_area_id = @ops_area_id))
	AND ((@location is null) or (nrls.location = @location))
	AND ((@car_segment_id is null) or (nrls.car_segment_id = @car_segment_id))
	AND ((@car_class_id is null) or (nrls.car_class_id = @car_class_id))
	AND ((@car_group_id is null) or (nrls.car_group_id = @car_group_id))
	GROUP BY 
		nrls.OperationalStatusCode , os.KCICode
	
		-- Change 2
	---------------------------------------------------------------------------------------------
	declare @tablecmd table (id int,daygroup varchar(10) , sqlstring varchar(255))
	insert into @tablecmd (id , daygroup)
	select ROW_NUMBER() OVER( ORDER BY Items) , Items from dbo.fSplit(@daygroupcode,',')
	declare @max int = (select MAX(NonRevDayGroupId) from [Settings].[NonRev_Day_Groups])
	update @tablecmd set sqlstring = t.sqlstring from @tablecmd tb
	inner join (
	select 
	DayGroupCode , 
	DayGroupName + ' + ' as [sqlstring]
	
	from [Settings].[NonRev_Day_Groups]) t on tb.daygroup = t.DayGroupCode
	declare @i int = 1 declare @cmdselect varchar(4000) = ''
	while exists(select sqlstring from @tablecmd where @i = id)
	begin
		set @cmdselect = @cmdselect +  (select sqlstring from @tablecmd where id = @i)
		set @i = @i + 1
	end
	set @cmdselect = @cmdselect + '_centinel'
	set @cmdselect = REPLACE(@cmdselect,'+ _centinel','')
	set @cmdselect = REPLACE(@cmdselect,'_centinel','')
	
	declare @rowcounter int = (select count(*) from @tablecmd)
		
	declare @cmd varchar(4000)
	set @cmd = 'UPDATE #REMARKS SET TotalVehicles = ' + @cmdselect
	
	--SELECT @cmd
	exec (@cmd)
	
	
	---------------------------------------------------------------------------------------------
		
	DECLARE @totaFleet INT , @totalFleetNonRev INT
	
	SET @totaFleet			= (SELECT SUM(TotalFleet) FROM #REMARKS)
	
	SET @totalFleetNonRev	= (SELECT SUM(TotalVehicles) FROM #REMARKS)
	
	UPDATE #REMARKS SET PctTotal = CONVERT(NUMERIC(5,2), TotalVehicles * 100 / @totaFleet)
	
	;WITH RemarksFill AS
	(
	SELECT 
		nrl.NonRevLogId , nrl.VehicleId , f.Serial , 
		nrl.OperStat , nrl.KCICode , nrl.CountryLoc , f.CarGroup , 
		ISNULL(cg.car_group_id,0)   AS [car_group_id], 
		ISNULL(cg.car_class_id,0)   AS [car_class_id], 
		ISNULL(cg.car_segment_id,0) AS [car_segment_id],
		nrl.Lstwwd AS [Location],
		cms.cms_location_group_id , cms.cms_pool_id , 
		ops.ops_area_id , ops.ops_region_id , nrl.IsOpen , nrl.RemarkId , 
		nrl.StartDate , nrl.EndDate , 
		1 AS [TotalFleet]
	FROM [General].Fact_NonRevLog nrl
	INNER JOIN [General].Dim_Fleet f on nrl.VehicleId = f.VehicleId
	LEFT JOIN [Settings].vw_CarGroups cg on f.CarGroup = cg.car_group and f.Country = cg.country
	LEFT JOIN [Settings].vw_Locations_CMS cms on nrl.Lstwwd = cms.location
	LEFT JOIN [Settings].vw_Locations_OPS ops on nrl.Lstwwd = ops.location
	where 
		(	nrl.EndDate = @calendar_date or
			nrl.EndDate is null or
			nrl.StartDate = @calendar_date) 
		AND nrl.RemarkId > 1
		
		AND ((@country is null) or (nrl.CountryCar = @country))
		AND ((@cms_pool_id is null) or (cms.cms_pool_id = @cms_pool_id))
		AND ((@cms_location_group_id is null) or (cms.cms_location_group_id = @cms_location_group_id))
		AND ((@ops_region_id is null) or (ops.ops_region_id = @ops_region_id))
		AND ((@ops_area_id is null) or (ops.ops_area_id = @ops_area_id))
		AND ((@location is null) or (nrl.Lstwwd = @location))
		AND ((@car_segment_id is null) or (cg.car_segment_id = @car_segment_id))
		AND ((@car_class_id is null) or (cg.car_class_id = @car_class_id))
		AND ((@car_group_id is null) or (cg.car_group_id = @car_group_id))
		AND ((@daygroupcode IS NULL) OR (nrl.DayGroupCode in (select Items from dbo.fSplit(@daygroupcode,','))))
		
		
	)
		
	UPDATE #REMARKS SET 
		RemarksFill = ISNULL(t.TotalFleet,0)
	FROM #REMARKS r
	INNER JOIN 
	(
		SELECT 
			OperStat , KCICode , SUM(TotalFleet) AS [TotalFleet]
		FROM RemarksFill
		GROUP BY 
			OperStat , KCICode
	) t
	ON r.GroupCode_Kci = t.KCICode and r.GroupCode_Stats = t.OperStat
	
	UPDATE #REMARKS SET RemarksFill = 0 WHERE RemarksFill IS NULL
	
	UPDATE #REMARKS SET RemarksNoFill = TotalVehicles - RemarksFill
			
	DECLARE @TABLE_RESULTS TABLE
	(
		rowId		INT IDENTITY (1,1) NOT NULL,
		GroupCode	VARCHAR(20) , TotalVehicles VARCHAR(20) , 
		PctNonRev	VARCHAR(20)	, PctTotal		VARCHAR(20)	, 
		RemarksFill	VARCHAR(20)	, RemarksNoFill VARCHAR(20) , PctRemarksFill	VARCHAR(20)	
	)
	
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
	
	IF @groupby IS NULL or @groupby = 'KCI'
	BEGIN
		INSERT INTO @TABLE_RESULTS
		(GroupCode , TotalVehicles , PctNonRev , PctTotal , RemarksFill , RemarksNoFill , PctRemarksFill)
	
		SELECT 
			R.GroupCode_Kci										AS [GroupCode], 
			dbo.fnAddCommaToInt(SUM(R.TotalVehicles))			AS [TotalVehicles] , 
						
			NULL , NULL ,

			dbo.fnAddCommaToInt(ISNULL(SUM(R.RemarksFill),0))	AS [RemarksFill],
			
			dbo.fnAddCommaToInt(SUM(R.RemarksNoFill))			AS [RemarksNoFill] ,
			
			CONVERT(VARCHAR,
				CONVERT(NUMERIC(5,2), 
					ROUND(
						ISNULL(SUM(R.RemarksFill),0) * 100
					/
						case when SUM(R.TotalVehicles) = 0 then 1 else SUM(R.TotalVehicles) end
					,2)
				)
			
				) + ' %'
									AS [PctRemarksFill]  
		FROM 
			#REMARKS R
		GROUP BY
			R.GroupCode_Kci
		--ORDER BY 
		--	CONVERT(INT,avg(R.PctNonRev)) desc
		
		
		UPDATE @TABLE_RESULTS SET
		PctTotal  = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
		CAST( convert(float,replace(TotalVehicles,',','')) AS FLOAT) * 100.00 / 
			CAST(@totaFleet AS FLOAT))) + ' %' , 
		PctNonRev = CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),
		CAST( convert(float,replace(TotalVehicles,',','')) AS FLOAT) * 100.00 / 
			CAST(@totalFleetNonRev AS FLOAT))) + ' %'
			
		

	END
	ELSE IF @groupby = 'STAT'
	BEGIN
		INSERT INTO @TABLE_RESULTS
		(GroupCode , TotalVehicles , PctNonRev , PctTotal , RemarksFill , RemarksNoFill , PctRemarksFill)
	
		SELECT 
			R.GroupCode_Stats								AS [GroupCode], 
			dbo.fnAddCommaToInt(sum(R.TotalVehicles))			AS [TotalVehicles] , 
			
			CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),CAST(sum(R.TotalVehicles) AS FLOAT) * 100.00 / 
			CAST(@totaFleet AS FLOAT))) + ' %' 
			
			AS [PctNonRev],
			
			CONVERT(VARCHAR,CONVERT(NUMERIC(5,2),CAST(sum(R.TotalVehicles) AS FLOAT) * 100.00 / 
			CAST(@totalFleetNonRev AS FLOAT))) + ' %'
			
			AS [PctTotal],
			
			dbo.fnAddCommaToInt(ISNULL(sum(R.RemarksFill),0))	AS [RemarksFill],
			dbo.fnAddCommaToInt(sum(R.RemarksNoFill))			AS [RemarksNoFill] ,
			
			CONVERT(VARCHAR,
				CONVERT(NUMERIC(5,2), 
					ROUND(
					CONVERT(NUMERIC(20,2),ISNULL(SUM(R.RemarksFill),0) * 100)
					/
					CONVERT(NUMERIC(20,2), case when SUM(R.TotalVehicles) = 0 then 1 else SUM(R.TotalVehicles) end)
					,2)
				)
			
				) + ' %'					AS [PctRemarksFill]   
		FROM 
			#REMARKS R
		GROUP BY
			r.GroupCode_Stats
		--ORDER BY 
			--CONVERT(INT,R.PctNonRev) desc
	END
		
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @TABLE_RESULTS
	ORDER BY
	CASE WHEN @sortExpression = 'GroupCode'				THEN GroupCode END ASC,
	CASE WHEN @sortExpression = 'GroupCode DESC'		THEN GroupCode END DESC,
	CASE WHEN @sortExpression = 'TotalVehicles'			THEN CONVERT(INT,REPLACE(TotalVehicles,',',''))  END ASC,
	CASE WHEN @sortExpression = 'TotalVehicles DESC'	THEN CONVERT(INT,REPLACE(TotalVehicles,',',''))  END DESC,
	CASE WHEN @sortExpression = 'PctNonRev'				THEN CONVERT(FLOAT,REPLACE(PctNonRev,'%','')) END ASC,
	CASE WHEN @sortExpression = 'PctNonRev DESC'		THEN CONVERT(FLOAT,REPLACE(PctNonRev,'%','')) END DESC,
	CASE WHEN @sortExpression = 'PctTotal'				THEN CONVERT(FLOAT,REPLACE(PctTotal,'%','')) END ASC,
	CASE WHEN @sortExpression = 'PctTotal DESC'			THEN CONVERT(FLOAT,REPLACE(PctTotal,'%','')) END DESC,	
	CASE WHEN @sortExpression = 'RemarksFill'			THEN CONVERT(INT,REPLACE(RemarksFill,',',''))  END ASC,
	CASE WHEN @sortExpression = 'RemarksFill DESC'		THEN CONVERT(INT,REPLACE(RemarksFill,',',''))  END DESC,
	CASE WHEN @sortExpression = 'RemarksNoFill'			THEN CONVERT(INT,REPLACE(RemarksNoFill,',',''))  END ASC,
	CASE WHEN @sortExpression = 'RemarksNoFill DESC'	THEN CONVERT(INT,REPLACE(RemarksNoFill,',',''))  END DESC,
	CASE WHEN @sortExpression = 'PctRemarksFill'		THEN CONVERT(FLOAT,REPLACE(PctRemarksFill,',',''))  END ASC,
	CASE WHEN @sortExpression = 'PctRemarksFill DESC'	THEN CONVERT(FLOAT,REPLACE(PctRemarksFill,',',''))  END DESC
	
	,CASE WHEN @sortExpression IS NULL THEN 
		CONVERT(INT,REPLACE(RemarksNoFill,',','')) END ASC ,
		CONVERT(INT,REPLACE(TotalVehicles,',','')) DESC
 
	
	SELECT 
		CS.GroupCode , CS.TotalVehicles , CS.PctNonRev , CS.PctTotal ,
		CS.RemarksFill , CS.RemarksNoFill  , CS.PctRemarksFill
	FROM 
		@TABLE_RESULTS CS
	INNER JOIN 
		@PAGING p ON p.rowId = cs.rowId 
	WHERE 
		(p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY 
		p.pageIndex;
		
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @TABLE_RESULTS;
	
	DROP TABLE #REMARKS

    
END