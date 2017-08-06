using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.UserControls.Pooling {
    public partial class BottomDropDown : System.Web.UI.UserControl {

        static String ONCHANGE="onchange",ONCLICK="onclick";
        public event EventHandler DropDownListCountryEvent;
        public event EventHandler CMSRadioButtonEvent;
        public event EventHandler OPSRadioButtonEvent;
        public event EventHandler DropDownListPoolEvent;
        public event EventHandler DropDownListLocationGroupEvent;
        public event EventHandler DropDownListBranchEvent;
        public event EventHandler DropDownListCarSegmentEvent;
        public event EventHandler DropDownListCarClassEvent;
        public event EventHandler DropDownListCarGroupEvent;
        public String EventCallback=String.Empty;

        protected void Page_Load(object sender, EventArgs e) {
            DropDownListCountry.Attributes.Add(ONCHANGE,EventCallback);
            DropDownListBranch.Attributes.Add(ONCHANGE,EventCallback);
            DropDownListCarClass.Attributes.Add(ONCHANGE,EventCallback);
            DropDownListCarGroup.Attributes.Add(ONCHANGE,EventCallback);
            DropDownListCarSegment.Attributes.Add(ONCHANGE,EventCallback);
            DropDownListLocationGroup.Attributes.Add(ONCHANGE,EventCallback);
            DropDownListPool.Attributes.Add(ONCHANGE,EventCallback);
            RadioButtonCMS.Attributes.Add(ONCLICK, EventCallback);
            RadioButtonOPS.Attributes.Add(ONCLICK, EventCallback);

        }
        protected void DropDownListCountry_SelectedIndexChanged(object sender, EventArgs e) {
            if (DropDownListCountryEvent != null) DropDownListCountryEvent(sender, e);
        }
        protected void RadioButtonCMS_CheckedChanged(object sender, EventArgs e) {
            if (CMSRadioButtonEvent != null) CMSRadioButtonEvent(sender, e);
        }
        protected void RadioButtonOPS_CheckedChanged(object sender, EventArgs e) {
            if (OPSRadioButtonEvent != null) OPSRadioButtonEvent(sender, e);
        }
        protected void DropDownListPool_SelectedIndexChanged(object sender, EventArgs e) {
            if (DropDownListPoolEvent != null) DropDownListPoolEvent(sender, e);
        }
        protected void DropDownListLocationGroup_SelectedIndexChanged(object sender, EventArgs e) {
            if (DropDownListLocationGroupEvent != null) DropDownListLocationGroupEvent(sender, e);
        }
        protected void DropDownListBranch_SelectedIndexChanged(object sender, EventArgs e) {
            if (DropDownListBranchEvent != null) DropDownListBranchEvent(sender, e);
         
        }
        protected void DropDownListCarSegment_SelectedIndexChanged(object sender, EventArgs e) {
            if (DropDownListCarSegmentEvent != null) DropDownListCarSegmentEvent(sender, e);
        }
        protected void DropDownListCarClass_SelectedIndexChanged(object sender, EventArgs e) {
            if (DropDownListCarClassEvent != null) DropDownListCarClassEvent(sender, e);
        }
        protected void DropDownListCarGroup_SelectedIndexChanged(object sender, EventArgs e) {
            if (DropDownListCarGroupEvent != null) DropDownListCarGroupEvent(sender, e);
   
        }
    }
}