
CREATE PROCEDURE [dbo].[AddUnmappedCarSegmentsClassesAndGroups]
AS
BEGIN
	SET NOCOUNT ON;

	
	SELECT distinct h.COUNTRY , h.vc
	into #MissingCarGroups
	FROM [dbo].FLEET_EUROPE_ACTUAL h 
	where h.COUNTRY + ' ' + h.vc not in (

		select cs.country + ' ' + cg.car_group as foundCarGroups
		from CAR_GROUPS cg
		join CAR_CLASSES cc on cg.car_class_id = cc.car_class_id
		join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
	)


	INSERT INTO CAR_SEGMENTS(car_segment, country, sort_car_segment)
	SELECT distinct 'UNMAPPED', country, 99
	FROM #MissingCarGroups
	where country not in (select distinct country from CAR_SEGMENTS where car_segment = 'UNMAPPED')

	INSERT INTO CAR_CLASSES(car_class, car_segment_id, sort_car_class)
	SELECT distinct 'UNMAPPED', cs.car_segment_id, 99
	FROM #MissingCarGroups
	join CAR_SEGMENTS cs on #MissingCarGroups.COUNTRY = cs.country
	where cs.car_segment_id not in (select car_segment_id from CAR_CLASSES where car_class = 'UNMAPPED')

	INSERT INTO CAR_GROUPS( car_group, car_group_gold, car_class_id, sort_car_group, car_group_fivestar, car_group_presidentCircle, car_group_platinum )
	SELECT distinct  mcg.vc, mcg.vc, unmappedClasses.car_class_id, 99, mcg.vc,mcg.vc,mcg.vc--, unmappedClasses.country
	FROM #MissingCarGroups mcg
	join 
	(
		select cc.car_class_id, cs.country
		from CAR_CLASSES cc 
		join CAR_SEGMENTS cs on cc.car_segment_id = cs.car_segment_id
		where cc.car_class = 'UNMAPPED' and cs.car_segment = 'UNMAPPED'
	) unmappedClasses on mcg.COUNTRY = unmappedClasses.country

END