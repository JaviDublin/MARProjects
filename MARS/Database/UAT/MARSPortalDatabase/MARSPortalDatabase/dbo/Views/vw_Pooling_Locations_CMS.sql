﻿CREATE VIEW dbo.vw_Pooling_Locations_CMS
AS
SELECT     dbo.LOCATIONS.location, dbo.CMS_LOCATION_GROUPS.cms_location_group_id, dbo.CMS_POOLS.cms_pool_id, dbo.COUNTRIES.country, 
                      dbo.CMS_POOLS.cms_pool, dbo.CMS_LOCATION_GROUPS.cms_location_group
FROM         dbo.LOCATIONS INNER JOIN
                      dbo.CMS_LOCATION_GROUPS ON dbo.LOCATIONS.cms_location_group_id = dbo.CMS_LOCATION_GROUPS.cms_location_group_id INNER JOIN
                      dbo.CMS_POOLS ON dbo.CMS_LOCATION_GROUPS.cms_pool_id = dbo.CMS_POOLS.cms_pool_id INNER JOIN
                      dbo.COUNTRIES ON dbo.CMS_POOLS.country = dbo.COUNTRIES.country
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Pooling_Locations_CMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'50
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Pooling_Locations_CMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[10] 3) )"
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
         Begin Table = "LOCATIONS"
            Begin Extent = 
               Top = 9
               Left = 25
               Bottom = 252
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "CMS_LOCATION_GROUPS"
            Begin Extent = 
               Top = 6
               Left = 281
               Bottom = 121
               Right = 506
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMS_POOLS"
            Begin Extent = 
               Top = 6
               Left = 544
               Bottom = 106
               Right = 696
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "COUNTRIES"
            Begin Extent = 
               Top = 160
               Left = 545
               Bottom = 275
               Right = 721
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
         Or = 13', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Pooling_Locations_CMS';
