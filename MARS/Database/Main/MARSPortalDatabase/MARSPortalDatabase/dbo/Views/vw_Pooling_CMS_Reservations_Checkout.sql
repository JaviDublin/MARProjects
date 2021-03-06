﻿
CREATE VIEW [dbo].[vw_Pooling_CMS_Reservations_Checkout]
AS
SELECT     dbo.vw_Pooling_Locations_CMS.location, dbo.vw_Pooling_Locations_CMS.cms_location_group_id, dbo.vw_Pooling_Locations_CMS.cms_pool_id, 
                      dbo.vw_Pooling_Locations_CMS.country, dbo.vw_Pooling_Vehicles.car_group_id, dbo.vw_Pooling_Vehicles.car_class_id, 
                      dbo.vw_Pooling_Vehicles.car_segment_id, dbo.RESERVATIONS_EUROPE_ACTUAL.CO_HOURS, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.CO_DAYS, dbo.RESERVATIONS_EUROPE_ACTUAL.GS, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.PREPAID, dbo.RESERVATIONS_EUROPE_ACTUAL.PREDELIVERY, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.CI_HOURS, dbo.RESERVATIONS_EUROPE_ACTUAL.ONEWAY, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.CI_HOURS_OFFSET, dbo.RESERVATIONS_EUROPE_ACTUAL.CI_DAYS, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.RENT_LOC, dbo.RESERVATIONS_EUROPE_ACTUAL.GR_INCL_GOLDUPGR, 
                      dbo.vw_Pooling_Locations_CMS.cms_pool, dbo.vw_Pooling_Locations_CMS.cms_location_group, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.RES_ID_NBR, dbo.RESERVATIONS_EUROPE_ACTUAL.RES_LOC, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.RS_ARRIVAL_DATE, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.RS_ARRIVAL_TIME, dbo.RESERVATIONS_EUROPE_ACTUAL.RTRN_DATE, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.RTRN_TIME, dbo.RESERVATIONS_EUROPE_ACTUAL.RES_DAYS, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.RATE_QUOTED, dbo.RESERVATIONS_EUROPE_ACTUAL.CUST_NAME, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.CDPID_NBR, dbo.RESERVATIONS_EUROPE_ACTUAL.NO1_CLUB_GOLD, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.FLIGHT_NBR, dbo.RESERVATIONS_EUROPE_ACTUAL.RTRN_LOC, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.RES_VEH_CLASS, dbo.RESERVATIONS_EUROPE_ACTUAL.PHONE, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.N1TYPE, dbo.RESERVATIONS_EUROPE_ACTUAL.NEVERLOST, 
                      dbo.RESERVATIONS_EUROPE_ACTUAL.REMARKS
FROM         dbo.vw_Pooling_Vehicles INNER JOIN
                      dbo.RESERVATIONS_EUROPE_ACTUAL ON 
                      dbo.vw_Pooling_Vehicles.car_group = dbo.RESERVATIONS_EUROPE_ACTUAL.GR_INCL_GOLDUPGR AND 
                      dbo.vw_Pooling_Vehicles.country = dbo.RESERVATIONS_EUROPE_ACTUAL.COUNTRY INNER JOIN
                      dbo.vw_Pooling_Locations_CMS ON dbo.RESERVATIONS_EUROPE_ACTUAL.RENT_LOC = dbo.vw_Pooling_Locations_CMS.location
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Pooling_CMS_Reservations_Checkout';


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
         Begin Table = "vw_Pooling_Vehicles"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 233
               Right = 195
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RESERVATIONS_EUROPE_ACTUAL_QUERY_TABLE"
            Begin Extent = 
               Top = 3
               Left = 271
               Bottom = 260
               Right = 455
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "vw_Pooling_Locations_CMS"
            Begin Extent = 
               Top = 19
               Left = 568
               Bottom = 204
               Right = 773
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Pooling_CMS_Reservations_Checkout';

