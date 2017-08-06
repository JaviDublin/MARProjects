using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Entities.VehiclesAbroad;
using App.BLL.VehiclesAbroad; // added

namespace App.UserControls {

    public partial class VehiclesAbroadDetails : System.Web.UI.UserControl {

        public void setTable(ICarSearchDataEntity csdf) {
            // populate the table with the appropriate data

            if (csdf != null) {
                tdGroup.InnerHtml = csdf.Vc;
                tdModelcode.InnerHtml = csdf.Model;
                tdModel.InnerHtml = csdf.Moddesc;
                tdUnit.InnerHtml = csdf.Unit;
                tdLicense.InnerHtml = csdf.License;
                tdVin.InnerHtml = csdf.Doc;
                tdCharged.InnerHtml = csdf.Charged;
                tdLastDoc.InnerHtml = csdf.Doc;
                tdLstwwd.InnerHtml = csdf.Lstwwd;
                tdLstdate.InnerHtml = csdf.Lstdate == null ? "" : csdf.Lstdate.Value.ToShortDateString();
                tdLstmlg.InnerHtml = csdf.Lstmlg.ToString();
                tdOp.InnerHtml = csdf.Op;
                tdMt.InnerHtml = csdf.Mt;
                tdDuewwd.InnerHtml = csdf.Duewwd;
                tdDuedate.InnerHtml = csdf.Duedate == null ? "" : csdf.Duedate.Value.ToShortDateString();
                tdDuetime.InnerHtml = csdf.Duetime;
                tdDriver.InnerHtml = csdf.Driver;
                tdNonrev.InnerHtml = csdf.Nonrev.ToString(); ;
                tdRegdate.InnerHtml = csdf.Regdate;
                tdBlockdate.InnerHtml = csdf.Blockdate;
                tdRemarkdate.InnerHtml = csdf.Remarkdate;
                tdOwnarea.InnerHtml = csdf.Ownarea;
                tdCarhold.InnerHtml = csdf.Hold;
                tdBddays.InnerHtml = csdf.Bddays;
                tdMmdays.InnerHtml = csdf.Mmdays;
                tdPrewwd.InnerHtml = csdf.Prevwwd;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            // does nothing
        }

        protected void ButtonSave_Click(object sender, EventArgs e) {
            VehicleDetailModel vdm = new VehicleDetailModel();
            vdm.saveRemark(tdLicense.InnerText, remarksText.InnerText);
            UpdatePanelVehicleDetailsModal.Visible = false;
        }

        protected void ButtonClose_Click(object sender, EventArgs e) {
            UpdatePanelVehicleDetailsModal.Visible = false;
        }
    }
}