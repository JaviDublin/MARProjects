-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_ReportForecasting]

	@year int,
	@week int,

	@country varchar(10),
	@pool_id int, --@pool varchar(50),
	@locationGroup varchar(50),
	@carclass_id int --@carclass varchar(50)
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @sYear int, @sWeek int, @eYear int, @eWeek int

	set @sYear = @year			set @eYear = @year
	set @sWeek = (@week - 5)	set @eWeek = (@week + 10)
	-- start week
	IF @sWeek < 1
	BEGIN
		set @sYear = (@sYear - 1)		set @sWeek = (@sWeek + 52)
	END
	-- end week
	IF @eWeek > 52
	BEGIN
		set @eYear = (@eYear + 1)		set @eWeek = (@eWeek -52)
	END

	--select  @sYear, @sWeek, @eYear, @eWeek

	SELECT FC.[year], FC.week,	--C.country as 'country', FC.location_group, FC.carclass as carclass,
			cast(cast(FC.[year] as char(4)) + '.' + RIGHT('0' + cast(FC.week as varchar(2)), 2) as varchar(10)) as 'yearWeek',

				SUM(H.onrent_DW) as 'onrent_DW', SUM(H.onrent_AT) as 'onrent_AT', SUM(H.onrent_FC) as 'onrent_FC',
				SUM(FC.onrent_R) as 'onrent_R', SUM(FC.onrent_TD) as 'onrent_TD', SUM(FC.onrent_BU_1) as 'onrent_BU_1', SUM(FC.onrent_BU_2) as 'onrent_BU_2', SUM(FC.onrent_RC) as 'onrent_RC',
				SUM(FCLY.onrent_DW) as 'onrent_LY'

	FROM mars_onrent_forecast FC
			inner join countries C on FC.country = C.country_dw
			inner join CMS_Location_Groups LG on LG.cms_location_group_code_dw = FC.location_group
			inner join CMS_Pools P on P.cms_pool_id = LG.cms_pool_id			
			inner join CAR_CLASSES CC on CC.car_class = FC.carclass
			inner join CAR_segments CS on CS.car_segment_id = CC.car_segment_id
			left join mars_onrent_history H on FC.[year]=H.[year] and FC.week=H.week and FC.country=H.country and FC.location_group=H.location_group and FC.carclass=H.carclass
			left join mars_onrent_history FCLY on FC.[year]-1=FCLY.[year] and FC.week=FCLY.week and FC.country=FCLY.country and FC.location_group=FCLY.location_group and FC.carclass=FCLY.carclass

	WHERE ((FC.[YEAR] > @sYear or (FC.[YEAR] = @sYear and FC.WEEK >= @sWeek)) AND (FC.[YEAR] < @eYear or (FC.[YEAR] = @eYear and FC.WEEK <= @eWeek)))
			and (C.country = @country or @country = 'ALL')
			and (P.cms_pool_id = @pool_id or @pool_id = -1)
			and (LG.cms_location_group_code = @locationGroup or @locationGroup = 'ALL') --and (FC.location_group = @locationGroup or @locationGroup = 'ALL') 
			and (CC.car_class_id = @carclass_id or @carclass_id = -1) --and (FC.carclass = @carclass or @carclass = 'ALL')

	GROUP BY FC.[year], FC.week	--, C.country	, FC.location_group, FC.carclass

	ORDER BY FC.[year], FC.week	--, C.country


END