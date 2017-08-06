using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Pooling.Controllers;
using Mars.Pooling.HTMLFactories;
using Mars.Pooling.HTMLFactories.Abstract;
using App.Classes.DAL.Pooling.Abstract;

using Mars.Entities.Pooling;

namespace Mars.Webservices {
    public partial class AlertsService:System.Web.UI.Page {

        static String HTMLPOPFACTORY="htmlpopfactory";
        IHtmlFactory _facPopup;
        IHtmlFactory FacPopup {
            get {
                if(Session[HTMLPOPFACTORY]==null) {
                    _facPopup=new HtmlFactory(Enums.HtmlTable.AlertsPopup);
                    _facPopup.HtmlTable.Filter=new MainFilterEntity();
                    Session[HTMLPOPFACTORY]=_facPopup;
                }
                return (IHtmlFactory)Session[HTMLPOPFACTORY];
            }
        }
        protected void Page_Load(object sender,EventArgs e) {

            if(!String.IsNullOrEmpty(Request.QueryString["mouseOverNode"])) {
                FacPopup.HtmlTable.Filter.LocAndGoldCar=Request.QueryString["mouseOverNode"];
                Response.Write(FacPopup.GetHTML());
                return;
            }
            Response.Write("No Data");
        }
    }
}