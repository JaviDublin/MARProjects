
-- =============================================
-- Author:		Gavin	
-- Create date: 23/3/12
-- Description:	Inserts or updates the forecast history db
-- =============================================
CREATE PROCEDURE [dbo].[forecastHistory_V1_0]

	@theday as varchar(50),
	@u as varchar(50), -- CMS_UNCONSTRAINEDXXXX, where XXXX = { '', '_WK1', '_WK2',..,'_WK8' }
	@c as varchar(50) -- CMS_CONSTRAINEDXXXX, where XXXX = { '', '_WK1', '_WK2',..,'_WK8' }
AS
BEGIN
	SET NOCOUNT ON;
	declare @dbHistory as varchar(200) -- The db to be inserted/updated
	declare @dbSource as varchar(200) -- The db that has the data	
	declare @query as varchar(1000) -- The query to be constructed and run 8 times

	set @dbHistory = '[dbo].[MARS_CMS_FORECAST_HISTORY]'
	set @dbSource = '[dbo].[MARS_CMS_FORECAST]'
	
	set @query =  'Merge ' + @dbHistory + ' as target '
                + 'using (select rep_Date, Constrained, Unconstrained, country, cms_location_group_id, car_class_id from '
                + @dbSource + ' where rep_Date = ''' + @theDay + ''') as source '
                + 'On target.rep_Date = ''' + @theDay + ''' and target.Country = source.Country '
                + 'and target.cms_location_group_id = source.cms_location_group_id '
                + 'and target.car_class_id = source.car_class_id '
                + 'when matched then '
                + 'update set target.' + @c + ' = source.Constrained, target.' + @u + ' = source.Unconstrained, '
                + '     target.country = source.country, target.cms_location_group_id = source.cms_location_group_id, '
                + '     target.car_class_id = source.car_class_id '
                + 'when not matched then '
                + 'insert (rep_Date, ' + @c + ',' + @u + ', country, cms_location_group_id, car_class_id) '
                + 'values '
                + '(    source.rep_Date, source.Constrained, source.Unconstrained, source.Country, '
                + '     source.cms_location_group_id, source.car_class_id);'	
                
	exec (@query)
END