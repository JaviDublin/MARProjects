CREATE VIEW Inp.FLEET_EUROPE_SUMMARY_HISTORY
AS
SELECT     d.Rep_Year, d.Rep_Month, d.Rep_WeekOfYear AS REP_WEEK_OF_YEAR, d.Rep_DayOfWeek AS REP_DAY_OF_WEEK, d.Rep_Date, f.COUNTRY, l.location, 
                      f.car_group, f.FLEET_RAC_TTL, f.FLEET_RAC_OPS, f.FLEET_CARSALES, f.FLEET_LICENSEE, f.TOTAL_FLEET, f.CARSALES, f.CARHOLD_H, f.CARHOLD_L, f.CU, f.HA, 
                      f.HL, f.LL, f.NC, f.PL, f.TC, f.SV, f.WS, f.WS_NONRAC, f.OPERATIONAL_FLEET, f.BD, f.MM, f.TW, f.TB, f.WS_RAC, f.AVAILABLE_TB, f.FS, f.RL, f.RP, f.TN, 
                      f.AVAILABLE_FLEET, f.RT, f.SU, f.GOLD, f.PREDELIVERY, f.OVERDUE, f.ON_RENT, f.TOTAL_FLEET_MIN, f.CARSALES_MIN, f.CARHOLD_H_MIN, f.CARHOLD_L_MIN, 
                      f.CU_MIN, f.HA_MIN, f.HL_MIN, f.LL_MIN, f.NC_MIN, f.PL_MIN, f.TC_MIN, f.SV_MIN, f.WS_MIN, f.WS_NONRAC_MIN, f.OPERATIONAL_FLEET_MIN, f.BD_MIN, 
                      f.MM_MIN, f.TW_MIN, f.TB_MIN, f.WS_RAC_MIN, f.AVAILABLE_TB_MIN, f.FS_MIN, f.RL_MIN, f.RP_MIN, f.TN_MIN, f.AVAILABLE_FLEET_MIN, f.RT_MIN, f.SU_MIN, 
                      f.GOLD_MIN, f.PREDELIVERY_MIN, f.OVERDUE_MIN, f.ON_RENT_MIN, f.TOTAL_FLEET_MAX, f.CARSALES_MAX, f.CARHOLD_H_MAX, f.CARHOLD_L_MAX, f.CU_MAX, 
                      f.HA_MAX, f.HL_MAX, f.LL_MAX, f.NC_MAX, f.PL_MAX, f.TC_MAX, f.SV_MAX, f.WS_MAX, f.WS_NONRAC_MAX, f.OPERATIONAL_FLEET_MAX, f.BD_MAX, f.MM_MAX, 
                      f.TW_MAX, f.TB_MAX, f.WS_RAC_MAX, f.AVAILABLE_TB_MAX, f.FS_MAX, f.RL_MAX, f.RP_MAX, f.TN_MAX, f.AVAILABLE_FLEET_MAX, f.RT_MAX, f.SU_MAX, 
                      f.GOLD_MAX, f.PREDELIVERY_MAX, f.OVERDUE_MAX, f.ON_RENT_MAX, f.TOTAL_FLEET_MAX_ABSOLUTE, f.OPERATIONAL_FLEET_MAX_ABSOLUTE, 
                      f.OVERDUE_MAX_ABSOLUTE, f.ON_RENT_MAX_ABSOLUTE
FROM         Inp.Fact_FLEET_EUROPE_SUMMARY_HISTORY AS f INNER JOIN
                      Inp.dim_Calendar AS d ON f.dim_Calendar_id = d.dim_Calendar_id INNER JOIN
                      Inp.dim_Location AS l ON f.dim_Location_id = l.dim_Location_id
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'Inp', @level1type = N'VIEW', @level1name = N'FLEET_EUROPE_SUMMARY_HISTORY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[28] 3) )"
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
         Begin Table = "f"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 308
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 204
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 114
               Left = 242
               Bottom = 222
               Right = 431
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
', @level0type = N'SCHEMA', @level0name = N'Inp', @level1type = N'VIEW', @level1name = N'FLEET_EUROPE_SUMMARY_HISTORY';





