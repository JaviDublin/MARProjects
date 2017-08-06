-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_LocationGroupByCountryGridview]

	@year int,
	@week int,
	@country varchar(10),
	@locationGroup varchar(50)
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select 	FC.[year], FC.week, C.country, LG.cms_location_group as 'location_group', FC.carclass, 
			FCLY.onrent_DW as 'onrent_LY',
			FC.onrent_R, FC.onrent_TD, FC.onrent_BU_1, FC.onrent_BU_2, FC.onrent_RC
			, cast(FC.TD_FLAG as int) as 'TD_FLAG', cast(FC.BU_1_flag as int) as 'BU_1_FLAG', cast(FC.BU_2_flag as int) as 'BU_2_FLAG'

	from mars_onrent_forecast FC
		inner join countries C on FC.country = C.country_dw
		left join mars_onrent_history FCLY on FC.[year]-1=FCLY.[year] and FC.week=FCLY.week and FC.country=FCLY.country and FC.location_group=FCLY.location_group and FC.carclass=FCLY.carclass
		left join CMS_Location_groups LG on LG.cms_location_group_code_dw = FC.location_group

	where FC.[year] = @year and FC.week = @week and C.country = @country
		and FC.location_group = @locationGroup

	order by FC.location_group, FC.carclass

END