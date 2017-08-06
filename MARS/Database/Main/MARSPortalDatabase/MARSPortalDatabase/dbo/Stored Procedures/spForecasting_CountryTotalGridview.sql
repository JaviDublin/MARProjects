-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_CountryTotalGridview]

	@year int,
	@week int,
	@country varchar(10),
	@summary int
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @countryCode varchar(10)
	set @countryCode = (select country_dw from Countries where country = @country)

	IF @summary = 0 
	-- group by Location Group
	BEGIN
		select FC.[year], FC.week, @country as 'country', LG.cms_location_group as 'location_group', '(ALL)' as carclass,
				SUM(FCLY.onrent_DW) as 'onrent_LY',
				SUM(FC.onrent_R) as 'onrent_R', SUM(FC.onrent_TD) as 'onrent_TD', SUM(FC.onrent_BU_1) as 'onrent_BU_1', SUM(FC.onrent_BU_2) as 'onrent_BU_2'
				, SUM(FC.onrent_RC) as 'onrent_RC'
				, SUM(cast(FC.TD_FLAG as int)) as 'TD_FLAG', SUM(cast(FC.BU_1_FLAG as int)) as 'BU_1_FLAG', SUM(cast(FC.BU_2_FLAG as int)) as 'BU_2_FLAG'

		from mars_onrent_forecast FC
			left join mars_onrent_history FCLY on FC.[year]-1=FCLY.[year] and FC.week=FCLY.week and FC.country=FCLY.country and FC.location_group=FCLY.location_group and FC.carclass=FCLY.carclass
			left join CMS_Location_groups LG on LG.cms_location_group_code_dw = FC.location_group
			--inner join countries C on FC.country = C.country_dw
			

		where FC.[year] = @year and FC.week = @week and FC.country = @countryCode--C.country = @country

		group by FC.[year], FC.week, FC.country, FC.location_group, LG.cms_location_group

		order by FC.location_group

	END
	ELSE 
	-- group by Car Class
	BEGIN
		select FC.[year], FC.week, @country as 'country', '(ALL)' as 'location_group', FC.carclass as carclass,
				SUM(FCLY.onrent_DW) as 'onrent_LY',
				SUM(FC.onrent_R) as 'onrent_R', SUM(FC.onrent_TD) as 'onrent_TD', SUM(FC.onrent_BU_1) as 'onrent_BU_1', SUM(FC.onrent_BU_2) as 'onrent_BU_2'
				, SUM(FC.onrent_RC) as 'onrent_RC'
				, SUM(cast(FC.TD_FLAG as int)) as 'TD_FLAG', SUM(cast(FC.BU_1_FLAG as int)) as 'BU_1_FLAG', SUM(cast(FC.BU_2_FLAG as int)) as 'BU_2_FLAG'

		from mars_onrent_forecast FC
			left join mars_onrent_history FCLY on FC.[year]-1=FCLY.[year] and FC.week=FCLY.week and FC.country=FCLY.country and FC.location_group=FCLY.location_group and FC.carclass=FCLY.carclass
			left join CMS_Location_groups LG on LG.cms_location_group_code_dw = FC.location_group
			--inner join countries C on FC.country = C.country_dw

		where FC.[year] = @year and FC.week = @week and FC.country = @countryCode--C.country = @country

		group by FC.[year], FC.week, FC.country, FC.carclass

		order by FC.carclass

	END



END