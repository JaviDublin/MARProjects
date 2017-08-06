﻿
CREATE PROCEDURE [dbo].[AddUnmappedLocations]
AS
BEGIN
	SET NOCOUNT ON;



	select distinct v.LastLocationCode, substring(lastlocationCode, 0, 3) as country
	into #temp1
	from Vehicle v
	where v.LastLocationCode not in (select location from LOCATIONS)


	insert into locationS( location, location_dw, real_location_name, location_name, location_name_dw, active, ap_dt_rr, cal, cms_location_group_id, ops_area_id, served_by_locn, turnaround_hours, ownarea, location_area_id, city_desc, country)
	SELECT LastLocationCode, LastLocationCode, LastLocationCode, LastLocationCode, LastLocationCode
			, 1, case when substring(lastlocationCode, 6, 1) = '5' then 'AP' else 'DT' END 
			, 'L'
			, lg.cms_location_group_id
			, a.ops_area_id
			, LastLocationCode
			, 0, null, null
			, null
			, c.country
	FROM #temp1 t
	join COUNTRIES c on t.country = c.country
	join CMS_POOLS p on c.country = p.country
	join CMS_LOCATION_GROUPS lg on p.cms_pool_id = lg.cms_pool_id
	join OPS_REGIONS r on c.country = r.country
	join OPS_AREAS a on r.ops_region_id = a.ops_region_id
	where p.cms_pool = 'Unmapped'
		and lg.cms_location_group = 'unmapped'
		and r.ops_region = 'Unmapped'
		and a.ops_area = 'Unmapped'

END