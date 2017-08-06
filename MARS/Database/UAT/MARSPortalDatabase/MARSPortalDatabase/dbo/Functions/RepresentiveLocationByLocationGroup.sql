CREATE FUNCTION [dbo].[RepresentiveLocationByLocationGroup](@COUNTRY varchar(2))
RETURNS @Results TABLE (CMS_LOCATION_GROUP_ID int, Location varchar(50))
AS

        BEGIN      
        
    INSERT INTO @Results
    SELECT L.cms_location_group_id, L.location
	FROM
	(
		SELECT 
			CMSL.country, 
			L.cms_location_group_id, 
			L.location, RANK() over (Partition BY L.cms_location_group_id order by L.location ASC) AS [RANK]
		FROM LOCATIONS L
			INNER JOIN vw_Mapping_CMS_Location CMSL ON CMSL.location = L.location
		WHERE (CMSL.country = @Country OR @Country IS NULL)
	) L
	WHERE L.[RANK] = 1


	
    RETURN
END


-- select * from [RepresentiveLocationByLocationGroup]('GE')