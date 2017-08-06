using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using App.BLL.Utilities;
using Mars.FleetAllocation.BusinessLogic;
using Mars.FleetAllocation.UserControls;
using Mars.FleetAllocation.UserControls.DemandGapProgress;

namespace Mars.Webservices
{
    /// <summary>
    /// Summary description for FaoServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class FaoServices : WebService
    {
        
        [ScriptMethod]
        [WebMethod(EnableSession = true)]
        public string GetFaoCalculationStatus()
        {
            var progressEnum = (DemandGapCalculationStep)Enum.Parse(typeof(DemandGapCalculationStep), Session[DemandGapCalculationProgress.CalculationProgressSessionString].ToString());

            if (progressEnum == DemandGapCalculationStep.NotStarted)
            {
                return "";
            }
            
            return "Refresh";
        }
    }
}
