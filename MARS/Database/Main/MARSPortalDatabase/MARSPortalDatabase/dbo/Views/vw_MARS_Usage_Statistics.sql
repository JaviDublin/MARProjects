CREATE VIEW dbo.vw_MARS_Usage_Statistics
AS
SELECT     dbo.MARS_TOOLS.marsToolId, dbo.MARS_TOOLS.marsTool, dbo.MARS_REPORT_NAMES.marsReportId, 
                      dbo.MARS_REPORT_NAMES.marsReportName, dbo.MARS_USAGE_STATISTICS.country, dbo.MARS_USAGE_STATISTICS.cms_pool_id, 
                      dbo.MARS_USAGE_STATISTICS.cms_location_group_id, dbo.MARS_USAGE_STATISTICS.ops_region_id, dbo.MARS_USAGE_STATISTICS.ops_area_id, 
                      dbo.MARS_USAGE_STATISTICS.location, dbo.MARS_USAGE_STATISTICS.racfId, dbo.MARS_USAGE_STATISTICS.report_date
FROM         dbo.MARS_REPORT_NAMES INNER JOIN
                      dbo.MARS_TOOLS ON dbo.MARS_REPORT_NAMES.marsToolId = dbo.MARS_TOOLS.marsToolId INNER JOIN
                      dbo.MARS_USAGE_STATISTICS ON dbo.MARS_REPORT_NAMES.marsReportId = dbo.MARS_USAGE_STATISTICS.reportId
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_MARS_Usage_Statistics';


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
         Begin Table = "MARS_REPORT_NAMES"
            Begin Extent = 
               Top = 13
               Left = 204
               Bottom = 156
               Right = 368
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MARS_TOOLS"
            Begin Extent = 
               Top = 15
               Left = 23
               Bottom = 100
               Right = 175
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MARS_USAGE_STATISTICS"
            Begin Extent = 
               Top = 6
               Left = 430
               Bottom = 186
               Right = 635
            End
            DisplayFlags = 280
            TopColumn = 1
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_MARS_Usage_Statistics';

