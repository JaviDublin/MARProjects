


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetCarVanForVcAndOwnwwd]
(
	@vc varchar(5),
	@ownwwd varchar(10)		
)
RETURNS varchar(2)
AS
BEGIN	
	DECLARE @carGroupName varchar(100)
	DECLARE @country varchar(2)	
	SELECT @country = LEFT(@ownwwd,(2))	
	SELECT @carGroupName = CS.car_segment
		FROM 
			CAR_SEGMENTS AS CS
			LEFT JOIN CAR_CLASSES AS CC ON CC.car_segment_id = CS.car_segment_id 
			LEFT JOIN CAR_GROUPS AS CG ON CG.car_class_id = CC.car_class_id
		WHERE 
			(CG.car_group = @vc)
			AND 
			(CS.country = @country)

	RETURN 
		(
		SELECT 
			CASE 
				WHEN (@carGroupName = 'Van') THEN 'V'
				ELSE 'C'
			END
		)
END







--set ANSI_NULLS ON
--set QUOTED_IDENTIFIER ON
--go
--
--
--
---- =============================================
---- Author:		<Author,,Name>
---- Create date: <Create Date, ,>
---- Description:	<Description, ,>
---- =============================================
--ALTER FUNCTION [dbo].[fnGetCarVanForVcAndOwnwwd]
--(
--	@vc varchar(5),
--	@ownwwd varchar(10)		
--)
--RETURNS varchar(2)
--AS
--BEGIN	
--	DECLARE @carGroupName varchar(100)
--	DECLARE @country_code varchar(2)	
--	SELECT @country_code = LEFT(@ownwwd,(2))	
--	SELECT @carGroupName = CG.group_name 
--		FROM 
--			CAR_CLASSES AS CC 
--			LEFT JOIN SEGMENTS AS S ON CC.segment_id = S.segment_id 
--			LEFT JOIN  CAR_GROUPS AS CG ON S.group_id = CG.group_id
--		WHERE 
--			(CC.car_class = @vc)
--			AND 
--			(CC.country_code = @country_code)
--
--	RETURN 
--		(
--		SELECT 
--			CASE 
--				WHEN (@carGroupName = 'Van') THEN 'V'
--				ELSE 'C'
--			END
--		)
--END
--
--
--
--
--