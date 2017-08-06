CREATE PROCEDURE [bcs].[s_Populate_ProcessControl_MARS]
	
AS
BEGIN

	SET NOCOUNT ON;
	
-- Mars Daily file 
--======================================================================================

declare @Batch_id int = 1
delete bcs.ProcessBatch  where ProcessBatch_id = @Batch_id
insert into bcs.ProcessBatch values (@Batch_id,'Mars_Daily')

declare @seq int = 0
delete  bcs.ProcessControl where ProcessBatch_id = @Batch_id
select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Upload File' ,  'SSIS' , 'Fleet Status Europe' ,
'MARS_SSIS_Carrent.dtsx',
'<File_Name>^Data2^^Data1^','DIRDUB01\RAD_DBUSER','00:01:00',5)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Review File' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Review_Fleet_OKC]',
null,null,null,null)


select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Stats into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_Averages]',
null,null,null,null)


select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History Fact' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History_Fact]',
null,null,null,null)



select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Query Table' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Query]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert OKCFILE into Query Table' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Query]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Actual Fleet' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Actual]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Query Table into Actual Fleet' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Actual]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Model Codes' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_ModelCodes]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Model Codes' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_ModelCodes]',
null,null,null,null)


select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Actual Fleet into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Stats' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Stats]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into Stats' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Stats]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Stats into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_Averages]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History Fact' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History_Fact]',
null,null,null,null)

--select @seq = @seq + 1
--insert bcs.ProcessControl values 
--(@Batch_id , @seq , 'Update Data tables' ,  'StoredProc' , 'HESCMARS01' ,
--'[Import].Update_Import_Data_Tables @importTypeIsDaily = 1 , @comment =  ''File Upload''',
--null,null,null,null)


-- Mars Hourly file 
--======================================================================================


set @Batch_id = 2
delete bcs.ProcessBatch  where ProcessBatch_id = @Batch_id
insert into bcs.ProcessBatch values (@Batch_id,'Mars_Hourly')

set @seq = 0
delete  bcs.ProcessControl where ProcessBatch_id = @Batch_id
select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Upload File' ,  'SSIS' , 'Fleet Status Europe' ,
'MARS_SSIS_Carrent.dtsx',
'<File_Name>^Data2^^Data1^','DIRDUB01\RAD_DBUSER','00:01:00',5)


select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Review File' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Review_Fleet_OKC]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Update Query Table' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Update_Fleet_Query]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Update Actual Fleet' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Update_Fleet_Actual]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary]',
null,null,null,null)


select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Actual Fleet into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary]',
null,null,null,null)


select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into Stats' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Stats]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Stats into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_Averages_Hour]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History Fact' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History_Fact]',
null,null,null,null)

--select @seq = @seq + 1
--insert bcs.ProcessControl values 
--(@Batch_id , @seq , 'Update Data tables' ,  'StoredProc' , 'HESCMARS01' ,
--'[Import].Update_Import_Data_Tables @importTypeIsDaily = 0 , @comment =  ''File Upload''',
--null,null)



-- Mars CMS Forecast
--======================================================================================
set @Batch_id = 3
delete bcs.ProcessBatch  where ProcessBatch_id = @Batch_id
insert into bcs.ProcessBatch values (@Batch_id,'CMS_Forecast')

set @seq = 0
delete  bcs.ProcessControl where ProcessBatch_id = @Batch_id
select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Unzip file' ,  'SSIS' , 'MARS CMS Forecast' ,
'MARS_Unzip.dtsx',
'<zip_SourceFile>^Data2^^Data3^^Data1^','DIRDUB01\RAD_DBUSER',null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Upload File' ,  'SSIS' , 'MARS CMS Forecast' ,
'MARS_SSIS_CMS.dtsx',
'<File_Name>^Data2^^Data4^','DIRDUB01\RAD_DBUSER',null,null)




select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Review File CMS Forecast' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Review_Forecast_CMS]',
null,null,null,null)


select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Review File CMS Forecast' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Execute_CMS_Procedures]',
null,null,null,null)



-- BPServer_Fleet
--======================================================================================
set @Batch_id = 4
delete bcs.ProcessBatch  where ProcessBatch_id = @Batch_id
insert into bcs.ProcessBatch values (@Batch_id,'BPServer_Fleet')


set @seq = 0
delete  bcs.ProcessControl where ProcessBatch_id = @Batch_id
select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Export File' ,  'StoredProc' , 'HESCMARS01' ,
'[Export].[GenerateFiles_cmd] @fileSystem = ''BPserver''',
NULL,NULL,null,null)

-- Mars_All
--======================================================================================
select @Batch_id = 5
delete bcs.ProcessBatch  where ProcessBatch_id = @Batch_id
insert into bcs.ProcessBatch values (@Batch_id,'Mars_All')

select @seq = 0
delete  bcs.ProcessControl where ProcessBatch_id = @Batch_id
select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Upload File' ,  'SSIS' , 'Fleet Status Europe' ,
'MARS_SSIS_Carrent.dtsx',
'<File_Name>^Data2^^Data1^','DIRDUB01\RAD_DBUSER','00:01:00',5)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Review File' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Review_Fleet_OKC]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary_Daily]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Stats into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_Averages]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History_Daily]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History Fact' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History_Fact_Daily]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Query Table' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Query]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert OKCFILE into Query Table' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Query]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Actual Fleet' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Actual]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Query Table into Actual Fleet' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Actual]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Model Codes' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_ModelCodes]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Model Codes' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_ModelCodes]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Update Query Table' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Update_Fleet_Query]', '@type = ''^Data3^''',		-- hourly
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Update Actual Fleet' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Update_Fleet_Actual]', '@type = ''^Data3^''',		-- hourly
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Actual Fleet into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Stats' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Stats]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into Stats' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Stats]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Truncate Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Truncate_Fleet_Summary]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Stats into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_Averages]', '@type = ''^Data3^''',		-- daily
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Stats into Summary' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_Averages_Hour]', '@type = ''^Data3^''',		-- hourly
null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Insert Summary into History Fact' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Insert_Fleet_Summary_History_Fact]',
null,null,null,null)

select @seq = @seq + 1
insert bcs.ProcessControl values 
(@Batch_id , @seq , 'Update last imported date' ,  'StoredProc' , 'HESCMARS01' ,
'[MARSPortal].[Import].[Update_Import_Data_Tables]', '@type = ''^Data3^'',@comment =  ''File Upload''',
null,null,null)

END