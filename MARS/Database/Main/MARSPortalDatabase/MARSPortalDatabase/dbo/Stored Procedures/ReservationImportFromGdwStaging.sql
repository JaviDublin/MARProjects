
CREATE PROCEDURE [dbo].[ReservationImportFromGdwStaging]
AS

BEGIN
	SET NOCOUNT ON;


	exec [dbo].[AddUnmappedCarSegmentsClassesAndGroupsFromReservations]

	exec [dbo].[AddUnmappedLocationsFromReservations]
	

	--Update Date Sold field, this field has come in as '0001-01-01 00:00:00'
	UPDATE ReservationStaging2
	set [DATE_SOLD_DT] = null
	where isdate([DATE_SOLD_DT] ) <> 1

	
	if exists(select 1 from ReservationStaging2)
	begin
	
	--Merge Staging to Reservations Table
	MERGE Reservation r
	USING (
			select rs.RES_ID_NBR as ExternalId
				, rs.COUNTRY as Country
				, RentLocation.dim_Location_id as PickupLocationId
				, ReturnLocation.dim_Location_id as ReturnLocationId
				, cast([RS_ARRIVAL_DATE] as datetime) +  cast([RS_ARRIVAL_TIME] as datetime) as PickupDate
				, cast(RTRN_DATE as datetime) +  cast(RTRN_TIME as datetime) as ReturnDate
				, cast([DATE_SOLD_DT] as datetime) + cast([DATE_SOLD_TM] as datetime) as BookedDate
				, GoldUpgr.UpgradedCarGroupId as UpgradedCarGroupId
				, GoldUpgr.CarGroupId as ReservedCarGroupId
				, isnull(RATE_QUOTED, '') as Tariff
				, rs.N1TYPE as N1Type
				, case when NeverLost = 'N' or NeverLost is null then 0 else 1 end as NeverLost
				, [CUST_NAME] as CustomerName
				, [FLIGHT_NBR] as FlightNumber
				, '' as Remark
				, getdate() as DateAdded
			from ReservationStaging2 rs
			left join Locations RentLocation on rs.RENT_LOC = RentLocation.Location
			left join Locations ReturnLocation on rs.RTRN_LOC = ReturnLocation.Location
			left join N1GoldGroupUpgrade as 
			 GoldUpgr on left(rs.GR_INCL_GOLDUPGR,2) = GoldUpgr.CarGroup 
							and rs.COUNTRY = GoldUpgr.country
							and isnull(rs.N1TYPE, 'NA') = GoldUpgr.N1Type
			where RentLocation.dim_Location_id is not null
				and ReturnLocation.dim_Location_id is not null
				and rs.GR_INCL_GOLDUPGR is not null
			
		) StagingData
	ON
		(r.ExternalId = StagingData.ExternalId)
	WHEN NOT MATCHED by TARGET
		THEN INSERT
		(
			ExternalId, Country, PickupLocationId, ReturnLocationId, PickupDate, ReturnDate
			, BookedDate, UpgradedCarGroupId, ReservedCarGroupId, Tariff, N1Type, NeverLost
			, CustomerName, FlightNumber, Remark, DateAdded, Comment
		)
		VALUES
		(
			StagingData.ExternalId
			, StagingData.Country
			, StagingData.PickupLocationId
			, StagingData.ReturnLocationId, PickupDate, ReturnDate, BookedDate
			, UpgradedCarGroupId, ReservedCarGroupId, Tariff, N1Type
			, NeverLost
			, CustomerName, FlightNumber, Remark, DateAdded, ''
		)
	WHEN MATCHED THEN UPDATE SET
		r.COUNTRY = StagingData.COUNTRY
		, r.PickupLocationId = StagingData.PickupLocationId
		, r.ReturnLocationId = StagingData.ReturnLocationId
		, r.PickupDate = StagingData.PickupDate
		, r.ReturnDate = StagingData.ReturnDate
		, r.BookedDate = StagingData.BookedDate
		, r.UpgradedCarGroupId = StagingData.UpgradedCarGroupId
		, r.ReservedCarGroupId = StagingData.ReservedCarGroupId
		, r.Tariff = StagingData.Tariff
		, r.N1Type = StagingData.N1Type
		, r.NeverLost = StagingData.NeverLost
		, r.CustomerName = StagingData.CustomerName
		, r.FlightNumber = StagingData.FlightNumber
		, r.Remark = ''
		, r.DateAdded = getdate()
		;
	END


	delete 
	from reservation
	where ExternalId in
	(select RES_ID_NBR from ReservationStaging2 
		where   lower(RESV_BOOK_CUR_ACTN_CD) in('x', 'r','u','l') 
		or DeleteIndicator = 'H'
		)


END