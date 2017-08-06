-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[fn_Staging_DayGroup]
(	
	
	@dayGroupName varchar(20)
)
RETURNS TABLE 
AS
RETURN 
(
	select 
		DATEADD(D,0,DATEDIFF(d,0,f.LastDailyFileDate)) as [LastDailyFileDate],
		f.Country , f.Lstwwd , f.CarGroup , f.OperStat , d.DayGroupName , COUNT(*) as [total]
	from [General].Staging_Fleet  f
	inner join 
		[General].Fact_NonRevLog as l on f.NonRevLogId = l.NonRevLogId
	inner join [Settings].NonRev_Day_Groups as d on l.DayGroupCode = d.DayGroupCode
	where d.DayGroupName = @dayGroupName
	group by 
		f.LastDailyFileDate , f.Country , f.Lstwwd , f.CarGroup , f.OperStat , d.DayGroupName
)