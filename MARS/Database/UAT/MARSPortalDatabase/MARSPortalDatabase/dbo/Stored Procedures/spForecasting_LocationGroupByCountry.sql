-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_LocationGroupByCountry] 

	@year int,
	@week int,
	@country varchar(10)
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select FC.location_group as 'location_group_id',
			case when LG.cms_location_group is not NULL then LG.cms_location_group else 'n/a' end  + ' (' + FC.location_group + ')' as 'location_group'
	from MARS_Onrent_Forecast FC
		inner join countries C on FC.country = C.country_dw
		left join CMS_Location_groups LG on LG.cms_location_group_code_dw = FC.location_group
	where FC.[year] = @year and FC.week = @week and C.country = @country
	group by FC.location_group, LG.cms_location_group
	order by FC.location_group

END