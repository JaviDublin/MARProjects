CREATE procedure [dbo].[proc_RepresentiveLocationByLocationGroupSelect]

	@Country varchar(2)
	
AS
BEGIN
--	=========================================================================
--	select one representive(first) location by location group
--	==========================================================================

	SET NOCOUNT ON;
	
	-- MERGE Necessary Fleet table with re-calculation of Utilisation

		SELECT L.cms_location_group_id, L.location
		FROM
		(
			SELECT CMSL.country, L.cms_location_group_id, L.location, RANK() over (Partition BY L.cms_location_group_id order by L.location ASC) AS [RANK]
			FROM LOCATIONS L
				INNER JOIN vw_Mapping_CMS_Location CMSL ON CMSL.location = L.location
			WHERE (CMSL.country = @Country OR @Country IS NULL)
		) L
		WHERE L.[RANK] = 1

END


-- EXEC [proc_RepresentiveLocationByLocationGroupSelect]'GE'