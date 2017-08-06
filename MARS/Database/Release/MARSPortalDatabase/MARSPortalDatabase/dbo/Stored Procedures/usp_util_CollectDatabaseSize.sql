CREATE PROC usp_util_CollectDatabaseSize
AS
BEGIN
SET NOCOUNT ON
INSERT INTO DatabaseFileSize
(
[rowId],
[database_id],
[file_id],
[file_type_desc],
[name],
[physical_name],
[state_desc],
[size],
[max_size],
[growth],
[is_sparse],
[is_percent_growth],
[collect_date]
)
SELECT
NEWID(),
[database_id],
[file_id],
[type_desc],
[name],
[physical_name],
[state_desc],
([size]*8)/1024,
[max_size],
[growth],
[is_sparse],
[is_percent_growth],
GETDATE()
FROM sys.master_files
where [physical_name] like '%MarsPortal%'
SET NOCOUNT OFF
END