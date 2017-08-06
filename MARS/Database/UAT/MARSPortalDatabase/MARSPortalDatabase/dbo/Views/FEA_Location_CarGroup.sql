
CREATE VIEW [dbo].[FEA_Location_CarGroup]
AS
SELECT     dbo.FLEET_EUROPE_STATS.COUNTRY, Settings.vw_CarGroups.car_group_id, Inp.dim_Location.cms_location_group_id, 
                      dbo.FLEET_EUROPE_STATS.OPERATIONAL_FLEET, dbo.FLEET_EUROPE_STATS.IMPORTDATE
FROM         dbo.FLEET_EUROPE_STATS LEFT OUTER JOIN
                      Settings.vw_CarGroups ON dbo.FLEET_EUROPE_STATS.CAR_GROUP = Settings.vw_CarGroups.car_group AND 
                      dbo.FLEET_EUROPE_STATS.COUNTRY = Settings.vw_CarGroups.country LEFT OUTER JOIN
                      Inp.dim_Location ON dbo.FLEET_EUROPE_STATS.LOCATION = Inp.dim_Location.location
WHERE     (dbo.FLEET_EUROPE_STATS.FLEET_RAC_TTL > 0) OR
                      (dbo.FLEET_EUROPE_STATS.FLEET_CARSALES > 0)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'FEA_Location_CarGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[60] 4[12] 2[10] 3) )"
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
         Begin Table = "FLEET_EUROPE_STATS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 372
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw_CarGroups (Settings)"
            Begin Extent = 
               Top = 160
               Left = 267
               Bottom = 349
               Right = 423
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "dim_Location (Inp)"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 148
               Right = 451
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
         Width = 2220
         Width = 2610
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2280
         Alias = 900
         Table = 2235
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1905
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'FEA_Location_CarGroup';

