-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spForecasting_CountryTotalUpdate]

	@year int,
	@week int,
	@country varchar(10),
	@locationGroup varchar(50),		-- 'ALL' or location group
	@carclass varchar(50),			-- 'ALL' or car class

	@isAdopt bit,	
	@isIncrease bit,
	@increasePercentage float,
	@sourceField varchar(5),
	@targetField varchar(5)
		
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

	-- 1. Adopt
	IF @isAdopt = 1
	BEGIN
		-- 1. TD
		IF @sourceField = 'TD'
		BEGIN
			-- 1. TD // TD -> TD
			IF @targetField = 'TD'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_TD = ONRENT_TD, TD_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 2. BU1 // TD -> BU1
			ELSE IF @targetField = 'BU1'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_1 = ONRENT_TD, BU_1_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 3. BU2 // TD -> BU2
			ELSE IF @targetField = 'BU2'		
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_2 = ONRENT_TD, BU_2_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 4. RC // TD -> RC
			ELSE IF @targetField = 'RC'		
			BEGIN
				update mars_onrent_forecast
				set ONRENT_RC = ONRENT_TD
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
		END
	-- 2. BU1
		ELSE IF @sourceField = 'BU1'
		BEGIN
			-- 1. TD // BU1 -> TD
			IF @targetField = 'TD'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_TD = ONRENT_BU_1, TD_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 2. BU1 // BU1 -> BU1
			ELSE IF @targetField = 'BU1'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_1 = ONRENT_BU_1, BU_1_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 3. BU2 // BU1 -> BU2
			ELSE IF @targetField = 'BU2'		
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_2 = ONRENT_BU_1, BU_2_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 4. RC // BU1 -> RC
			ELSE IF @targetField = 'RC'		
			BEGIN
				update mars_onrent_forecast
				set ONRENT_RC = ONRENT_BU_1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
		END
	-- 3. BU2
		ELSE IF @sourceField = 'BU2'
		BEGIN
			-- 1. TD // BU2 -> TD
			IF @targetField = 'TD'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_TD = ONRENT_BU_2, TD_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 2. BU1 // BU2 -> BU1
			ELSE IF @targetField = 'BU1'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_1 = ONRENT_BU_2, BU_1_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 3. BU2 // BU2 -> BU2
			ELSE IF @targetField = 'BU2'		
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_2 = ONRENT_BU_2, BU_2_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 4. RC // BU2 -> RC
			ELSE IF @targetField = 'RC'		
			BEGIN
				update mars_onrent_forecast
				set ONRENT_RC = ONRENT_BU_2
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
		END
	END

	-- 2. Increase %
	ELSE IF @isIncrease = 1
	BEGIN
		-- 1. TD
			IF @targetField = 'TD'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_TD = (ONRENT_TD + (ONRENT_TD* @increasePercentage)/100.0), TD_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 2. BU1
			ELSE IF @targetField = 'BU1'
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_1 = (ONRENT_BU_1 + (ONRENT_BU_1* @increasePercentage)/100.0), BU_1_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
			-- 3. BU2
			ELSE IF @targetField = 'BU2'		
			BEGIN
				update mars_onrent_forecast
				set ONRENT_BU_2 = (ONRENT_BU_2 + (ONRENT_BU_2* @increasePercentage)/100.0), BU_2_FLAG = 1
				where [year] = @year and week = @week and country = @countryCode
					and (location_group = @locGroupDW or @locationGroup = 'ALL') and (carclass = @carclass or @carclass = 'ALL')
			END
	END

END