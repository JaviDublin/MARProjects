
CREATE VIEW N1GoldGroupUpgrade
AS
select  N1Type, lp.ResGoldLevelId
	, cg.car_group as CarGroup
	, cg.car_group_id as CarGroupId
	, upgradedCg.car_group_id as UpgradedCarGroupId
	, cs.country
from CAR_GROUPS cg
join CAR_CLASSES cc on cg.car_class_id = cc.car_class_id
join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
cross apply (
	select N1Type, lp.ResGoldLevelId
	from [dbo].[ResLoyaltyProgram] lp
	join [dbo].[ResGoldLevel] gl on lp.ResGoldLevelId = gl.ResGoldLevelId
) lp
join CAR_GROUPS upgradedCg on case ResGoldLevelId 
					when 1 then cg.car_group 
					when 2 then cg.car_group_fivestar
					when 3 then cg.car_group_gold
					when 4 then cg.car_group_presidentCircle
					when 5 then cg.car_group_platinum
				end = upgradedCg.car_group
join CAR_CLASSES upgradedCc on upgradedCg.car_class_id = upgradedCc.car_class_id
join CAR_SEGMENTS upgradedCs on upgradedCc.car_segment_id = upgradedCs.car_segment_id
where upgradedcs.country = cs.country