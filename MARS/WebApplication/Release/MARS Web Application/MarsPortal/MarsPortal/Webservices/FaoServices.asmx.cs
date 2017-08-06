using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using App.BLL.Utilities;
using Mars.FleetAllocation.UserControls;

namespace Mars.Webservices
{
    /// <summary>
    /// Summary description for FaoServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class FaoServices : WebService
    {
        
        [ScriptMethod]
        [WebMethod(EnableSession = true)]
        public string GetStage1CalculationStatus()
        {
            var cachedResult = Session[DemandGapDisplay.FaoCalculationStarted];
            if (cachedResult == null)
            {
                return "Calculating";
            }
            
            var resultAsString = cachedResult as string;
            return resultAsString;
        }
    }
}
