CREATE VIEW dbo.qry_TransferDesk
AS
SELECT     TOP (100) PERCENT FEA.OWNAREA, FEA.COUNTRY AS OWNCTRY, LEFT(FEA.LSTWWD, 2) AS LSTCTRY, FEA.LSTWWD, FEA.VC, FEA.UNIT, FEA.MODEL, 
                      FEA.MODDESC, FEA.LICENSE, FEA.SERIAL, FEA.LSTMLG, FEA.OPERSTAT, FEA.MOVETYPE
FROM         dbo.FLEET_EUROPE_ACTUAL AS FEA LEFT OUTER JOIN
                      dbo.ExclusionList AS e ON FEA.LICENSE = e.plate
WHERE     (NOT (FEA.MOVETYPE = 'L-O' OR
                      FEA.MOVETYPE = 'T-O')) AND (NOT (FEA.COUNTRY = LEFT(FEA.LSTWWD, 2))) AND (NOT (FEA.OPERSTAT = 'RT')) AND (LEFT(FEA.LSTWWD, 2) <> 'IR') AND 
                      (FEA.LSTWWD <> 'UKBEL50') AND (FEA.LSTWWD <> 'UKBEL51') AND (FEA.LSTWWD <> 'UKLDY50') OR
                      (NOT (FEA.MOVETYPE = 'L-O' OR
                      FEA.MOVETYPE = 'T-O')) AND (NOT (FEA.COUNTRY = LEFT(FEA.LSTWWD, 2))) AND (LEFT(FEA.LSTWWD, 2) <> 'IR') AND (FEA.LSTWWD <> 'UKBEL50') AND 
                      (FEA.LSTWWD <> 'UKBEL51') AND (FEA.LSTWWD <> 'UKLDY50') AND (NOT (FEA.MOVETYPE = 'R-O')) OR
                      (NOT (FEA.MOVETYPE = 'L-O' OR
                      FEA.MOVETYPE = 'T-O')) AND (NOT (FEA.COUNTRY = LEFT(FEA.LSTWWD, 2))) AND (FEA.OPERSTAT IS NULL) AND (LEFT(FEA.LSTWWD, 2) <> 'IR') AND 
                      (FEA.LSTWWD <> 'UKBEL50') AND (FEA.LSTWWD <> 'UKBEL51') AND (FEA.LSTWWD <> 'UKLDY50') OR
                      (NOT (FEA.MOVETYPE = 'L-O' OR
                      FEA.MOVETYPE = 'T-O')) AND (NOT (FEA.COUNTRY = LEFT(FEA.LSTWWD, 2))) AND (LEFT(FEA.LSTWWD, 2) <> 'IR') AND (FEA.LSTWWD <> 'UKBEL50') AND 
                      (FEA.LSTWWD <> 'UKBEL51') AND (FEA.LSTWWD <> 'UKLDY50') AND (FEA.MOVETYPE IS NULL)
ORDER BY FEA.LSTWWD
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'qry_TransferDesk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[31] 3) )"
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
         Begin Table = "FEA"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 114
               Right = 413
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
      Begin ColumnWidths = 12
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
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'qry_TransferDesk';

