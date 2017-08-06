-- =============================================
-- Author:		Javier
-- Create date: October 2012
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [General].[Select_NonRev_Grid]
	
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
	@ownarea				VARCHAR(1500)	= NULL,
	@movType				VARCHAR(500)	= NULL,
	@no_rev					INT				= NULL,
	@plate					VARCHAR(25)		= NULL, 
	@unit					VARCHAR(10)		= NULL, 
	@serial					VARCHAR(25)		= NULL, 
	@driverName				VARCHAR(25)		= NULL, 
	@colour					VARCHAR(5)		= NULL, 
	@mileage				NUMERIC(18,2)	= NULL,	
	@model					VARCHAR(8000)	= NULL, 
	@modelDesc				VARCHAR(8000)	= NULL, 
	@operstat_name			VARCHAR(300)	= NULL,
	@select_by				VARCHAR(10)		= NULL,
	@hasRemark				VARCHAR(10)		= NULL,
	@sortExpression			VARCHAR(50)		= NULL,
	@startRowIndex			INT				= NULL,
	@maximumRows			INT				= NULL
	
AS
BEGIN
	
	--IF @hasRemark IS NULL BEGIN SET @hasRemark = 0 END
	IF @hasRemark = 'All' BEGIN SET @hasRemark = NULL END ELSE BEGIN SET @hasRemark = 1 END
	IF @select_by IS NULL BEGIN SET @select_by = 'LSTWWD' END
	
	
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
	
	DECLARE @CARSEARCH TABLE 
	(
		rowId				INT IDENTITY (1,1) NOT NULL,
		Lstwwd				VARCHAR(50)		NULL,
		Duewwd				VARCHAR(50)		NULL,
		DueDate				DateTime		NULL,
		CarGroup			VARCHAR(50)		NULL,
		Plate				VARCHAR(50)		NULL,
		Unit				VARCHAR(50)		NULL,
		ModelDesc			VARCHAR(255)	NULL,
		LstDate				DATETIME		NULL,
		LstMlg				INT				NULL,
		NRDays				INT				NULL,
		OperStat			VARCHAR(10)		NULL,
		MoveType			VARCHAR(10)		NULL,
		DepStat				VARCHAR(10)		NULL,
		Carhold				VARCHAR(10)		NULL,
		NextRent			DATETIME		NULL,
		RemarkCode			VARCHAR(50)		NULL,
		Remark				VARCHAR(255)	NULL,
		Serial				VARCHAR(30)		NULL,
		NextRentColor		VARCHAR(30)		NULL
	)
	
	IF (@select_by = 'LSTWWD')
	BEGIN
		INSERT INTO @CARSEARCH
		(
			Lstwwd , Duewwd, DueDate, CarGroup , Plate , Unit , 
			ModelDesc , LstDate , LstMlg ,
			NRDays , OperStat , MoveType , DepStat ,
			Carhold , NextRent , RemarkCode , Remark , Serial 
		)
		
		
		SELECT
			FNR.Lstwwd , FNR.Duewwd, FNR.DueDate, FNR.CarGroup , FNR.Plate , FNR.Unit , 
			FNR.ModelDesc , FNR.LstDate , FNR.LstMlg ,
			FNR.NRDays , FNR.OperStat , 
			FNR.MoveType , FNR.DepStat , FNR.Carhold , FNR.ERDate as [NextRent] 
			, NRRL.RemarkText
			, LEFT(FNR.Remark, 50)
			--ISNULL(SUBSTRING(NRRL.RemarkText,1,10),'') + '...' AS [RemarkCode], 
			--ISNULL(SUBSTRING(FNR.Remark,1,10),'') + '...' AS [Remark],
			, FNR.Serial
		FROM 
			[General].vw_Fleet_NonRevLog FNR
		LEFT JOIN [Settings].[NonRev_Remarks_List] AS NRRL ON
			FNR.RemarkId = NRRL.RemarkId
		WHERE
				FNR.IsOpen = 1 AND
				
			( FNR.CountryCar = @country OR @country IS NULL)
			
		AND ((@hasRemark IS NULL) OR (NRRL.RemarkText = 'No Reason Code' or FNR.ERDate < DATEADD(D,0,DATEDIFF(D,0,GETDATE()))))
			
		AND ((FNR.Lstwwd IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
		AND ((FNR.Lstwwd IN (SELECT location	FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
		AND ((FNR.Lstwwd IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
		AND ((FNR.Lstwwd IN (SELECT location	FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
		AND ( FNR.Lstwwd = @location OR @location IS NULL)
		AND (
			(@fleet_name = 'CARSALES'			AND FNR.Fleet_carsales > 0) 
		OR 
			(@fleet_name = 'RAC OPS'			AND FNR.Fleet_rac_ops > 0) 
		OR 
			(@fleet_name = 'RAC TTL'			AND FNR.Fleet_rac_ttl > 0) 
		OR 
			(@fleet_name = 'ADVANTAGE'			AND FNR.Fleet_adv > 0) 
		OR 
			(@fleet_name = 'HERTZ ON DEMAND'	AND FNR.Fleet_hod > 0) 
		OR 
			(@fleet_name = 'LICENSEE'			AND FNR.Fleet_licensee > 0) 
		OR 
			(@fleet_name IS NULL				
			AND (FNR.Fleet_carsales > 0 OR FNR.Fleet_rac_ttl > 0)))--Fleet	
		AND ((FNR.CarGroup IN (SELECT CG.car_group	FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
		AND ((FNR.CarGroup IN (SELECT CG.car_group	FROM CAR_GROUPS AS CG	WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
		AND ((FNR.CarGroup =  (SELECT car_group		FROM CAR_GROUPS			WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
		AND ((FNR.OwnArea IN (SELECT * FROM fSplit(@ownarea,','))) OR @ownarea IS NULL)  
		AND ((FNR.MoveType IN (SELECT * FROM fSplit(@movType,','))) OR @movType IS NULL)  
		AND ((FNR.NRdays >= @no_rev) OR @no_rev IS NULL) 
		AND ((FNR.Plate LIKE ('%' + @plate + '%')) or @plate IS NULL) 
		AND ((FNR.Unit LIKE ('%' + @unit + '%')) or @unit IS NULL) 
		AND ((FNR.Serial LIKE ('%' + @serial + '%')) or @serial IS NULL) 
		AND ((FNR.DrvName LIKE ('%' + @driverName + '%')) or @driverName IS NULL) 
		AND ((FNR.Color LIKE ('%' + @colour + '%')) or @colour IS NULL) 
		AND ((FNR.LstMlg >= @mileage) or @mileage IS NULL) 
		AND ((FNR.ModelDesc LIKE ('%' + @modelDesc + '%')) or @modelDesc IS NULL) 
		AND ((FNR.Model IN (SELECT * FROM fSplit(@model,','))) or @model IS NULL) 
		AND (@operstat_name IS NULL 		
		OR ('BD' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'BD')
		OR ('WS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'WS')		
		OR ('CU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'CU')
		OR ('FS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'FS')
		OR ('HA' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'HA')
		OR ('HL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'HL')
		OR ('LL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'LL')
		OR ('MM' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'MM')
		OR ('NC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'NC')
		OR ('PL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'PL')
		OR ('RL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'RL')
		OR ('RP' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'RP')
		OR ('RT' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'RT')
		OR ('SU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'SU')
		OR ('SV' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'SV')
		OR ('TB' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TB')
		OR ('TC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TC')
		OR ('TN' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TN')
		OR ('TW' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TW')		
		OR ('UNK' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'UNK') 
		) -- Status (OPERSTAT)
		
		ORDER BY FNR.NRdays DESC , Lstwwd , OperStat , CarGroup , Plate
		
	END
	ELSE IF (@select_by = 'DUEWWD')
	BEGIN
		INSERT INTO @CARSEARCH
		(
			Lstwwd , Duewwd, DueDate, CarGroup , Plate , Unit , 
			ModelDesc , LstDate , LstMlg ,
			NRDays , OperStat , MoveType , DepStat ,
			Carhold , NextRent , RemarkCode , Remark , Serial
		)
		
		
		SELECT
			FNR.Lstwwd , FNR.Duewwd, FNR.DueDate, FNR.CarGroup , FNR.Plate , FNR.Unit , 
			FNR.ModelDesc , FNR.LstDate , FNR.LstMlg ,
			FNR.NRDays , FNR.OperStat , 
			FNR.MoveType , FNR.DepStat , FNR.Carhold , FNR.ERDate as [NextRent] ,
			ISNULL(SUBSTRING(NRRL.RemarkText,1,10),'') + '...' AS [RemarkCode], 
			ISNULL(SUBSTRING(FNR.Remark,1,10),'') + '...' AS [Remark],
			FNR.Serial
		FROM 
			[General].vw_Fleet_NonRevLog FNR
		LEFT JOIN [Settings].[NonRev_Remarks_List] AS NRRL ON
			FNR.RemarkId = NRRL.RemarkId
		WHERE
				FNR.IsOpen = 1 AND
				
			( FNR.CountryCar = @country OR @country IS NULL)
		
		AND ((@hasRemark IS NULL) OR (NRRL.RemarkText = 'No Reason Code' or FNR.ERDate < DATEADD(D,0,DATEDIFF(D,0,GETDATE()))))	
			
		AND ((FNR.Duewwd IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
		AND ((FNR.Duewwd IN (SELECT location	FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
		AND ((FNR.Duewwd IN (SELECT L.location	FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
		AND ((FNR.Duewwd IN (SELECT location	FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
		AND ( FNR.Duewwd = @location OR @location IS NULL)
		AND (
			(@fleet_name = 'CARSALES'			AND FNR.Fleet_carsales > 0) 
		OR 
			(@fleet_name = 'RAC OPS'			AND FNR.Fleet_rac_ops > 0) 
		OR 
			(@fleet_name = 'RAC TTL'			AND FNR.Fleet_rac_ttl > 0) 
		OR 
			(@fleet_name = 'ADVANTAGE'			AND FNR.Fleet_adv > 0) 
		OR 
			(@fleet_name = 'HERTZ ON DEMAND'	AND FNR.Fleet_hod > 0) 
		OR 
			(@fleet_name = 'LICENSEE'			AND FNR.Fleet_licensee > 0) 
		OR 
			(@fleet_name IS NULL				AND (FNR.Fleet_carsales > 0 OR FNR.Fleet_rac_ttl > 0)))--Fleet	
		AND ((FNR.CarGroup IN (SELECT CG.car_group	FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
		AND ((FNR.CarGroup IN (SELECT CG.car_group	FROM CAR_GROUPS AS CG	WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
		AND ((FNR.CarGroup =  (SELECT car_group		FROM CAR_GROUPS			WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
		AND ((FNR.OwnArea IN (SELECT * FROM fSplit(@ownarea,','))) OR @ownarea IS NULL)  
		AND ((FNR.MoveType IN (SELECT * FROM fSplit(@movType,','))) OR @movType IS NULL)  
		AND ((FNR.NRdays >= @no_rev) OR @no_rev IS NULL) 
		AND ((FNR.Plate LIKE ('%' + @plate + '%')) or @plate IS NULL) 
		AND ((FNR.Unit LIKE ('%' + @unit + '%')) or @unit IS NULL) 
		AND ((FNR.Serial LIKE ('%' + @serial + '%')) or @serial IS NULL) 
		AND ((FNR.DrvName LIKE ('%' + @driverName + '%')) or @driverName IS NULL) 
		AND ((FNR.Color LIKE ('%' + @colour + '%')) or @colour IS NULL) 
		AND ((FNR.LstMlg >= @mileage) or @mileage IS NULL) 
		AND ((FNR.ModelDesc LIKE ('%' + @modelDesc + '%')) or @modelDesc IS NULL) 
		AND ((FNR.Model IN (SELECT * FROM fSplit(@model,','))) or @model IS NULL) 
		AND (@operstat_name IS NULL 		
		OR ('BD' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'BD')
		OR ('WS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'WS')		
		OR ('CU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'CU')
		OR ('FS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'FS')
		OR ('HA' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'HA')
		OR ('HL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'HL')
		OR ('LL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'LL')
		OR ('MM' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'MM')
		OR ('NC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'NC')
		OR ('PL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'PL')
		OR ('RL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'RL')
		OR ('RP' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'RP')
		OR ('RT' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'RT')
		OR ('SU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'SU')
		OR ('SV' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'SV')
		OR ('TB' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TB')
		OR ('TC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TC')
		OR ('TN' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TN')
		OR ('TW' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'TW')		
		OR ('UNK' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FNR.OperStat = 'UNK') 
		) -- Status (OPERSTAT)
		
		ORDER BY FNR.NRdays DESC , Lstwwd , OperStat , CarGroup , Plate
	END
	
	
	UPDATE @CARSEARCH SET NextRentColor = 'Green' WHERE 
		DATEADD(D,0,DATEDIFF(D,0,NextRent)) > DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
	UPDATE @CARSEARCH SET NextRentColor = 'Yellow' WHERE 
		DATEADD(D,0,DATEDIFF(D,0,NextRent)) = DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
	UPDATE @CARSEARCH SET NextRentColor = 'Red' WHERE 
		DATEADD(D,0,DATEDIFF(D,0,NextRent)) < DATEADD(D,0,DATEDIFF(D,0,GETDATE()))
	UPDATE @CARSEARCH SET NextRentColor = 'Red' WHERE NextRentColor IS NULL
	
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @CARSEARCH
	ORDER BY
	CASE WHEN @sortExpression = 'Lstwwd'				THEN Lstwwd END ASC,
	CASE WHEN @sortExpression = 'Lstwwd DESC'			THEN Lstwwd END DESC,
	CASE WHEN @sortExpression = 'CarGroup'				THEN CarGroup END ASC,
	CASE WHEN @sortExpression = 'CarGroup DESC'			THEN CarGroup END DESC,
	CASE WHEN @sortExpression = 'Plate'					THEN Plate END ASC,
	CASE WHEN @sortExpression = 'Plate DESC'			THEN Plate END DESC,
	CASE WHEN @sortExpression = 'Unit'					THEN Unit END ASC,
	CASE WHEN @sortExpression = 'Unit DESC'				THEN Unit END DESC,	
	CASE WHEN @sortExpression = 'ModelDesc'				THEN ModelDesc END ASC,
	CASE WHEN @sortExpression = 'ModelDesc DESC'		THEN ModelDesc END DESC,
	CASE WHEN @sortExpression = 'LstDate'				THEN LstDate END ASC,
	CASE WHEN @sortExpression = 'LstDate DESC'			THEN LstDate END DESC,
	CASE WHEN @sortExpression = 'LstMlg'				THEN LstMlg END ASC,
	CASE WHEN @sortExpression = 'LstMlg DESC'			THEN LstMlg END DESC,
	CASE WHEN @sortExpression = 'NRDays'				THEN NRDays END ASC,
	CASE WHEN @sortExpression = 'NRDays DESC'			THEN NRDays END DESC,
	CASE WHEN @sortExpression = 'OperStat'				THEN OperStat END ASC,
	CASE WHEN @sortExpression = 'OperStat DESC'			THEN OperStat END DESC,
	CASE WHEN @sortExpression = 'MoveType'				THEN MoveType END ASC,
	CASE WHEN @sortExpression = 'MoveType DESC'			THEN MoveType END DESC,
	CASE WHEN @sortExpression = 'DepStat'				THEN DepStat END ASC,
	CASE WHEN @sortExpression = 'DepStat DESC'			THEN DepStat END DESC,
	CASE WHEN @sortExpression = 'Carhold'				THEN Carhold END ASC,
	CASE WHEN @sortExpression = 'Carhold DESC'			THEN Carhold END DESC,
	CASE WHEN @sortExpression = 'NextRent'				THEN NextRent END ASC,
	CASE WHEN @sortExpression = 'NextRent DESC'			THEN NextRent END DESC,
	CASE WHEN @sortExpression = 'Processed'				THEN NextRentColor END ASC,
	CASE WHEN @sortExpression = 'Processed DESC'		THEN NextRentColor END DESC,
	CASE WHEN @sortExpression = 'RemarkCode'			THEN RemarkCode END ASC,
	CASE WHEN @sortExpression = 'RemarkCode DESC'		THEN RemarkCode END DESC,
	CASE WHEN @sortExpression = 'Remark'				THEN Remark END ASC,
	CASE WHEN @sortExpression = 'Remark DESC'			THEN Remark END DESC
	
	SELECT 
		CS.Lstwwd , cs.Duewwd, cs.DueDate, CS.CarGroup , CS.Plate , CS.Unit ,
		CS.ModelDesc , CS.LstDate , CS.LstMlg ,
		CS.NRDays , CS.OperStat , CS.MoveType ,
		CS.DepStat , CS.Carhold , CS.NextRent , CS.RemarkCode ,  CS.Remark , CS.Serial ,
		CS.NextRentColor
	FROM 
		@CARSEARCH CS
	INNER JOIN 
		@PAGING p ON p.rowId = cs.rowId 
	WHERE 
		(p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY 
		p.pageIndex;
		
	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @CARSEARCH;
    
END