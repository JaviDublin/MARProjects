CREATE PROCEDURE [dbo].[ReservationImportImproved]
AS

BEGIN
	SET NOCOUNT ON;
	
	declare @RowsInStaging varchar(50) = (select count(*) from ReservationStaging2)
	declare @CommentsInStaging varchar(50) = (select count(*) from ResRmkStaging2)

	declare @RowsInActual varchar(50) = (select count(*) from Reservations)
	declare @CommentsInActual varchar(50) = (select count(*) from ResRemarks)
	

	insert into [Trace]
	select getdate(), 'ResImport', 'Res Staging Rows ' + @RowsInStaging +  ' Staging comments ' + @CommentsInStaging
						, 'Reservation Rows ' + @RowsInActual +  ' Comments ' + @CommentsInActual , '', '','', '',''

	;
	--Delete any duplicates of ResIdNumber
	WITH StagingDataWithDuplicates as
	(
	  SELECT ROW_NUMBER() OVER (PARTITION BY res_id_nbr
								ORDER BY  res_id_nbr DESC ) RowNum
	  FROM   ReservationStaging2
	)
	delete from StagingDataWithDuplicates where RowNum > 1
	
	--Update Date Sold field, this field has come in as '0001-01-01 00:00:00'
	UPDATE ReservationStaging2
	set [DATE_SOLD_DT] = null
	where isdate([DATE_SOLD_DT] ) <> 1
	
	
	DECLARE @LastUpdated DATETIME
	SET @LastUpdated =GETDATE()
	
	if exists(select 1 from ReservationStaging2)
	begin
	
	--Merge Staging to Reservations Table
	MERGE Reservations r
	USING (
			select rs.EFF_DTTM, rs.RES_ID_NBR, rs.COUNTRY
				, GoldUpgr.CarGroupId as CarGroupId
				, GoldUpgr.UpgradedCarGroupId
				, RentLocation.dim_Location_id as RentLocation, ReturnLocation.dim_Location_id as ReturnLocation
				, rs.ONEWAY, [RS_ARRIVAL_DATE], [RS_ARRIVAL_TIME]
				,[RTRN_DATE], [RTRN_TIME],[RES_DAYS] ,[RATE_QUOTED]
				,[SUBTOTAL_2], [MOP], [PREPAID], [NEVERLOST], [PREDELIVERY]
				,[CUST_NAME], [PHONE], [CDPID_NBR], [CNTID_NBR], [NO1_CLUB_GOLD]
				,[TACO], [FLIGHT_NBR], [GS], rs.[N1TYPE], DATE_SOLD_DT, DATE_SOLD_TM, GR_INCL_GOLDUPGR
				
			from ReservationStaging2 rs
			left join Locations RentLocation on rs.RENT_LOC = RentLocation.Location
			left join Locations ReturnLocation on rs.RTRN_LOC = ReturnLocation.Location
			left join N1GoldGroupUpgrade as 
			 GoldUpgr on left(rs.GR_INCL_GOLDUPGR,2) = GoldUpgr.CarGroup 
							and rs.COUNTRY = GoldUpgr.country
							and rs.[N1TYPE] = GoldUpgr.N1Type
			
		) StagingData
	ON
		(r.RES_ID_NBR = StagingData.RES_ID_NBR)
	WHEN NOT MATCHED by TARGET
		THEN INSERT
		(RES_ID_NBR
		,COUNTRY
		,RENT_LOC
		,RTRN_LOC
		,[ONEWAY]
		,[RS_ARRIVAL_DATE]
		,[RS_ARRIVAL_TIME]
		,[RTRN_DATE]
		,[RTRN_TIME]
		,[RES_DAYS]
		,[GR_INCL_GOLDUPGR]
		,[RATE_QUOTED]
		,[SUBTOTAL_2]
		,[MOP]
		,[PREPAID]
		,[NEVERLOST]
		,[PREDELIVERY]
		,[CUST_NAME]
		,[PHONE]
		,[CDPID_NBR]
		,[CNTID_NBR]
		,[NO1_CLUB_GOLD]
		,[TACO]
		,[FLIGHT_NBR]
		,[GS]
		,[N1TYPE]
		,[DATE_SOLD]
		,[CO_HOURS]
		,[CO_DAYS]
		,[CI_HOURS]
		,[CI_HOURS_OFFSET]
		,[CI_DAYS]
		,ArrivalDateTime
		,ReturnDateTime
		,LastUpdated
		,ReservedCarGroup
		,ReservedCarGroupId
		)
		VALUES
		(
			StagingData.RES_ID_NBR
			, StagingData.COUNTRY
			, RentLocation
			, ReturnLocation
			, case when RentLocation <> ReturnLocation then 'Y' else 'N' end
			, cast([RS_ARRIVAL_DATE] as datetime)
			, cast([RS_ARRIVAL_TIME] as datetime)
			, cast([RTRN_DATE] as DateTime)
			, cast([RTRN_TIME] as DateTime)
			, cast([RES_DAYS] as Float)
			, UpgradedCarGroupId
			, [RATE_QUOTED]
			, cast([SUBTOTAL_2] as Float)
			, [MOP]
			, case when [PREPAID]='P' then 'Y' else 'N' end
			--, case when [NEVERLOST]='NAVSYSREQ' then 'Y' else 'N' end
			, [NEVERLOST]
			, [PREDELIVERY]
			, [CUST_NAME]
			, [PHONE]
			, [CDPID_NBR]
			, [CNTID_NBR]
			, right([NO1_CLUB_GOLD],10)
			, [TACO]
			, [FLIGHT_NBR]
			, [GS]
			, [N1TYPE]
			, cast([DATE_SOLD_DT] as datetime) + cast([DATE_SOLD_TM] as datetime)
			, 0,0,0,0,0		--HoursBetween now and X are no longer stored in the database
			, cast([RS_ARRIVAL_DATE] as datetime) +  cast([RS_ARRIVAL_TIME] as datetime)
			, cast([RTRN_DATE] as datetime) + cast([RTRN_TIME] as datetime)
			,@LastUpdated
			,GR_INCL_GOLDUPGR
			, CarGroupId
		)
	WHEN MATCHED THEN UPDATE SET
		r.COUNTRY = StagingData.COUNTRY
		, r.RENT_LOC = RentLocation
		, r.RTRN_LOC = ReturnLocation
		, r.[ONEWAY]=case when RentLocation <> ReturnLocation then 'Y' else 'N' end
		, r.[RS_ARRIVAL_DATE] =  cast(StagingData.[RS_ARRIVAL_DATE] as Datetime)
		, r.[RS_ARRIVAL_TIME] = cast(StagingData.[RS_ARRIVAL_TIME] as DateTime)
		, r.[RTRN_DATE] = cast(StagingData.[RTRN_DATE] as datetime)
		, r.[RTRN_TIME]= cast(StagingData.[RTRN_TIME] as datetime)
		, r.[RES_DAYS]= cast(StagingData.[RES_DAYS] as float)
		, r.[GR_INCL_GOLDUPGR] = UpgradedCarGroupId
		, r.[RATE_QUOTED] = StagingData.[RATE_QUOTED]
		, r.[SUBTOTAL_2] =  cast(StagingData.[SUBTOTAL_2] as float)
		, r.[MOP] = StagingData.[MOP]
		, r.[PREPAID] = case when StagingData.[PREPAID]='P' then 'Y' else 'N' end
		--, r.[NEVERLOST] = case when StagingData.[NEVERLOST]='NAVSYSREQ' then 'Y' else 'N' end
		, r.[NEVERLOST] = StagingData.[NEVERLOST]
		, r.[PREDELIVERY] = StagingData.[PREDELIVERY]
		, r.[CUST_NAME] = StagingData.[CUST_NAME]
		, r.[PHONE] = StagingData.[PHONE]
		, r.[CDPID_NBR] = StagingData.[CDPID_NBR]
		, r.[CNTID_NBR] = StagingData.[CNTID_NBR]
		, r.[NO1_CLUB_GOLD] = right(StagingData.[NO1_CLUB_GOLD],10)
		, r.[TACO] = StagingData.[TACO]
		, r.[FLIGHT_NBR] = StagingData.[FLIGHT_NBR]
		, r.[GS] = StagingData.[GS]
		, r.[N1TYPE] = StagingData.[N1TYPE]
		, r.[DATE_SOLD] = cast(StagingData.[DATE_SOLD_DT] as Datetime) + cast( StagingData.[DATE_SOLD_TM] as datetime)
		, r.ArrivalDateTime = cast(StagingData.[RS_ARRIVAL_DATE] as Datetime) + cast(StagingData.[RS_ARRIVAL_TIME] as DateTime)
		, r.ReturnDateTime = cast(StagingData.[RTRN_DATE] as datetime) + cast(StagingData.[RTRN_TIME] as datetime)
		, r.LastUpdated=@LastUpdated
		, r.ReservedCarGroup = StagingData.GR_INCL_GOLDUPGR
		, r.ReservedCarGroupId = StagingData.CarGroupId
;

		declare @maxDttmRecieved datetime 
		set @maxDttmRecieved = (select top 1 MAX(cast([DATE_SOLD_DT] as Datetime)
		 + cast( [DATE_SOLD_TM] as datetime)) from  ReservationStaging2)
		
		declare @rowsRecieved int
		set @rowsRecieved = (select COUNT(*) from ReservationStaging2)
		
		insert into dbo.ResControl
		select 2, cast(@rowsRecieved as varchar) + ' new Rows Recieved', GETDATE() , null, @maxDttmRecieved

	END
	--Remarks
	
	
	
	if exists(select 1 from ResRmkStaging2)
	begin

		declare @RemarksNotInMainResTable varchar(50) = (select count(*) from ResRmkStaging2
					where ResIdNbr not in (select distinct RES_ID_NBR from Reservations))
	

		insert into [Trace]
		select getdate(), 'ResImport', 'Remark Rows not in Reservations ' + @RemarksNotInMainResTable, '', '', '','', '',''

		delete 
		from ResRmkStaging2
		where ResIdNbr not in (select distinct RES_ID_NBR from Reservations)
	
	
		MERGE ResRemarks rr
		USING (
			select ResIdNbr, SeqNbr, ResRmkType, Remark
			from dbo.ResRmkStaging2
		) StagingRemarks
		ON
			(rr.ResIdNbr = StagingRemarks.ResIdNbr
				and rr.SeqNbr = cast(StagingRemarks.SeqNbr as int))
		WHEN NOT MATCHED by TARGET
			THEN INSERT 
			(
				SeqNbr, ResRmkType, Remark, ResIdNbr
			)
			VALUES
			(
				cast(StagingRemarks.SeqNbr as int)
				, StagingRemarks.ResRmkType
				, StagingRemarks.Remark
				, StagingRemarks.ResIdNbr
			)
		WHEN MATCHED THEN UPDATE SET
			rr.ResRmkType = StagingRemarks.ResRmkType
			, rr.Remark = StagingRemarks.Remark
		
		--WHEN NOT MATCHED by SOURCE
		--then delete
		;
		
		
	end
	



	--Delete reservations past checkout date



	

	--declare @RemarksAttachedToCancelledReservations varchar(50) = (select count(*) from ResRemarks
	--where ResIdNbr in
	--(select RES_ID_NBR from ReservationStaging2 
	--	where lower(RESV_BOOK_ACTN_CD) in('x', 'r') 
	--		or lower(RESV_BOOK_CUR_ACTN_CD) in('x', 'r') ))
	

	--	insert into [Trace]
	--	select getdate(), 'ResImport', 'Remark Rows attached to Cancelled Reservations ' + @RemarksAttachedToCancelledReservations, '', '', '','', '',''


	delete
	from ResRemarks
	where ResIdNbr in
	(select RES_ID_NBR from ReservationStaging2 
		where lower(RESV_BOOK_CUR_ACTN_CD) in('x', 'r','u','l'
			) 
			or DeleteIndicator = 'H'
			)

	delete 
	from reservations
	where RES_ID_NBR in
	(select RES_ID_NBR from ReservationStaging2 
		where   lower(RESV_BOOK_CUR_ACTN_CD) in('x', 'r','u','l') 
		or DeleteIndicator = 'H'		
		)





	--Add location specific Offset hours
	update r
	set CI_HOURS_OFFSET = l.turnaround_hours
	from reservations r
	join LOCATIONS l on r.RTRN_LOC = l.dim_Location_id
		
		
	--Mark all logs as processed
	Update ReservationTeradataControlLog
	set Processed = 1
	where Processed = 0
	

END