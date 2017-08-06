
namespace App.BLL.Utilities
{

        public enum DateFormats
        {
            NoTime = 1,
            StartOfDay = 2,
            EndOfDay = 3,
            IncludeTime = 4
        };

        public enum ResultCode
        {
            Success = 0,
            Failed = -1,
            Duplicate = 1,
            NotAuthenticated = 2,
            Initialized = -2
        };

        public enum RoleCodes
        {
            Administrator = 1,
            TDAdjustmentAndReconciliation = 2,
            BU1Adjustment = 3,
            BU2Adjustment = 4
        };

        public enum AdjustmentType
        {
            Amount = 1,
            PerCent
        };

        public enum AdjustmentForecast
        {
            OnRent_LY = 1,
            Constrained,
            Unconstrained,
        }

        public enum Adjustment
        {
            Adjustment_TD = 2,
            Adjustment_BU1 = 3,
            Adjustment_BU2 = 4,
        }

        public enum ReportTypeExport
        {
            FutureTrend = 1,
            SupplyAnalysis,
            FleetComparison,
            SiteComparison,
            KPI,
            FleetPlanDetail,
            Forecast,
            Benchmark,
            NecessaryFleet
        }

        public enum ScenarioType
        {
            Actual = 1,
            Scenario1 = 2,
            Scenario2 = 3,
            Scenario3
        }

        public enum LocationLogic
        {
            Cms,
            Ops
        }

}