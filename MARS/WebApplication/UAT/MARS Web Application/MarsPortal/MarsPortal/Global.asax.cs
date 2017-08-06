using System;
using System.Web.Routing;
using System.Web.Security;
using App.BLL;
using System.Configuration;
using System.Web;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Mars.Pooling.Installer;

namespace MarsPortal {
    public class Global: HttpApplication {

        public static IWindsorContainer CastleContainer { get; private set; }
        public IWindsorContainer BootstrapContainer() {
            return new WindsorContainer()
               .Install( new PoolingInstaller()
               );
        }
        protected void Application_Start(object sender,EventArgs e) {

            
            
            RegisterRoutes(RouteTable.Routes);
            CastleContainer=BootstrapContainer();
 
        }
        static void RegisterRoutes(RouteCollection routes) {
            routes.MapPageRoute("FutureTrend","FutureTrend","~/App.Site/Sizing/FutureTrend/FutureTrend.aspx");
            routes.MapPageRoute("Default","Home","~/Default.aspx");
            routes.MapPageRoute("FleetComparison","FleetComparison","~/App.Site/Sizing/Comparison/Fleet/FleetComparison.aspx");
            routes.MapPageRoute("KPI","KPI","~/App.Site/Reports/Sizing/KPI.aspx");
            routes.MapPageRoute("SiteComparison","SiteComparison","~/App.Site/Sizing/Comparison/Site/SiteComparison.aspx");
            routes.MapPageRoute("SupplyAnalysis","SupplyAnalysis","~/App.Site/Sizing/SupplyAnalysis/SupplyAnalysis.aspx");
            routes.MapPageRoute("ForecastingManagement","ForecastingManagement","~/App.Site/Forecasting/Adjustment/Forecast.aspx");
            routes.MapPageRoute("SizingManagement","SizingManagement","~/App.Site/Sizing/Management/FleetSize.aspx");
            routes.MapPageRoute("Forecast","Forecast","~/App.Site/Forecasting/Forecast/Forecast.aspx");
            routes.MapPageRoute("Benchmark","Benchmark","~/App.Site/Forecasting/Benchmark/Benchmark.aspx");


            // Pooling routes - removed as there's a problem with the chart in teh Day Actual page
            //routes.MapPageRoute("","Pooling/ReservationDetails", "~/App.Site/Pooling/ReservationDetails.aspx");
            //routes.MapPageRoute("","Pooling/DayActuals/{statusType}","~/App.Site/Pooling/DayActuals.aspx",false,new RouteValueDictionary { { "statusType","Thirty" } });
            //routes.Ignore("{*pathInfo}",new { pathInfo = @"^.*(ChartImg.axd)$" });
            //routes.MapPageRoute("","Pooling/Alerts","~/App.Site/Pooling/Alerts.aspx");
            //routes.MapPageRoute("","Pooling/SiteComparison","~/App.Site/Pooling/SiteComparison.aspx");
        }
        protected void Session_Start(object sender,EventArgs e) {
           
        }
        protected void Application_BeginRequest(object sender,EventArgs e) {
        }
        protected void Application_AuthenticateRequest(object sender,EventArgs e) {
        }
        protected void Application_Error(object sender,EventArgs e) {
        }
        protected void Application_End(object sender,EventArgs e) {
            CastleContainer.Dispose();
        }
        protected void Session_End(object sender,EventArgs e) {
        }
        protected void InitializeRoutes(RouteCollection routes) {
            Routes.InitializeRoutes(routes);
        }
    }
}