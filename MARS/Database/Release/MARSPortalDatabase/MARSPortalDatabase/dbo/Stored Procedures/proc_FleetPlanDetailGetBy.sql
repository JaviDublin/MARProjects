CREATE PROCEDURE [dbo].[proc_FleetPlanDetailGetBy]
@country NVARCHAR (4000), @locationGroup INT, @carClassGroup INT, @startDate DATETIME, @endDate DATETIME
AS EXTERNAL NAME [SqlServerProject1].[StoredProcedures].[proc_FleetPlanDetailGetBy]


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'17', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_FleetPlanDetailGetBy';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'Management.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'proc_FleetPlanDetailGetBy';

