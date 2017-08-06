
CREATE PROCEDURE [VehiclesAbroad].[ReservationBalance] @startDate Datetime,
                                                      @endDate   Datetime
AS
  BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

      SELECT cown.country                 AS owning_country,
             cin.country                  AS in_country,
             ISNULL(COUNT(fea.SERIAL), 0) AS result
      INTO   #availableTable
      FROM   dbo.Fleet_europe_actual fea
             INNER JOIN dbo.Countries cown
                     ON fea.COUNTRY = cown.country
             INNER JOIN dbo.Countries cin
                     ON LEFT(fea.LSTWWD, 2) = cin.country
      WHERE  ( fea.FLEET_RAC_TTL = 1
                OR fea.FLEET_CARSALES = 1 )
             AND ( cown.active = 1 )
             AND NOT( cown.country = cin.country )
      GROUP  BY cown.country,
                cin.country



      SELECT cown.country                 AS owning_country,
             cdue.country                 AS in_country,
             ISNULL(COUNT(fea.SERIAL), 0) result
      INTO   #ciReturns
      FROM   dbo.Fleet_europe_actual fea
             INNER JOIN dbo.Countries cown
                     ON fea.COUNTRY = cown.country
             INNER JOIN dbo.Countries cin
                     ON LEFT(fea.LSTWWD, 2) = cin.country
             INNER JOIN dbo.Countries cdue
                     ON CASE
                          WHEN LEFT(fea.DUEWWD, 2) IS NULL THEN LEFT(fea.LSTWWD, 2)
                          ELSE LEFT(fea.DUEWWD, 2)
                        END = cdue.country
      WHERE  ( fea.FLEET_RAC_TTL = 1
                OR fea.FLEET_CARSALES = 1 )
             AND ( cown.active = 1 )
             AND ( cown.country = cin.country
                   AND NOT( cown.country = cdue.country )
                   AND fea.ON_RENT = 1 )
             AND ( fea.DUEDATE >= @startDate
                   AND fea.DUEDATE <= @endDate )
      GROUP  BY cown.country,
                cdue.country

      -- removed by Damien 25.06.2014
      -- SELECT COUNTRY                          owning,
      --        LEFT(RTRN_LOC, 2)                destination,
      --        ISNULL(COUNT(rea.RES_ID_NBR), 0) result
      ---- INTO   #resfromto
      -- FROM   [dbo].[Reservations_europe_actual] rea
      -- WHERE  COUNTRY != LEFT(rea.RTRN_LOC, 2)
      --        AND ( rea.RS_ARRIVAL_DATE >= @startDate
      --              AND rea.RS_ARRIVAL_DATE <= @endDate )
      -- GROUP  BY rea.COUNTRY,
      --           LEFT(rea.RTRN_LOC, 2)

      SELECT l.country                      owning,
             LEFT(l.served_by_locn, 2)      destination,
             ISNULL(COUNT(r.RES_ID_NBR), 0) result
             INTO   #resfromto
      FROM   Reservations r
             INNER JOIN Locations l
                     ON r.RTRN_LOC = l.dim_Location_id
      WHERE  l.country != LEFT(l.served_by_locn, 2)
             AND ( r.RS_ARRIVAL_DATE >= @startDate
                   AND r.RS_ARRIVAL_DATE <= @endDate )
      GROUP  BY l.country,
                LEFT(l.served_by_locn, 2)





      SELECT isnull(avt.OWNING_COUNTRY, cir.OWNING_COUNTRY) owning,
             ISNULL(avt.IN_COUNTRY, cir.IN_COUNTRY)         destination,
             isnull(avt.RESULT, 0)                          available,
             isnull(cir.RESULT, 0)                          orown2due
      INTO   #addTable
      FROM   #availableTable avt
             FULL OUTER JOIN #ciReturns cir
                          ON avt.IN_COUNTRY = cir.IN_COUNTRY
                             AND avt.OWNING_COUNTRY = cir.OWNING_COUNTRY

      SELECT isnull(at.owning, rft.owning)           owning,
             ISNULL(at.destination, rft.destination) destination,
             isnull(at.available, 0)                 available,
             ISNULL(at.orOwn2Due, 2)                 orown2due,
             isnull(rft.result, 0)                   res
      INTO   #addTable2
      FROM   #addTable at
             FULL OUTER JOIN #resfromto rft
                          ON at.destination = rft.destination
                             AND at.owning = rft.owning
      ORDER  BY owning

      SELECT at2.owning,
             at2.destination,
             at2.orOwn2Due + at2.available + at2.res addition
      INTO   #addTable3
      FROM   #addTable2 at2

      SELECT at3.owning              ownback,
             at3.destination         dueback,
             at3.orOwn2Due + at3.res subtraction
      INTO   #minusTable
      FROM   #addTable2 at3

      SELECT ISNULL(c1.country_description, c3.country_description) owning,
             ISNULL(c2.country_description, c4.country_description) destination,
             at4.addition - mt.subtraction                          result
      FROM   #addTable3 at4
             JOIN #minusTable mt
               ON at4.owning = mt.dueBack
                  AND at4.destination = mt.ownBack
             JOIN dbo.Countries c1
               ON at4.owning = c1.country
             JOIN dbo.Countries c2
               ON at4.destination = c2.country
             JOIN dbo.Countries c3
               ON mt.dueBack = c3.country
             JOIN dbo.Countries c4
               ON mt.ownBack = c4.country
      ORDER  BY destination,
                owning

      DROP TABLE #availableTable

      DROP TABLE #ciReturns

      DROP TABLE #resfromto

      DROP TABLE #addTable

      DROP TABLE #addTable2

      DROP TABLE #addTable3

      DROP TABLE #minusTable
  END