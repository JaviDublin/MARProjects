
CREATE VIEW [dbo].[CmsForecastView]
AS
SELECT     mcf.REP_DATE, mcf.COUNTRY, mcf.CMS_LOCATION_GROUP_ID, mcf.CAR_CLASS_ID AS Car_Group_Id, mcf.CONSTRAINED, mcf.UNCONSTRAINED, mcf.FLEET, 
                      mcf.RESERVATIONS_BOOKED, mcf.CURRENT_ONRENT, mcf.FORECASTED_RETURNS, mcf.ONRENT_LY, mcf.AVAILABLE_FLEET, 
                      COALESCE ((CASE UTILISATION WHEN 0 THEN 0 ELSE CASE NONREV_FLEET WHEN 100 THEN 0 ELSE (mcf.CONSTRAINED / UTILISATION * 100) 
                      / ((100 - NONREV_FLEET) / 100) END END), mcf.CONSTRAINED, 0) AS NecessaryConstrained, 
                      COALESCE ((CASE UTILISATION WHEN 0 THEN 0 ELSE CASE NONREV_FLEET WHEN 100 THEN 0 ELSE (mcf.unconstrained / UTILISATION * 100) 
                      / ((100 - NONREV_FLEET) / 100) END END), mcf.UNCONSTRAINED, 0) AS NecessaryUnconstrained, 
                      COALESCE ((CASE UTILISATION WHEN 0 THEN 0 ELSE CASE NONREV_FLEET WHEN 100 THEN 0 ELSE (mcf.reservations_booked / UTILISATION * 100) 
                      / ((100 - NONREV_FLEET) / 100) END END), mcf.CONSTRAINED, 0) AS NecessaryBooked, nft.NONREV_FLEET, nft.UTILISATION, 
                      dbo.CMS_LOCATION_GROUPS.cms_pool_id, dbo.CAR_GROUPS.car_class_id, dbo.CAR_CLASSES.car_segment_id, 
                      dbo.CMS_LOCATION_GROUPS.cms_location_group, dbo.CAR_GROUPS.car_group, dbo.CAR_CLASSES.car_class, dbo.CAR_SEGMENTS.car_segment, 
                      dbo.CMS_POOLS.cms_pool, c.country_description, mcf.OPERATIONAL_FLEET as OPERATIONAL_FLEET, mcf.TOTAL_FLEET as TOTAL_FLEET
FROM         dbo.MARS_CMS_NECESSARY_FLEET AS nft FULL OUTER JOIN
                      dbo.MARS_CMS_FORECAST AS mcf LEFT OUTER JOIN
                      dbo.CAR_GROUPS ON mcf.CAR_CLASS_ID = dbo.CAR_GROUPS.car_group_id LEFT OUTER JOIN
                      dbo.CAR_CLASSES ON dbo.CAR_GROUPS.car_class_id = dbo.CAR_CLASSES.car_class_id LEFT OUTER JOIN
                      dbo.CAR_SEGMENTS ON dbo.CAR_CLASSES.car_segment_id = dbo.CAR_SEGMENTS.car_segment_id LEFT OUTER JOIN
                      dbo.CMS_POOLS INNER JOIN
                      dbo.CMS_LOCATION_GROUPS ON dbo.CMS_POOLS.cms_pool_id = dbo.CMS_LOCATION_GROUPS.cms_pool_id ON 
                      mcf.CMS_LOCATION_GROUP_ID = dbo.CMS_LOCATION_GROUPS.cms_location_group_id ON nft.CMS_LOCATION_GROUP_ID = mcf.CMS_LOCATION_GROUP_ID AND 
                      nft.CAR_CLASS_ID = mcf.CAR_CLASS_ID INNER JOIN
                      dbo.COUNTRIES AS c ON nft.COUNTRY = c.country
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'CmsForecastView';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 268
               Bottom = 114
               Right = 443
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
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 15705
         Alias = 2655
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'CmsForecastView';


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
         Begin Table = "nft"
            Begin Extent = 
               Top = 6
               Left = 647
               Bottom = 114
               Right = 864
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "mcf"
            Begin Extent = 
               Top = 13
               Left = 13
               Bottom = 121
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "CAR_GROUPS"
            Begin Extent = 
               Top = 13
               Left = 454
               Bottom = 121
               Right = 609
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "CAR_CLASSES"
            Begin Extent = 
               Top = 135
               Left = 457
               Bottom = 243
               Right = 613
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CAR_SEGMENTS"
            Begin Extent = 
               Top = 257
               Left = 461
               Bottom = 365
               Right = 628
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMS_POOLS"
            Begin Extent = 
               Top = 150
               Left = 260
               Bottom = 243
               Right = 411
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMS_LOCATION_GROUPS"
            Begin Extent = 
               Top = 147
               Left = 18
               Bottom = 255
               Right = 242
            End
           ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'CmsForecastView';

