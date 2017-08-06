-- =============================================
-- Author:		Javier
-- Create date: June 2012
-- Description:	
-- =============================================
CREATE PROCEDURE [Import].[Update_CMS_Forecast_History]
	
	--@theDay DATETIME
	
AS
BEGIN
	
	SET NOCOUNT ON;
	/*
	declare @theDay DATETIME = DATEADD(dd, DATEDIFF(dd, 0, GETDATE() - 1), 0) 

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

    */

	DECLARE @t1 DATETIME;
		DECLARE @t2 DATETIME;

		
		/*
		declare @startDate as datetime = '2014-10-15'

			select distinct f.REP_DATE --, f.COUNTRY, f.CAR_CLASS_ID, f.CMS_LOCATION_GROUP_ID, CONSTRAINED, UNCONSTRAINED
			from  MarsPortalLiveRestore.dbo.[MARS_CMS_FORECAST] f 
			where datepart(dw, f.REP_DATE) = datepart(dw, @startDate)
				and f.REP_DATE <= dateadd(week, 8, @startDate)
				and f.rep_date >= @startDate
				
		*/
		
		declare @startDate as date = cast(GETDATE() as date);	
		SET @t1 = GETDATE();	

		merge into [MARS_CMS_FORECAST_HISTORY] as target
		--merge into [MARS_CMS_FORECAST_HISTORY_Small2] as target
		using 
		(
			select  f.REP_DATE, f.COUNTRY, f.CAR_CLASS_ID, f.CMS_LOCATION_GROUP_ID, CONSTRAINED, UNCONSTRAINED
			from  [MARS_CMS_FORECAST] f 
			where datepart(dw, f.REP_DATE) = datepart(dw, @startDate)
				and f.REP_DATE <= dateadd(week, 8, @startDate)
				and f.rep_date >= @startDate
		) 
		as source 
		On target.rep_Date = source.rep_date
			and target.Country = source.Country 
			and target.cms_location_group_id = source.cms_location_group_id 
			and target.car_class_id = source.car_class_id 
		when matched 
			then update set target.cms_Constrained = case when source.rep_date = @startDate then source.constrained else target.cms_constrained end
							, target.cms_Unconstrained = case when source.rep_date = @startDate then source.unconstrained else target.cms_unconstrained end
							, target.CMS_CONSTRAINED_WK1 =	case when source.rep_date = dateadd(week, 1, @startDate) 
													then source.constrained else target.CMS_CONSTRAINED_WK1 end
							, target.CMS_UNCONSTRAINED_WK1 = case when source.rep_date = dateadd(week, 1, @startDate) 
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK1 end
							, target.CMS_CONSTRAINED_WK2 = case when source.rep_date = dateadd(week, 2, @startDate)  
													then source.constrained else target.CMS_CONSTRAINED_WK2 end
							, target.CMS_UNCONSTRAINED_WK2 = case when source.rep_date = dateadd(week, 2, @startDate)  
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK2 end
							, target.CMS_CONSTRAINED_WK3 = case when source.rep_date = dateadd(week, 3, @startDate)  
													then source.constrained else target.CMS_CONSTRAINED_WK3 end
							, target.CMS_UNCONSTRAINED_WK3 = case when source.rep_date = dateadd(week, 3, @startDate)  
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK3 end
							, target.CMS_CONSTRAINED_WK4 = case when source.rep_date = dateadd(week, 4, @startDate)  
													then source.constrained else target.CMS_CONSTRAINED_WK4 end
							, target.CMS_UNCONSTRAINED_WK4 = case when source.rep_date = dateadd(week, 4, @startDate)  
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK4 end
							, target.CMS_CONSTRAINED_WK5 = case when source.rep_date = dateadd(week, 5, @startDate)  
													then source.constrained else target.CMS_CONSTRAINED_WK5 end
							, target.CMS_UNCONSTRAINED_WK5 = case when source.rep_date = dateadd(week, 5, @startDate)  
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK5 end
							, target.CMS_CONSTRAINED_WK6 = case when source.rep_date = dateadd(week, 6, @startDate)  
													then source.constrained else target.CMS_CONSTRAINED_WK6 end
							, target.CMS_UNCONSTRAINED_WK6 = case when source.rep_date = dateadd(week, 6, @startDate)  
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK6 end
							, target.CMS_CONSTRAINED_WK7 = case when source.rep_date = dateadd(week, 7, @startDate)  
													then source.constrained else target.CMS_CONSTRAINED_WK7 end
							, target.CMS_UNCONSTRAINED_WK7 = case when source.rep_date = dateadd(week, 7, @startDate)  
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK7 end
							, target.CMS_CONSTRAINED_WK8 = case when source.rep_date = dateadd(week, 8, @startDate)  
													then source.constrained else target.CMS_CONSTRAINED_WK8 end
							, target.CMS_UNCONSTRAINED_WK8 = case when source.rep_date = dateadd(week, 8, @startDate)  
													then source.unconstrained else target.CMS_UNCONSTRAINED_WK8 end
		when not matched 
			then insert (
				rep_Date
				, country
				, cms_location_group_id
				, car_class_id
				, cms_constrained, cms_unconstrained
				,  [CMS_CONSTRAINED_WK1], [CMS_UNCONSTRAINED_WK1]
				,  [CMS_CONSTRAINED_WK2], [CMS_UNCONSTRAINED_WK2]
				,  [CMS_CONSTRAINED_WK3], [CMS_UNCONSTRAINED_WK3]
				,  [CMS_CONSTRAINED_WK4], [CMS_UNCONSTRAINED_WK4]
				,  [CMS_CONSTRAINED_WK5], [CMS_UNCONSTRAINED_WK5]
				,  [CMS_CONSTRAINED_WK6], [CMS_UNCONSTRAINED_WK6]
				,  [CMS_CONSTRAINED_WK7], [CMS_UNCONSTRAINED_WK7]
				,  [CMS_CONSTRAINED_WK8], [CMS_UNCONSTRAINED_WK8]
				 ) 
			values 
			(    
				source.rep_date
				, source.Country
				, source.cms_location_group_id
				, source.car_class_id
				, case when source.rep_date = @startDate then source.constrained else null end
				, case when source.rep_date = @startDate then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 1, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 1, @startDate) then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 2, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 2, @startDate) then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 3, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 3, @startDate) then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 4, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 4, @startDate) then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 5, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 5, @startDate) then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 6, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 6, @startDate) then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 7, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 7, @startDate) then source.unconstrained else null end
				, case when source.rep_date = dateadd(week, 8, @startDate) then source.constrained else null end
				, case when source.rep_date = dateadd(week, 8, @startDate) then source.unconstrained else null end
			)
		--OUTPUT $ACTION ChangeType, updated.*, deleted.*
		;

		SET @t2 = GETDATE();
		SELECT DATEDIFF(second,@t1,@t2) AS elapsed_seconds;

END