using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using System.Web.UI;

namespace App.Classes.BLL.Pooling.Models {
    public class JavaScriptModel : IJavaScriptModel {

        static String WEBSERVICE=@"~/App.Webservices/Reservations/ReservationUpdateService.svc";
        static String STARTUPSCRIPT=@"<script type='text/javascript' src='~/App.Scripts/Reservations/ReservationUpdate.js'></script>",
            STARTUPSCRIPTNAME="UpdateDBScript";                  

        public void SetServiceReference(System.Web.UI.Page p) {
            ScriptManager.GetCurrent(p).Services.Add(new ServiceReference(WEBSERVICE));
        }
        public void SetJavaScriptService(System.Web.UI.Page p) {
            p.ClientScript.RegisterStartupScript(this.GetType(),STARTUPSCRIPTNAME,STARTUPSCRIPT);
        }
    }
}