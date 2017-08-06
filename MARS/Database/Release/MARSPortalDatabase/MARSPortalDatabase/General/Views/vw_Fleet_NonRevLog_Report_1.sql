CREATE VIEW [General].[vw_Fleet_NonRevLog_Report]
AS
SELECT     df.Country, nr.Lstwwd, lops.ops_area_id, lops.ops_region_id, lcms.cms_location_group_id, lcms.cms_pool_id, cc.car_group_id, cc.car_class_id, cc.car_segment_id, 
                      nr.OperStat, os.KCICode, nr.MoveType, nr.IsOpen, nr.Fleet_rac_ops, nr.Fleet_rac_ttl, nr.Fleet_carsales, nr.Fleet_licensee, nr.Fleet_adv, nr.Fleet_hod, nr.TotalFleet, 
                      nr.AvailableFleet, nr.OperationalFleet, nr.DayGroupCode, nr.RemarkId, nr.CountryLoc, nr.CountryCar
FROM         General.Dim_Fleet AS df INNER JOIN
                      General.Fact_NonRevLog AS nr ON df.VehicleId = nr.VehicleId INNER JOIN
                      Settings.Operational_Status AS os ON nr.OperStat = os.OperationalStatusCode INNER JOIN
                      Settings.vw_CarGroups AS cc ON df.CarGroup = cc.car_group AND df.Country = cc.country AND cc.Counter = 1 INNER JOIN
                      Settings.vw_Locations_CMS AS lcms ON nr.Lstwwd = lcms.location AND lcms.Counter = 1 INNER JOIN
                      Settings.vw_Locations_OPS AS lops ON nr.Lstwwd = lops.location AND lops.Counter = 1
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'General', @level1type = N'VIEW', @level1name = N'vw_Fleet_NonRevLog_Report';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'th = 1500
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
', @level0type = N'SCHEMA', @level0name = N'General', @level1type = N'VIEW', @level1name = N'vw_Fleet_NonRevLog_Report';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[22] 4[4] 2[35] 3) )"
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
         Begin Table = "df"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 200
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "nr"
            Begin Extent = 
               Top = 6
               Left = 238
               Bottom = 114
               Right = 398
            End
            DisplayFlags = 280
            TopColumn = 41
         End
         Begin Table = "os"
            Begin Extent = 
               Top = 6
               Left = 436
               Bottom = 114
               Right = 630
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cc"
            Begin Extent = 
               Top = 6
               Left = 668
               Bottom = 114
               Right = 824
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "lcms"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 227
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "lops"
            Begin Extent = 
               Top = 114
               Left = 265
               Bottom = 222
               Right = 440
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
         Wid', @level0type = N'SCHEMA', @level0name = N'General', @level1type = N'VIEW', @level1name = N'vw_Fleet_NonRevLog_Report';

