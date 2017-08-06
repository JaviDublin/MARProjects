


CREATE FUNCTION [dbo].[fnResGetUpgradedCarGroupId]
(
	@ReservedCarGroup varchar(10),
	@N1Type  varchar(10),
	@Country  varchar(10)
)
RETURNS int
AS
BEGIN
	DECLARE @UpgradedCarGroupId int
	
	declare @UpgradedCarGroup varchar(10)

	set @UpgradedCarGroup = 
	(	
			select top 1
			case  (
						select gl.ResGoldLevelId
						from [dbo].[ResLoyaltyProgram] lp
						join [dbo].[ResGoldLevel] gl on lp.ResGoldLevelId = gl.ResGoldLevelId
						where N1Type =  @N1Type
					) when 1 then cg.car_group 
					when 2 then cg.car_group_fivestar
					when 3 then cg.car_group_gold
					when 4 then cg.car_group_presidentCircle
					when 5 then cg.car_group_platinum
				end as UpgradedCarGroup
			from CAR_GROUPS cg
			join CAR_CLASSES cc on cg.car_class_id = cc.car_class_id
			join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
			where car_group = @ReservedCarGroup
				and cs.country = @Country
	)


	set @UpgradedCarGroupId = 
	(
		select cg.car_group_id
		from CAR_GROUPS cg
		join CAR_CLASSES cc on cg.car_class_id = cc.car_class_id
		join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
		where cg.car_group = @UpgradedCarGroup 
		and cs.country =  @Country
	)



	RETURN	@UpgradedCarGroupId		

END