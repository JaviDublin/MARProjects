using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.DAL.Sizing.Abstract;
using Mars.DAL.Sizing;
using Mars.Entities.Sizing.Abstract;

namespace Mars.Webservices.Sizing {

    public partial class SizingService:System.Web.UI.Page {

        static IFleetPlanWebServiceRepository _flp;
        IFleetPlanWebServiceRepository _fleetPlanWebServiceRepository {
            get {
                if(_flp == null) _flp = new FleetPlanWebServiceRepository();
                return _flp;
            }
        }
        protected void Page_Load(object sender,EventArgs e) {
            IFleetPlanEntity ifpe=_fleetPlanWebServiceRepository.GetMessage();
            Response.Write("{ 'Action' : 'No message' , 'Message' : 'No message'}");
            if(ifpe==null) { return; }
            Response.Write("{ 'Action' : '"+_fleetPlanWebServiceRepository.GetMessage().Status+"' , 'Message' : '"+_fleetPlanWebServiceRepository.GetMessage().Message+"'}");
        }
    }
}