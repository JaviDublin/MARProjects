-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Review_Fleet_OKC]
	
AS
BEGIN


	SET NOCOUNT ON;
	
	
	DELETE FROM FLEET_EUROPE_OKCFILE WHERE LASTCOLUMN <> ''
	
	
-- 1 - Trim main table values
--========================================================================================
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		SERIAL		= LTRIM(RTRIM(SERIAL))	,
		OWNAREA		= LTRIM(RTRIM(OWNAREA))	,
		OWNWWD		= LTRIM(RTRIM(OWNWWD))	,
		CTRYCODE	= LTRIM(RTRIM(CTRYCODE)),
		OPERSTAT	= LTRIM(RTRIM(OPERSTAT)),
		LSTWWD		= LTRIM(RTRIM(LSTWWD))	,
		PREVWWD		= LTRIM(RTRIM(PREVWWD))	, 
		DUEWWD		= LTRIM(RTRIM(DUEWWD))	,
		UNIT		= LTRIM(RTRIM(UNIT))	, 
		LICENSE		= LTRIM(RTRIM(LICENSE))

	delete from FLEET_EUROPE_OKCFILE
	where CTRYCODE not in (select country from COUNTRIES)

	SELECT TOP 100 CTRYCODE FROM FLEET_EUROPE_OKCFILE

-- 2 - Set Operational Status as UNK (Unknown) where OPERSTAT is blank or null
--========================================================================================
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		OPERSTAT = 'UNK'
	WHERE 
		(OPERSTAT IS NULL OR OPERSTAT = '')


-- 3 - Replace Company Code with Country Codes
--========================================================================================
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		CTRYCODE = C.country
	FROM [dbo].FLEET_EUROPE_OKCFILE AS FEO
	INNER JOIN 
	(select company , country from [dbo].COMPANIES where company <> country) AS C
	ON FEO.CTRYCODE = C.company
		
		
-- 4 - Delete Invalid Rows (No Area Code or No Location)
--========================================================================================
	
	DELETE FROM [dbo].FLEET_EUROPE_OKCFILE 
	WHERE 
		((OWNAREA IS NULL) 
			OR 
		(OWNAREA = '')
			OR
		(ISNUMERIC(OWNAREA) = 0))
		
	
-- 5 - Group of queries to get Country Codes where CC are blank
--========================================================================================
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		CTRYCODE = SUBSTRING(LSTWWD,1,2)
	FROM 
		[dbo].FLEET_EUROPE_OKCFILE 
	WHERE 
		CTRYCODE = '' and LSTWWD <> ''
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		CTRYCODE = A.country
	FROM 
		[dbo].FLEET_EUROPE_OKCFILE FEO
	INNER JOIN [dbo].AREACODES AS A ON FEO.OWNAREA = A.ownarea
	WHERE 
		FEO.CTRYCODE = ''
		
	UPDATE [dbo].AREACODES SET
		country = FEO.CTRYCODE
	FROM [dbo].AREACODES A
	INNER JOIN [dbo].FLEET_EUROPE_OKCFILE AS FEO ON A.ownarea = FEO.OWNAREA
	WHERE 
		A.country = 'NN' AND FEO.CTRYCODE <> 'NN'
		
	DELETE FROM [dbo].FLEET_EUROPE_OKCFILE 
	WHERE 
		CTRYCODE NOT IN (SELECT country FROM [dbo].COUNTRIES)
		

		
-- 5 (1/2) NEW ADDITION FOR VHICLES SUBLEASE
--========================================================================================		
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		CTRYCODE = GVL.Country_Rent
	FROM
		[dbo].FLEET_EUROPE_OKCFILE FEO
	INNER JOIN
		[General].VEHICLE_LEASE GVL ON 
			FEO.SERIAL	 = GVL.Serial 
		AND FEO.CTRYCODE = GVL.Country_Owner
			
			
-- 6 - Set Location Code Unmmapped where the Location Code is null
--========================================================================================

	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		LSTWWD = L.location
	FROM [dbo].FLEET_EUROPE_OKCFILE FEO
	INNER JOIN LOCATIONS AS L ON FEO.CTRYCODE =  L.country AND L.location_name = 'UNMAPPED'
	WHERE 
		(FEO.LSTWWD IS NULL OR FEO.LSTWWD = '')
		
	DELETE FROM [dbo].FLEET_EUROPE_OKCFILE 
	WHERE
		(LSTWWD IS NULL OR LSTWWD = '')

	
-- 7 - Update OWNWWD with the Country Code where is blank and Location with Location Code
--========================================================================================	
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		OWNWWD = CTRYCODE
	WHERE 
		OWNWWD = ''
			
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		LSTWWD = L.location
	FROM 
		[dbo].FLEET_EUROPE_OKCFILE FEO
	INNER JOIN [dbo].LOCATIONS AS L ON FEO.LSTWWD = L.location_area_id
	WHERE 
		ISNUMERIC(FEO.LSTWWD) = 1
		
		
-- 8 - Replace Area Code with Country Codes in OWNWWD
--========================================================================================	
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		OWNWWD = A.country
	FROM 
		[dbo].FLEET_EUROPE_OKCFILE AS FEO
	INNER JOIN [dbo].AREACODES AS A ON FEO.OWNAREA = A.ownarea
	WHERE 
		ISNUMERIC(OWNWWD) = 1


-- 9 - Add to the system valid Area Codes that don't exist in the Database as Licence (default)
--========================================================================================
	
	INSERT INTO [dbo].AREACODES
	
	SELECT 
		FEO.OWNAREA , ISNULL(C.country,'NN') , 'UNKNOWN' , 0 , 0 , 0 , 1 , 0 , 0
	FROM
		[dbo].FLEET_EUROPE_OKCFILE FEO
	INNER JOIN [dbo].COMPANIES AS C ON
		FEO.CTRYCODE = C.company
	WHERE FEO.OWNAREA NOT IN 
		(SELECT OWNAREA FROM [dbo].AREACODES)
	GROUP BY 
		FEO.OWNAREA , C.country

		
-- 10 - Remove Duplicate Serials (FleetCo by default)
--========================================================================================
	;WITH DuplicateEntries AS
		(
			SELECT 
				LEFT(LTRIM(RTRIM(feo.SERIAL)), 25)  AS "SERIAL"  ,
				LEFT(feo.OWNAREA	, 10)			AS "OWNAREA" ,
				LEFT(feo.VENDNBR	, 10)			AS "VENDNBR" ,
				LTRIM(RTRIM(feo.CTRYCODE))			AS "CTRYCODE",
				CASE WHEN 
				ROW_NUMBER() OVER
				(
					PARTITION BY 
						feo.SERIAL 
					ORDER BY 
						feo.SERIAL ,  ac.OpCo DESC
				)  = 1 
				THEN 0 
				ELSE 1 
				END									AS "IsDuplicate",
				ac.OpCo								AS "IsOpco"
		
			FROM 
				[dbo].FLEET_EUROPE_OKCFILE feo
			INNER JOIN 
				[dbo].AREACODES AS ac ON feo.OWNAREA = ac.ownarea
		)
		
		DELETE rms FROM 
			[dbo].FLEET_EUROPE_OKCFILE as rms
		INNER JOIN DuplicateEntries ON
			rms.SERIAL   = DuplicateEntries.SERIAL
		AND rms.OWNAREA  = DuplicateEntries.OWNAREA
		AND rms.VENDNBR  = DuplicateEntries.VENDNBR
		AND rms.CTRYCODE = DuplicateEntries.CTRYCODE
		AND DuplicateEntries.IsDuplicate    = 1
		AND DuplicateEntries.IsOpco			= 0
		
				
-- 11 - Add new locations to the system
--========================================================================================	
	
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		LSTWWD = loc.location
	FROM
		[dbo].FLEET_EUROPE_OKCFILE feo
	INNER JOIN 
		LOCATIONS AS loc on feo.LSTWWD = loc.location_area_id
		
	UPDATE [dbo].FLEET_EUROPE_OKCFILE SET
		LSTWWD = loc.location
	FROM
		[dbo].FLEET_EUROPE_OKCFILE feo
	INNER JOIN 
		LOCATIONS AS loc on feo.CTRYCODE = loc.country AND loc.location_name = 'UNMAPPED' 
	WHERE
		ISNUMERIC(feo.LSTWWD) = 1
	
	-- Excluding Switzerland Locations
	-----------------------------------------------------------------
	INSERT INTO LOCATIONS
		
	SELECT 
		FEOKC.LSTWWD						AS "location"				, 
		FEOKC.LSTWWD						AS "location_dw"			, 
		FEOKC.LSTWWD						AS "real_location_name"		, 
		FEOKC.LSTWWD						AS "location_name"			, 
		FEOKC.LSTWWD						AS "location_name_dw"		,	 
		1									AS "active"					, 
		CASE 
			WHEN SUBSTRING(FEOKC.LSTWWD,6,1) = '5' THEN 'AP' 
			ELSE 'DT' 
		END 
											AS "ap_dt_rr"				,
		'L'									AS "cal"					,
		LG.cms_location_group_id			AS "cms_location_group_id"	,
		AR.ops_area_id						AS "ops_area_id"			,
		FEOKC.LSTWWD						AS "served_by_locn"			,
		0									AS "turnaround_hours"		,			
		null AS "ownarea",
		null AS "location_area_id",
		null								AS "city_desc"				,
		FEOKC.CTRYCODE						AS "country"
		--SUBSTRING(FEOKC.LSTWWD,1,2)			AS "country" 
	FROM 
		[dbo].FLEET_EUROPE_OKCFILE FEOKC 
	INNER JOIN OPS_REGIONS			AS RG ON 
		FEOKC.CTRYCODE = RG.country AND RG.ops_region	= 'UNMAPPED'
		--SUBSTRING(FEOKC.LSTWWD,1,2) = RG.country AND RG.ops_region	= 'UNMAPPED'
	INNER JOIN OPS_AREAS			AS AR ON 
		RG.ops_region_id = AR.ops_region_id AND AR.ops_area			= 'UNMAPPED'
	INNER JOIN CMS_POOLS			AS PO ON 
		FEOKC.CTRYCODE  = PO.country AND PO.cms_pool	= 'UNMAPPED'
		--SUBSTRING(FEOKC.LSTWWD,1,2) = PO.country AND PO.cms_pool	= 'UNMAPPED'
	INNER JOIN CMS_LOCATION_GROUPS	AS LG ON 
		PO.cms_pool_id = LG.cms_pool_id AND LG.cms_location_group	= 'UNMAPPED'
	WHERE 
		FEOKC.LSTWWD NOT IN (SELECT LOCATION FROM LOCATIONS)
	
	AND  FEOKC.CTRYCODE NOT IN ('SZ','CH') -- Added 01/Oct/2012 by Javi
	AND  SUBSTRING(FEOKC.LSTWWD,1,2) = FEOKC.CTRYCODE -- Added 11/Apr/2013 by Javi
	
	GROUP BY 
		FEOKC.LSTWWD , AR.ops_area_id , LG.cms_location_group_id ,  FEOKC.CTRYCODE	
	
	
	-- Only Switzerland Locations (and Area Code start with 7)
	-----------------------------------------------------------------
	
	--INSERT INTO LOCATIONS
		
	--SELECT 
	--	FEOKC.LSTWWD						AS "location"				, 
	--	FEOKC.LSTWWD						AS "location_dw"			, 
	--	FEOKC.LSTWWD						AS "real_location_name"		, 
	--	FEOKC.LSTWWD						AS "location_name"			, 
	--	FEOKC.LSTWWD						AS "location_name_dw"		,	 
	--	1									AS "active"					, 
	--	CASE 
	--		WHEN SUBSTRING(FEOKC.LSTWWD,6,1) = '5' THEN 'AP' 
	--		ELSE 'DT' 
	--	END 
	--										AS "ap_dt_rr"				,
	--	'L'									AS "cal"					,
	--	LG.cms_location_group_id			AS "cms_location_group_id"	,
	--	AR.ops_area_id						AS "ops_area_id"			,
	--	FEOKC.LSTWWD						AS "served_by_locn"			,
	--	0									AS "turnaround_hours"		,			
	--	ISNULL(FEOKC.ownarea,'000' + SUBSTRING(LSTWWD,1,2))	AS "ownarea"			,
	--		ISNULL(FEOKC.ownarea,'000' + SUBSTRING(LSTWWD,1,2)) 
	--	 + SUBSTRING(LSTWWD,6,2)							AS "location_area_id"	,
	--	null								AS "city_desc"				,
	--	FEOKC.CTRYCODE						AS "country"
	--	--SUBSTRING(FEOKC.LSTWWD,1,2)			AS "country" 
	--FROM 
	--	[dbo].FLEET_EUROPE_OKCFILE FEOKC 
	--INNER JOIN OPS_REGIONS			AS RG ON 
	--	FEOKC.CTRYCODE = RG.country AND RG.ops_region	= 'UNMAPPED'
	--	--SUBSTRING(FEOKC.LSTWWD,1,2) = RG.country AND RG.ops_region	= 'UNMAPPED'
	--INNER JOIN OPS_AREAS			AS AR ON 
	--	RG.ops_region_id = AR.ops_region_id AND AR.ops_area			= 'UNMAPPED'
	--INNER JOIN CMS_POOLS			AS PO ON 
	--	FEOKC.CTRYCODE  = PO.country AND PO.cms_pool	= 'UNMAPPED'
	--	--SUBSTRING(FEOKC.LSTWWD,1,2) = PO.country AND PO.cms_pool	= 'UNMAPPED'
	--INNER JOIN CMS_LOCATION_GROUPS	AS LG ON 
	--	PO.cms_pool_id = LG.cms_pool_id AND LG.cms_location_group	= 'UNMAPPED'
	--WHERE 
	--	FEOKC.LSTWWD NOT IN (SELECT LOCATION FROM LOCATIONS)
	
	--AND  FEOKC.CTRYCODE IN ('SZ','CH') AND SUBSTRING(FEOKC.OWNAREA,1,1) = '7' -- Added 01/Oct/2012 by Javi
	
	--GROUP BY 
	--	FEOKC.LSTWWD , AR.ops_area_id , LG.cms_location_group_id , FEOKC.ownarea	, FEOKC.CTRYCODE	

	-- Only Switzerland Locations (and Area Code different than 7)
	-----------------------------------------------------------------
	INSERT INTO LOCATIONS
		
	SELECT 
		FEOKC.LSTWWD						AS "location"				, 
		FEOKC.LSTWWD						AS "location_dw"			, 
		FEOKC.LSTWWD						AS "real_location_name"		, 
		FEOKC.LSTWWD						AS "location_name"			, 
		FEOKC.LSTWWD						AS "location_name_dw"		,	 
		1									AS "active"					, 
		CASE 
			WHEN SUBSTRING(FEOKC.LSTWWD,6,1) = '5' THEN 'AP' 
			ELSE 'DT' 
		END 
											AS "ap_dt_rr"				,
		'L'									AS "cal"					,
		LG.cms_location_group_id			AS "cms_location_group_id"	,
		AR.ops_area_id						AS "ops_area_id"			,
		FEOKC.LSTWWD						AS "served_by_locn"			,
		0									AS "turnaround_hours"		,			
		ISNULL(FEOKC.ownarea,'000' + SUBSTRING(LSTWWD,1,2))	AS "ownarea"			,
			ISNULL(FEOKC.ownarea,'000' + SUBSTRING(LSTWWD,1,2)) 
		 + SUBSTRING(LSTWWD,6,2)							AS "location_area_id"	,
		null								AS "city_desc"				,
		FEOKC.CTRYCODE						AS "country"
		--SUBSTRING(FEOKC.LSTWWD,1,2)			AS "country" 
	FROM 
		[dbo].FLEET_EUROPE_OKCFILE FEOKC 
	INNER JOIN OPS_REGIONS			AS RG ON 
		FEOKC.CTRYCODE = RG.country AND RG.ops_region	= 'UNMAPPED'
		--SUBSTRING(FEOKC.LSTWWD,1,2) = RG.country AND RG.ops_region	= 'UNMAPPED'
	INNER JOIN OPS_AREAS			AS AR ON 
		RG.ops_region_id = AR.ops_region_id AND AR.ops_area			= 'UNMAPPED'
	INNER JOIN CMS_POOLS			AS PO ON 
		FEOKC.CTRYCODE  = PO.country AND PO.cms_pool	= 'UNMAPPED'
		--SUBSTRING(FEOKC.LSTWWD,1,2) = PO.country AND PO.cms_pool	= 'UNMAPPED'
	INNER JOIN CMS_LOCATION_GROUPS	AS LG ON 
		PO.cms_pool_id = LG.cms_pool_id AND LG.cms_location_group	= 'UNMAPPED'
	WHERE 
		FEOKC.LSTWWD NOT IN (SELECT LOCATION FROM LOCATIONS)
	
	AND  FEOKC.CTRYCODE IN ('SZ','CH') AND SUBSTRING(FEOKC.OWNAREA,1,1) <> '7' -- Added 01/Oct/2012 by Javi
	
	GROUP BY 
		FEOKC.LSTWWD , AR.ops_area_id , LG.cms_location_group_id , FEOKC.ownarea	, FEOKC.CTRYCODE		
		
-- 12 - Add new locations to the Table dim_Location (New)
--========================================================================================	
   
	INSERT INTO inp.dim_Location (location , cms_location_group_id , ops_area_id)
	SELECT 
		location , cms_location_group_id , ops_area_id
	FROM [dbo].LOCATIONS
	WHERE location NOT IN (SELECT location FROM inp.dim_Location)
	GROUP BY 
		location , cms_location_group_id , ops_area_id
   
   
END