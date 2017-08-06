

CREATE VIEW [dbo].[vwPageUsage]
AS
SELECT DISTINCT 
                      TOP (100) PERCENT dbo.StatisticsPageAccess.Url, 
                      dbo.StatisticsPageAccess.AccessedOn, 
                      dbo.StatisticsPageAccess .AccessedBy ,
                      dbo.StatisticsPageAccess .UserName ,
                      CONVERT(VARCHAR, dbo.StatisticsPageAccess.AccessedOn, 103)  AS AccessedDate, 
                      CONVERT(VARCHAR, dbo.StatisticsPageAccess.AccessedOn, 108) AS AccessedTime, 
                      DATENAME(month, CAST(dbo.StatisticsPageAccess.AccessedOn AS DATE)) AS Month, 
                      DATENAME(year, CAST(dbo.StatisticsPageAccess.AccessedOn AS DATE)) AS Year, 
                      DATENAME(ISO_WEEK, CAST(dbo.StatisticsPageAccess.AccessedOn AS DATE)) AS WeekNo, 
                      CASE WHEN AccessedBy IN ('csudreau1', 'bozange1', 'ebertin1', 
                      'VBUCHET1') 
                      THEN 'France' 
                      WHEN AccessedBy IN ('sbuechs1', 'acebi1', 'bwernthal1', 'cpletschacher1', 'dnguyen3', 'fkoerbach1', 'gkirsten1', 'jberger1', 'jgroll1', 
                      'klips1', 'llaboch1', 'lschallnat1', 'mleuck1', 'mlamer1', 'pazizy1', 'skoehler1', 'tjblock1', 'tjustice1', 'ellinger') THEN 'Germany' WHEN AccessedBy IN ('apapia1', 
                      'dpgherra1', 'elashkor1', 'amakhtar1', 'gjames2', 'hhendrickson1', 'jshaw1', 'Jgreen4', 'kmartin1', 'KACRAMOND1', 'mfeniger1', 'mjtrow1', 'PDOBSON1', 'rpannell1', 
                      'SALEX1') 
                      THEN 'United Kingdom' 
                      WHEN AccessedBy IN ('mpiccio1', 'mzottola1') 
                      THEN 'Italy' 
                      WHEN country_description IS NULL 
                      THEN 'Unknown' 
                      ELSE country_description 
                      END AS Country
FROM         dbo.StatisticsPageAccess LEFT OUTER JOIN
                      dbo.COUNTRIES AS c ON c.country = 
                      UPPER(SUBSTRING(dbo.StatisticsPageAccess.AccessedBy, 0, 3))
ORDER BY dbo.StatisticsPageAccess.AccessedOn DESC
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwPageUsage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "StatisticsPageAccess"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 237
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 275
               Bottom = 125
               Right = 459
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwPageUsage';

