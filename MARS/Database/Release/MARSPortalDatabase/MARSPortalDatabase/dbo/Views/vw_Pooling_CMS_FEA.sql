CREATE VIEW [dbo].[vw_Pooling_CMS_FEA]
AS
SELECT     dbo.vw_Pooling_Locations_CMS.location, dbo.vw_Pooling_Locations_CMS.cms_location_group_id, dbo.vw_Pooling_Locations_CMS.cms_pool_id, 
                      dbo.vw_Pooling_Locations_CMS.country, dbo.vw_Pooling_Vehicles.car_group_id, dbo.vw_Pooling_Vehicles.car_class_id, 
                      dbo.vw_Pooling_Vehicles.car_segment_id, dbo.FLEET_EUROPE_ACTUAL.RT, 
                      dbo.FLEET_EUROPE_ACTUAL.CI_HOURS, dbo.FLEET_EUROPE_ACTUAL.CI_HOURS_OFFSET, 
                      dbo.FLEET_EUROPE_ACTUAL.ON_RENT, dbo.FLEET_EUROPE_ACTUAL.CI_DAYS, 
                      dbo.FLEET_EUROPE_ACTUAL.DUEWWD, dbo.FLEET_EUROPE_ACTUAL.LSTWWD, 
                      dbo.FLEET_EUROPE_ACTUAL.MOVETYPE, dbo.FLEET_EUROPE_ACTUAL.TOTAL_FLEET, 
                      dbo.FLEET_EUROPE_ACTUAL.OVERDUE, dbo.FLEET_EUROPE_ACTUAL.VC, 
                      dbo.vw_Pooling_Locations_CMS.cms_pool, dbo.vw_Pooling_Vehicles.car_group, dbo.vw_Pooling_Vehicles.car_class, 
                      dbo.vw_Pooling_Vehicles.car_segment, dbo.vw_Pooling_Locations_CMS.cms_location_group
FROM         dbo.vw_Pooling_Vehicles INNER JOIN
                      dbo.FLEET_EUROPE_ACTUAL ON dbo.vw_Pooling_Vehicles.car_group = dbo.FLEET_EUROPE_ACTUAL.VC AND 
                      dbo.vw_Pooling_Vehicles.country = dbo.FLEET_EUROPE_ACTUAL.COUNTRY INNER JOIN
                      dbo.vw_Pooling_Locations_CMS ON dbo.FLEET_EUROPE_ACTUAL.LSTWWD = dbo.vw_Pooling_Locations_CMS.location
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Pooling_CMS_FEA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[39] 4[15] 2[17] 3) )"
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
         Begin Table = "vw_Pooling_Vehicles"
            Begin Extent = 
               Top = 162
               Left = 53
               Bottom = 281
               Right = 256
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "FLEET_EUROPE_ACTUAL_QUERY_TABLE"
            Begin Extent = 
               Top = 27
               Left = 318
               Bottom = 197
               Right = 505
            End
            DisplayFlags = 280
            TopColumn = 47
         End
         Begin Table = "vw_Pooling_Locations_CMS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 133
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 1500
         Width = 2340
         Width = 1500
         Width = 1500
         Width = 1500
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Pooling_CMS_FEA';





