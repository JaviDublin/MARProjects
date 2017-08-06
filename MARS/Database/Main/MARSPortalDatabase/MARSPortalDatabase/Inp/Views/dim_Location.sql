	CREATE view [Inp].[dim_Location]
	as
		select	l.dim_Location_id 
				, l.location 
				, l.location_area_id 
				, l.cms_location_group_id 
				, l.ops_area_id 
				, l.country 
		--		, c.country_description 
		--		, c.country_dw 
		from LOCATIONS l
		--	left join Countries c
		--		on l.country = c.country 