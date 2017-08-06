


CREATE view [dbo].[vwUserLogonHistoryDetails]
as

SELECT distinct TOP (100) PERCENT  
dbo.UserLogonHistory.UserId                                   AS UserID,
                         dbo.UserLogonHistory.UserName,
                         --dbo.UserLogonHistory.TimeStamp                                AS Date,
                         Datename(dw, Cast(dbo.UserLogonHistory.TimeStamp AS DATE))    AS DayOfWeek,
                         CONVERT(VARCHAR, dbo.UserLogonHistory.TimeStamp, 101)         AS ShortDate,
                         --CONVERT(VARCHAR, dbo.UserLogonHistory.TimeStamp, 108)         AS ShortTime,
                         Datename(month, Cast(dbo.UserLogonHistory.TimeStamp AS DATE)) AS Month,
                         Datename(year, Cast(dbo.UserLogonHistory.TimeStamp AS DATE))  AS Year,
                         Datename(ISO_WEEK, Cast(dbo.UserLogonHistory.TimeStamp AS DATE))  AS WeekNo,
                         CASE
                           when UserId IN ('csudreau1', 'bozange1', 'ebertin1', 'VBUCHET1') then 'France'
                           when UserId in( 'sbuechs1', 'acebi1', 'bwernthal1', 'cpletschacher1', 
                           'dnguyen3', 'fkoerbach1','gkirsten1', 'jberger1', 'jgroll1', 'klips1', 'llaboch1'
                           , 'lschallnat1', 'mleuck1', 'mlamer1', 'pazizy1', 'skoehler1', 'tjblock1', 'tjustice1',
                            'ellinger') then 'Germany'
                           when UserId in ('apapia1', 'dpgherra1','elashkor1', 'amakhtar1', 'gjames2'
                           ,'hhendrickson1','jshaw1', 'Jgreen4', 'kmartin1', 'KACRAMOND1','mfeniger1', 'mjtrow1' ,'PDOBSON1'
                           , 'rpannell1', 'SALEX1' ) then 'United Kingdom'
                           when UserId IN ('mpiccio1', 'mzottola1') then 'Italy'
                           WHEN country_description IS NULL THEN 'Unknown'
                         
                           ELSE country_description
                         END                                                           AS Country
FROM   dbo.UserLogonHistory
       LEFT OUTER JOIN dbo.COUNTRIES AS c
                    ON c.country = Upper(Substring(dbo.UserLogonHistory.UserId, 0, 3))