-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_LocationGroupByCountryUpdate]

	@year int,
	@week int,
	@country varchar(10),
	@locationGroup varchar(50),
	@carclass varchar(50),
	@targetField varchar(5),
	@targetValue float
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @locGroupDW varchar(50)
	set @locGroupDW = (select cms_location_group_code_dw from cms_location_groups where cms_location_group = @locationGroup)

	declare @countryCode varchar(10)
	set @countryCode = (select country_dw from Countries where country = @country)

	-- 1. TD
	IF @targetField = 'TD'
	BEGIN
		update mars_onrent_forecast
		set ONRENT_TD = @targetValue, TD_FLAG = 1
		where [year] = @year and week = @week and country = @countryCode
			and location_group = @locGroupDW and carclass = @carclass
	END
	-- 2. BU1
	ELSE IF @targetField = 'BU1'
	BEGIN
		update mars_onrent_forecast
		set ONRENT_BU_1 = @targetValue, BU_1_FLAG = 1
		where [year] = @year and week = @week and country = @countryCode
			and location_group = @locGroupDW and carclass = @carclass
	END
	-- 3. BU2
	ELSE IF @targetField = 'BU2'		
	BEGIN
		update mars_onrent_forecast
		set ONRENT_BU_2 = @targetValue, BU_2_FLAG = 1
		where [year] = @year and week = @week and country = @countryCode
			and location_group = @locGroupDW and carclass = @carclass
	END

	-- 4. RC
	ELSE IF @targetField = 'RC'		
	BEGIN
		update mars_onrent_forecast
		set ONRENT_RC = @targetValue
		where [year] = @year and week = @week and country = @countryCode
			and location_group = @locGroupDW and carclass = @carclass
	END

END