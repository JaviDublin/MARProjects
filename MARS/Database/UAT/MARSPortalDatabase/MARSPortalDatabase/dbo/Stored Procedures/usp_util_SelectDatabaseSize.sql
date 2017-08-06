CREATE PROC [dbo].[usp_util_SelectDatabaseSize]
AS
BEGIN
SET NOCOUNT ON
select 'MarsPortal' , SUM(size)  as 'MB',SUM(size)/1024 as 'GB', collect_date
from DatabaseFileSize
group by collect_date
order by collect_date
SET NOCOUNT OFF
END