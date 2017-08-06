-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Update_CMS_Forecast_History]
	
	@theDay DATETIME
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @count int, @current_count int, @current_date datetime
	declare @c varchar(50), @u varchar(50)
	
	SET @count = 8			-- 8 weeks
	SET @current_count = 0
	SET @current_date = @theDay	
	
	WHILE(@current_count <= @count)
	BEGIN
		if @current_count = 0 
			begin
				set @c = 'cms_constrained'
				set @u = 'cms_unconstrained'
			end
		else
			begin
				set @c = 'cms_constrained_wk' + cast (@current_count as varchar)
				set @u = 'cms_unconstrained_wk' + cast (@current_count as varchar)
			end				
	
	
	declare @dbHistory as varchar(200) -- The db to be inserted/updated
	declare @dbSource as varchar(200) -- The db that has the data	
	declare @query as varchar(1000) -- The query to be constructed and run 8 times

	set @dbHistory	= '[dbo].[MARS_CMS_FORECAST_HISTORY]'
	set @dbSource	= '[dbo].[MARS_CMS_FORECAST]'
	
	SET @query =  'Merge ' + @dbHistory + ' as target '
                + 'using (select rep_Date, Constrained, Unconstrained, country, cms_location_group_id, car_class_id from '
                + @dbSource + ' where rep_Date = ''' + 
                convert(varchar,@current_date) 
                + ''') as source '
                + 'On target.rep_Date = ''' + 
                convert(varchar,@current_date )+ ''' and target.Country = source.Country '
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
		
		
		
		SET @current_count = @current_count + 1	
		SET @current_date = DATEADD(d, 7, @current_date)
	END

    
END