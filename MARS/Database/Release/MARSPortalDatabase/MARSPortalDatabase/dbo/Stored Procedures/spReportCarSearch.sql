-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spReportCarSearch] 
	
	@country				VARCHAR(10)		= NULL,
	@cms_pool_id			INT				= NULL,
	@cms_location_group_id	INT				= NULL, --@cms_location_group_code VARCHAR(10) = NULL,
	@ops_region_id			INT				= NULL,
	@ops_area_id			INT				= NULL,	
	@location				VARCHAR(50)		= NULL,	
	@fleet_name				VARCHAR(50)		= NULL,
	@car_segment_id			INT				= NULL,
	@car_class_id			INT				= NULL,
	@car_group_id			INT				= NULL,	
	@operstat_name			VARCHAR(300)	= NULL,
	@ownarea				VARCHAR(500)	= NULL,
	@modelcode				VARCHAR(500)	= NULL,
	@no_rev					INT				= NULL,
	@select_by				VARCHAR(10)		= NULL,
	@sortExpression			VARCHAR(50)		= NULL,
	@license				VARCHAR(25)		= NULL, -- Altered by Gavin 2/4/12
	@unit					VARCHAR(10)		= NULL, -- Altered by Gavin 2/4/12
	@serial					VARCHAR(25)		= NULL, -- Altered by Gavin 3/4/12
	@driverName				VARCHAR(25)		= NULL, -- Altered by Gavin 3/4/12
	@colour					VARCHAR(5)		= NULL, -- Altered by Gavin 3/4/12
	@mileage				NUMERIC(18,2)	= NULL,	-- Altered by Gavin 3/4/12
	@model					VARCHAR(50)		= NULL, -- Altered by Gavin 3/4/12
	@modelDesc				VARCHAR(50)		= NULL, -- Altered by Gavin 3/4/12
	@racfId					VARCHAR(10)		= NULL, -- Altered by Gavin 4/4/12
	@startRowIndex			INT				= NULL,
	@maximumRows			INT				= NULL

AS


BEGIN -- Create a table variable for paging and sorting


	IF @mileage = 0 SET @mileage = NULL

	DECLARE @CARSEARCH TABLE 
	(
		rowId		INT IDENTITY (1,1) NOT NULL,
		vc			VARCHAR(50) NULL,
		unit		VARCHAR(50) NULL,
		license		VARCHAR(50) NULL,
		moddesc		VARCHAR(50) NULL, -- altered by Gavin 2-5-12
		lstwwd		VARCHAR(50) NULL,
		lstdate		DATETIME NULL,
		duewwd		VARCHAR(50) NULL,
		duedate		DATETIME NULL,
		duetime		DATETIME NULL,
		op			VARCHAR(50) NULL,
		mt			VARCHAR(50) NULL,
		hold		VARCHAR(50) NULL,
		nr			VARCHAR(50) NULL,
		driver		VARCHAR(50) NULL,
		doc			VARCHAR(50) NULL,
		lstmlg		VARCHAR(50) NULL,
		serial		VARCHAR(50) NULL,
		remark		VARCHAR(500) NULL,
		erdpassed	VARCHAR(50) NULL
		,model		VARCHAR(50) NULL -- added by Gavin 2-5-12
		,rc			VARCHAR(50) NULL -- added by Gavin 2-5-12
		,lstTime	VARCHAR(50) NULL -- added by Gavin 2-5-12
		,iDate		VARCHAR(50) NULL -- added by Gavin 2-5-12
		,msoDate	VARCHAR(50) NULL -- added by Gavin 2-5-12
		,capDate	VARCHAR(50) NULL -- added by Gavin 2-5-12
		,ownArea	VARCHAR(50) NULL -- added by Gavin 2-5-12
		,carHold1	VARCHAR(50) NULL -- added by Gavin 2-5-12
		,bdDays		VARCHAR(50) NULL -- added by Gavin 2-5-12
		,mmDays		VARCHAR(50) NULL -- added by Gavin 2-5-12
		,resolution	VARCHAR(50) null -- added by Gavin 2-5-12
	)
	
	DECLARE @PAGING TABLE
	(
		pageIndex		INT IDENTITY (1,1) NOT NULL,
		rowId			INT NULL
	)
		
END

-- Alteration by Gavin 4/4/12 for MarsV3
-- Get the maximum amount of records that can be return according to the racfId of the user
declare @maxRows as int
set @maxRows = 500 -- Default
If Exists	
	(Select [noRows]
			From [dbo].[Mars_UserCarSearch]
			Where [racfId]=@racfId)
	begin
		set @maxRows = (Select [noRows]
			From [dbo].[Mars_UserCarSearch]
			Where [racfId]=@racfId)
	end

IF (@select_by = 'LSTWWD')
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Insert statements for procedure here
	INSERT INTO @CARSEARCH
	(vc,unit,license,moddesc,lstwwd,lstdate,duewwd,duedate,duetime,op,mt,hold,nr,driver,doc,lstmlg,serial,remark,erdpassed
		,model, rc, lstTime, iDate, msoDate, capDate, ownArea, carHold1, bdDays, mmDays, resolution)-- added by Gavin 2-5-12
	SELECT TOP (@maxRows)
		FEA.VC AS 'VC',
		CAST(FEA.UNIT AS INT) AS 'UNIT',
		FEA.LICENSE AS 'LICENSE',
		FEA.MODDESC AS 'MODDESC',	-- Altered from MODEL to MODDESC by Gavin 2-5-12	
		FEA.LSTWWD,
		FEA.LSTDATE AS 'LSTDATE',		
		FEA.DUEWWD,
		FEA.DUEDATE AS 'DUEDATE',
		FEA.DUETIME AS 'DUETIME',
		FEA.OPERSTAT AS 'OP',
		FEA.MOVETYPE AS 'MT',
		FEA.CARHOLD1 AS 'HOLD',
		FEA.DAYSREV AS 'NR',
		FEA.DRVNAME AS 'DRIVER',
		FEA.LSTNO AS 'DOC',
		CAST(FEA.LSTMLG AS INT) AS 'LSTMLG',
		FEA.SERIAL,
		LEFT(R.REMARK,50) AS 'REMARK',
		CASE WHEN (R.EXPECTEDRESOLUTIONDATE < GETDATE()) THEN 1 ELSE 0 END AS 'ERDPASSED'	
		,FEA.MODEL -- added by Gavin 2-5-12
		,FEA.RC
		,FEA.LSTTIME
		,FEA.IDATE 
		,FEA.MSODATE
		,FEA.CAPDATE 
		,FEA.OWNAREA
		,FEA.CARHOLD1
		,FEA.BDDAYS
		,FEA.MMDAYS 
		,FEA.RADATE -- to here
	FROM FLEET_EUROPE_ACTUAL AS FEA 
	LEFT JOIN VEHICLE_REMARKS AS R ON R.SERIAL = FEA.SERIAL 	
	WHERE
	(COUNTRY = @country OR @country IS NULL) -- Country
	AND ((LSTWWD IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
	AND ((LSTWWD IN (SELECT location FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
	AND ((LSTWWD IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
	AND ((LSTWWD IN (SELECT location FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
	AND (LSTWWD = @location OR @location IS NULL) -- Location
	
	AND (
			(@fleet_name = 'CARSALES' AND FLEET_CARSALES > 0) 
		OR 
			(@fleet_name = 'RAC OPS' AND FLEET_RAC_OPS > 0) 
		OR 
			(@fleet_name = 'RAC TTL' AND FLEET_RAC_TTL > 0) 
		OR 
			(@fleet_name = 'ADVANTAGE' AND FLEET_ADV > 0) 
		OR 
			(@fleet_name = 'HERTZ ON DEMAND' AND FLEET_HOD > 0) 
		OR 
			(@fleet_name = 'LICENSEE' AND FLEET_LICENSEE > 0) 
		OR 
			(@fleet_name IS NULL AND (FLEET_CARSALES > 0 OR FLEET_RAC_TTL > 0)
			AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)
			))--Fleet		
	
	AND ((VC IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
	AND ((VC IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
	AND ((VC = (SELECT car_group FROM CAR_GROUPS WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
	AND (@operstat_name IS NULL 		
		OR ('AVAILABLE FLEET' IN (SELECT * FROM fSplit(@operstat_name,',')) AND AVAILABLE_FLEET = 1)
		OR ('BD' IN (SELECT * FROM fSplit(@operstat_name,',')) AND BD = 1)
		OR ('WS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND WS = 1)		
		OR ('CARSALES' IN (SELECT * FROM fSplit(@operstat_name,',')) AND CARSALES = 1)
		OR ('CU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND CU = 1)
		OR ('FS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FS = 1)
		OR ('GOLD' IN (SELECT * FROM fSplit(@operstat_name,',')) AND GOLD = 1)
		OR ('HA' IN (SELECT * FROM fSplit(@operstat_name,',')) AND HA = 1)
		OR ('HL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND HL = 1)
		OR ('LL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND LL = 1)
		OR ('MM' IN (SELECT * FROM fSplit(@operstat_name,',')) AND MM = 1)
		OR ( 'NC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND NC = 1)
		OR ('ON RENT' IN (SELECT * FROM fSplit(@operstat_name,',')) AND ON_RENT = 1)
		OR ('OPERATIONAL FLEET' IN (SELECT * FROM fSplit(@operstat_name,',')) AND OPERATIONAL_FLEET = 1)
		OR ('OVERDUE' IN (SELECT * FROM fSplit(@operstat_name,',')) AND OVERDUE = 1)
		OR ('PL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND PL = 1)
		OR ('PREDELIVERY' IN (SELECT * FROM fSplit(@operstat_name,',')) AND PREDELIVERY = 1)
		OR ('RL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND RL = 1)
		OR ('RP' IN (SELECT * FROM fSplit(@operstat_name,',')) AND RP = 1)
		OR ('RT' IN (SELECT * FROM fSplit(@operstat_name,',')) AND RT = 1)
		OR ('SU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND SU = 1)
		OR ('SV' IN (SELECT * FROM fSplit(@operstat_name,',')) AND SV = 1)
		OR ('TB' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TB = 1)
		OR ('TC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TC = 1)
		OR ('TN' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TN = 1)
		OR ('TOTAL FLEET' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TOTAL_FLEET = 1)
		OR ('TW' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TW = 1)	
		
		OR ('UNK' IN (SELECT * FROM fSplit(@operstat_name,',')) AND OPERSTAT = 'UNK')-- UNK Status (Added by Javi 17/07/2012)
	) -- Status (OPERSTAT)
	AND ((OWNAREA IN (SELECT * FROM fSplit(@ownarea,','))) OR @ownarea IS NULL)  --Ownarea
	--AND ((MODEL IN (SELECT * FROM fSplit(@modelcode,','))) OR @modelcode IS NULL)  --Modelcode -- Removed by Gavin 2-5-12
	AND ((DAYSREV >= @no_rev) OR @no_rev IS NULL) -- No Rev
	and ((FEA.LICENSE like ('%' + @license + '%')) or @license is null) -- Altered by Gavin 2/4/12
	and ((FEA.UNIT like ('%' + @unit + '%')) or @unit is null) -- Altered by Gavin 2/4/12
	and ((FEA.SERIAL like ('%' + @serial + '%')) or @serial is null) -- Altered by Gavin 3/4/12
	and ((FEA.DRVNAME like ('%' + @driverName + '%')) or @driverName is null) -- Altered by Gavin 3/4/12
	and ((FEA.COLOR like ('%' + @colour + '%')) or @colour is null) -- Altered by Gavin 3/4/12
	and ((FEA.LSTMLG >= @mileage) or @mileage is null) -- Altered by Gavin 3/4/12
	and ((FEA.MODDESC like ('%' + @modelDesc + '%')) or @modelDesc is null) -- Altered by Gavin 2-5-12
	and ((FEA.MODEL like ('%' + @model + '%')) or @model is null) -- Altered by Gavin 2-5-12
	ORDER BY DAYSREV DESC

END

ELSE IF (@select_by = 'DUEWWD')
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO @CARSEARCH
	(vc,unit,license,moddesc,lstwwd,lstdate,duewwd,duedate,duetime,op,mt,hold,nr,driver,doc,lstmlg,serial,remark,erdpassed
			,model, rc, lstTime, iDate, msoDate, capDate, ownArea, carHold1, bdDays, mmDays, resolution)-- added by Gavin 2-5-12
	SELECT TOP (@maxRows)
		FEA.VC AS 'VC',
		CAST(FEA.UNIT AS INT) AS 'UNIT',
		FEA.LICENSE AS 'LICENSE',
		FEA.MODDESC AS 'MODDESC', 	-- Altered from MODEL to MODDESC by Gavin 2-5-12		
		FEA.LSTWWD,
		FEA.LSTDATE AS 'LSTDATE',		
		FEA.DUEWWD,
		FEA.DUEDATE AS 'DUEDATE',
		FEA.DUETIME AS 'DUETIME',
		FEA.OPERSTAT AS 'OP',
		FEA.MOVETYPE AS 'MT',
		FEA.CARHOLD1 AS 'HOLD',
		FEA.DAYSREV AS 'NR',
		FEA.DRVNAME AS 'DRIVER',
		FEA.LSTNO AS 'DOC',
		CAST(FEA.LSTMLG AS INT) AS 'LSTMLG',
		FEA.SERIAL,
		LEFT(R.REMARK,50) AS 'REMARK',
		CASE WHEN (R.EXPECTEDRESOLUTIONDATE < GETDATE()) THEN 1 ELSE 0 END AS 'ERDPASSED'
		,FEA.MODEL -- added by Gavin 2-5-12
		,FEA.RC
		,FEA.LSTTIME
		,FEA.IDATE 
		,FEA.MSODATE
		,FEA.CAPDATE 
		,FEA.OWNAREA
		,FEA.CARHOLD1
		,FEA.BDDAYS
		,FEA.MMDAYS 
		,FEA.RADATE -- to here
	FROM FLEET_EUROPE_ACTUAL AS FEA 
	LEFT JOIN VEHICLE_REMARKS AS R ON R.SERIAL = FEA.SERIAL 
	WHERE
	(COUNTRY = @country OR @country IS NULL) -- Country
	AND ((DUEWWD IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN CMS_LOCATION_GROUPS AS CLG ON L.cms_location_group_id = CLG.cms_location_group_id WHERE CLG.cms_pool_id = @cms_pool_id)) OR @cms_pool_id IS NULL) -- CMS_POOLS
	AND ((DUEWWD IN (SELECT location FROM LOCATIONS WHERE cms_location_group_id = @cms_location_group_id)) OR @cms_location_group_id IS NULL) -- CMS_LOCATION_GROUPS
	AND ((DUEWWD IN (SELECT L.location FROM LOCATIONS AS L INNER JOIN OPS_AREAS AS OA ON L.ops_area_id = OA.ops_area_id WHERE OA.ops_region_id = @ops_region_id)) OR @ops_region_id IS NULL) -- OPS_REGIONS
	AND ((DUEWWD IN (SELECT location FROM LOCATIONS WHERE ops_area_id = @ops_area_id)) OR @ops_area_id IS NULL) -- OPS_AREAS
	AND (DUEWWD = @location OR @location IS NULL) -- Location
	
		AND (
			(@fleet_name = 'CARSALES' AND FLEET_CARSALES > 0) 
		OR 
			(@fleet_name = 'RAC OPS' AND FLEET_RAC_OPS > 0) 
		OR 
			(@fleet_name = 'RAC TTL' AND FLEET_RAC_TTL > 0) 
		OR 
			(@fleet_name = 'ADVANTAGE' AND FLEET_ADV > 0) 
		OR 
			(@fleet_name = 'HERTZ ON DEMAND' AND FLEET_HOD > 0) 
		OR 
			(@fleet_name = 'LICENSEE' AND FLEET_LICENSEE > 0) 
		OR 
			(@fleet_name IS NULL AND (FLEET_CARSALES > 0 OR FLEET_RAC_TTL > 0)
			AND NOT(FLEET_ADV = 1) AND NOT(FLEET_HOD = 1) AND NOT(FLEET_LICENSEE = 1)
			))--Fleet		
	
	AND ((VC IN (SELECT CG.car_group FROM CAR_GROUPS AS CG INNER JOIN CAR_CLASSES AS CC ON CG.car_class_id = CC.car_class_id WHERE CC.car_segment_id = @car_segment_id)) OR @car_segment_id IS NULL) --@car_segment_id
	AND ((VC IN (SELECT CG.car_group FROM CAR_GROUPS AS CG WHERE CG.car_class_id = @car_class_id)) OR @car_class_id IS NULL) --@car_class_id
	AND ((VC = (SELECT car_group FROM CAR_GROUPS WHERE car_group_id = @car_group_id)) OR @car_group_id IS NULL) --@car_group_id
	AND (@operstat_name IS NULL 		
		OR ('AVAILABLE FLEET' IN (SELECT * FROM fSplit(@operstat_name,',')) AND AVAILABLE_FLEET = 1)
		OR ('BD' IN (SELECT * FROM fSplit(@operstat_name,',')) AND BD = 1)
		OR ('WS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND WS = 1)		
		OR ('CARSALES' IN (SELECT * FROM fSplit(@operstat_name,',')) AND CARSALES = 1)
		OR ('CU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND CU = 1)
		OR ('FS' IN (SELECT * FROM fSplit(@operstat_name,',')) AND FS = 1)
		OR ('GOLD' IN (SELECT * FROM fSplit(@operstat_name,',')) AND GOLD = 1)
		OR ('HA' IN (SELECT * FROM fSplit(@operstat_name,',')) AND HA = 1)
		OR ('HL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND HL = 1)
		OR ('LL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND LL = 1)
		OR ('MM' IN (SELECT * FROM fSplit(@operstat_name,',')) AND MM = 1)
		OR ( 'NC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND NC = 1)
		OR ('ON RENT' IN (SELECT * FROM fSplit(@operstat_name,',')) AND ON_RENT = 1)
		OR ('OPERATIONAL FLEET' IN (SELECT * FROM fSplit(@operstat_name,',')) AND OPERATIONAL_FLEET = 1)
		OR ('OVERDUE' IN (SELECT * FROM fSplit(@operstat_name,',')) AND OVERDUE = 1)
		OR ('PL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND PL = 1)
		OR ('PREDELIVERY' IN (SELECT * FROM fSplit(@operstat_name,',')) AND PREDELIVERY = 1)
		OR ('RL' IN (SELECT * FROM fSplit(@operstat_name,',')) AND RL = 1)
		OR ('RP' IN (SELECT * FROM fSplit(@operstat_name,',')) AND RP = 1)
		OR ('RT' IN (SELECT * FROM fSplit(@operstat_name,',')) AND RT = 1)
		OR ('SU' IN (SELECT * FROM fSplit(@operstat_name,',')) AND SU = 1)
		OR ('SV' IN (SELECT * FROM fSplit(@operstat_name,',')) AND SV = 1)
		OR ('TB' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TB = 1)
		OR ('TC' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TC = 1)
		OR ('TN' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TN = 1)
		OR ('TOTAL FLEET' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TOTAL_FLEET = 1)
		OR ('TW' IN (SELECT * FROM fSplit(@operstat_name,',')) AND TW = 1)	
		
		OR ('UNK' IN (SELECT * FROM fSplit(@operstat_name,',')) AND OPERSTAT = 'UNK') -- UNK Status (Added by Javi 17/07/2012)
			
	) -- Status (OPERSTAT)
	AND ((OWNAREA IN (SELECT * FROM fSplit(@ownarea,','))) OR @ownarea IS NULL)  --Ownarea
	--AND ((MODEL IN (SELECT * FROM fSplit(@modelcode,','))) OR @modelcode IS NULL)  --Modelcode --Remove by Gavin 2-5-12
	AND ((DAYSREV >= @no_rev) OR @no_rev IS NULL) -- No Rev
	and ((FEA.LICENSE like ('%' + @license + '%')) or @license is null) -- Altered by Gavin 2/4/12
	and ((FEA.UNIT like ('%' + @unit + '%')) or @unit is null) -- Altered by Gavin 2/4/12
	and ((FEA.SERIAL like ('%' + @serial + '%')) or @serial is null) -- Altered by Gavin 3/4/12
	and ((FEA.DRVNAME like ('%' + @driverName + '%')) or @driverName is null) -- Altered by Gavin 3/4/12
	and ((FEA.COLOR like ('%' + @colour + '%')) or @colour is null) -- Altered by Gavin 3/4/12
	and ((FEA.LSTMLG >= @mileage) or @mileage is null) -- Altered by Gavin 3/4/12
	and ((FEA.MODDESC like ('%' + @modelDesc + '%')) or @modelDesc is null) -- Altered by Gavin 2-5-12
	and ((FEA.MODEL like ('%' + @model + '%')) or @model is null) -- Altered by Gavin 2-5-12
	ORDER BY DAYSREV DESC

END


BEGIN -- Select Results

	-- INSERT INTO Paging Table
	INSERT INTO @PAGING (rowId) SELECT rowId FROM @CARSEARCH
	ORDER BY
	CASE WHEN @sortExpression = 'vc' THEN vc END ASC,
	CASE WHEN @sortExpression = 'vc DESC' THEN vc END DESC,
	CASE WHEN @sortExpression = 'unit' THEN unit END ASC,
	CASE WHEN @sortExpression = 'unit DESC' THEN unit END DESC,
	CASE WHEN @sortExpression = 'license' THEN license END ASC,
	CASE WHEN @sortExpression = 'license DESC' THEN license END DESC,
	CASE WHEN @sortExpression = 'model' THEN model END ASC,
	CASE WHEN @sortExpression = 'model DESC' THEN model END DESC,
	CASE WHEN @sortExpression = 'lstwwd' THEN lstwwd END ASC,
	CASE WHEN @sortExpression = 'lstwwd DESC' THEN lstwwd END DESC,
	CASE WHEN @sortExpression = 'lstdate' THEN lstdate END ASC,
	CASE WHEN @sortExpression = 'lstdate DESC' THEN lstdate END DESC,
	CASE WHEN @sortExpression = 'duedate' THEN duedate END ASC,
	CASE WHEN @sortExpression = 'duedate DESC' THEN duedate END DESC,
	CASE WHEN @sortExpression = 'duetime' THEN duetime END ASC,
	CASE WHEN @sortExpression = 'duetime DESC' THEN duetime END DESC,
	CASE WHEN @sortExpression = 'op' THEN op END ASC,
	CASE WHEN @sortExpression = 'op DESC' THEN op END DESC,
	CASE WHEN @sortExpression = 'mt' THEN mt END ASC,
	CASE WHEN @sortExpression = 'mt DESC' THEN mt END DESC,
	CASE WHEN @sortExpression = 'hold' THEN hold END ASC,
	CASE WHEN @sortExpression = 'hold DESC' THEN hold END DESC,
	CASE WHEN @sortExpression = 'nr' THEN nr END ASC,
	CASE WHEN @sortExpression = 'nr DESC' THEN nr END DESC,
	CASE WHEN @sortExpression = 'driver' THEN driver END ASC,
	CASE WHEN @sortExpression = 'driver DESC' THEN driver END DESC,
	CASE WHEN @sortExpression = 'doc' THEN doc END ASC,
	CASE WHEN @sortExpression = 'doc DESC' THEN doc END DESC,
	CASE WHEN @sortExpression = 'lstmlg' THEN lstmlg END ASC,
	CASE WHEN @sortExpression = 'lstmlg DESC' THEN lstmlg END DESC,
	CASE WHEN @sortExpression = 'serial' THEN serial END ASC,
	CASE WHEN @sortExpression = 'serial DESC' THEN serial END DESC,
	CASE WHEN @sortExpression = 'remark' THEN remark END ASC,
	CASE WHEN @sortExpression = 'remark DESC' THEN remark END DESC,
	CASE WHEN @sortExpression = 'erdpassed' THEN erdpassed END ASC,
	CASE WHEN @sortExpression = 'erdpassed DESC' THEN erdpassed END DESC

	--Select results
	SELECT 
		cs.vc,cs.unit,cs.license,cs.moddesc,cs.lstwwd,cs.lstdate,cs.duewwd,cs.duedate,
		cs.duetime,cs.op,cs.mt,cs.hold,cs.nr,cs.driver,cs.doc,cs.lstmlg,cs.serial,cs.remark,cs.erdpassed
		,cs.model, cs.rc, cs.lstTime, cs.iDate, cs.msoDate, cs.capDate, cs.ownArea, cs.carHold1, cs.bdDays -- added by Gavin 2-5-12
		,cs.mmDays, cs.resolution -- added by Gavin 2-5-12
	FROM @CARSEARCH cs
	INNER JOIN @PAGING p ON p.rowId = cs.rowId 
	WHERE (p.pageIndex BETWEEN @startRowIndex AND @maximumRows)
	ORDER BY p.pageIndex;

	--Select total row count
	SELECT COUNT(*) AS totalCount FROM @CARSEARCH;



END