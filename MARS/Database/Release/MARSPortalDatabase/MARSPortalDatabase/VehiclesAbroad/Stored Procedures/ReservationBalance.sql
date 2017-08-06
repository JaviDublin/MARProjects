CREATE PROCEDURE [VehiclesAbroad].[ReservationBalance]
	@startDate DateTime, @endDate Datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT COWN.country AS OWNING_COUNTRY,CIN.country AS IN_COUNTRY, ISNULL(COUNT(FEA.SERIAL),0) AS RESULT
	into #availableTable
	FROM dbo.FLEET_EUROPE_ACTUAL FEA
		INNER JOIN dbo.COUNTRIES COWN ON FEA.COUNTRY = COWN.country
		inner JOIN dbo.COUNTRIES CIN ON LEFT(FEA.LSTWWD, 2) = CIN.country
	WHERE (FEA.FLEET_RAC_TTL = 1 OR FEA.FLEET_CARSALES = 1)
		AND(COWN.active = 1)
		AND NOT(COWN.country = CIN.country)
	GROUP BY COWN.country, CIN.country
		
	SELECT COWN.country AS OWNING_COUNTRY,CDUE.country AS IN_COUNTRY, ISNULL(COUNT(FEA.SERIAL),0) RESULT
	into #ciReturns
	FROM dbo.FLEET_EUROPE_ACTUAL FEA
		INNER JOIN dbo.COUNTRIES COWN ON FEA.COUNTRY = COWN.country
		INNER JOIN dbo.COUNTRIES CIN ON LEFT(FEA.LSTWWD, 2) = CIN.country
		INNER JOIN dbo.COUNTRIES CDUE ON CASE WHEN LEFT(FEA.DUEWWD, 2) IS NULL THEN LEFT(FEA.LSTWWD, 2) ELSE LEFT(FEA.DUEWWD, 2) END = CDUE.country
	WHERE (FEA.FLEET_RAC_TTL = 1 OR FEA.FLEET_CARSALES = 1)
		AND(COWN.active = 1)
		AND(COWN.country = CIN.country AND NOT(COWN.country = CDUE.country) AND FEA.ON_RENT = 1)
		And (fea.DUEDATE >= @startDate and fea.DUEDATE <=@endDate)
	GROUP BY COWN.country,CDUE.country
	
	SELECT COUNTRY owning, left(RTRN_LOC,2) destination,ISNULL(COUNT(rea.RES_ID_NBR),0) result
	into #resfromto
	  FROM [dbo].[RESERVATIONS_EUROPE_ACTUAL] rea
	  where COUNTRY != LEFT(rea.RTRN_LOC, 2)
	  and (rea.RS_ARRIVAL_DATE>=@startDate and rea.RS_ARRIVAL_DATE<=@endDate)
	  group by rea.COUNTRY, left(rea.RTRN_LOC,2)
	
	select isnull(avt.OWNING_COUNTRY, cir.OWNING_COUNTRY) owning, ISNULL(avt.IN_COUNTRY, cir.IN_COUNTRY) destination,isnull(avt.RESULT,0) available, isnull(cir.RESULT,0) orOwn2Due
	into #addTable
	from #availableTable avt
	full outer join #ciReturns cir on avt.IN_COUNTRY=cir.IN_COUNTRY and avt.OWNING_COUNTRY=cir.OWNING_COUNTRY
	
	select isnull(at.owning, rft.owning) owning, ISNULL(at.destination, rft.destination) destination, isnull(at.available,0) available,ISNULL(at.orOwn2Due,2)orOwn2Due, isnull(rft.result,0) res
	into #addTable2
	from #addTable at
	full outer join #resfromto rft on at.destination=rft.destination and at.owning=rft.owning
	order by owning
	
	select at2.owning, at2.destination, at2.orOwn2Due+at2.available+at2.res addition
	into #addTable3
	from #addTable2 at2
	
	select at3.owning ownBack, at3.destination dueBack, at3.orOwn2Due+at3.res subtraction
	into #minusTable
	from #addTable2 at3
	
	select ISNULL(c1.country_description, c3.country_description) owning, ISNULL(c2.country_description, c4.country_description) destination, at4.addition - mt.subtraction result
	from #addTable3 at4
	join #minusTable mt on at4.owning=mt.dueBack and at4.destination=mt.ownBack
	join dbo.COUNTRIES c1 on at4.owning = c1.country
	join dbo.COUNTRIES c2 on at4.destination = c2.country
	join dbo.COUNTRIES c3 on mt.dueBack = c3.country
	join dbo.COUNTRIES c4 on mt.ownBack = c4.country
	order by destination, owning

drop table #availableTable
drop table #ciReturns
drop table #resfromto
drop table #addTable
drop table #addTable2
drop table #addTable3
drop table #minusTable
END