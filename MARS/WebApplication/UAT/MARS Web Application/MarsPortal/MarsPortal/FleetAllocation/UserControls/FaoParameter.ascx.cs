using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.UserControls.Phase4;
using Mars.FleetAllocation.DataAccess.ParameterAccess;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.UserControls
{
    public partial class FaoParameter : UserControl
    {
        public bool HideCarClass { set { vhParams.HideCarClass = value; } }
        public bool HideCarGroup { set { vhParams.HideCarGroup = value; } }

        public bool HideLocationBranch { set { vhParams.HideLocationBranch = value; } }

        public bool ShowCommercialCarSegment { set { pnlCommercialCarSegment.Visible = value; } }

        public bool ShowMonthSelector { set { pnlMonthSelection.Visible = value; } }

        public bool ShowDayOfWeek { set { pnlDayOfWeek.Visible = value; } }

        public string LocationQuickSelect { get; set; }

        public string UpdatePanelClientId { get { return upnlParameters.ClientID; } }

        public bool AllowAdvancedParameters { set { cbBasicParameters.Visible = value; } }

        public Dictionary<DictionaryParameter, string> SessionStoredFaoMinCommSegParameters
        {
            get { return vhParams.SessionStoredAvailabilityParameters; }
            set { vhParams.SessionStoredAvailabilityParameters = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var dataAccess = new FaoDataContext())
                {
                    var segments = ListItemAccess.GetCommercialCarSegments(dataAccess, true);
                    lbCommercialCarSegment.Items.AddRange(segments.ToArray());
                    var daysOfWeek = ListItemAccess.GetDayOfWeeks(true);
                    lbDayOfWeek.Items.AddRange(daysOfWeek.ToArray());
                }
                FillMonthSelector();
            }

            

        }

        private void FillMonthSelector()
        {
            var months = new List<ListItem>();
            for (int i = 0; i < 12; i++) 
            {
                months.Add(new ListItem(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i], (i + 1).ToString(CultureInfo.InvariantCulture)));
            }
            ddlMonth.Items.AddRange(months.ToArray());
            tbYear.Text = DateTime.Now.Year.ToString();
        }

        public Dictionary<DictionaryParameter, string> GetParameters()
        {
            var returned = vhParams.Visible ? vhParams.BuildParameterDictionary() : vhMultiParams.BuildParameterDictionary();
            if (lbCommercialCarSegment.Visible)
            {
                var selectedCcs = AvailabilityParameters.GetSelectedKeys(lbCommercialCarSegment.Items);
                returned.Add(DictionaryParameter.CommercialCarSegment, selectedCcs);    
            }
            
            if (lbDayOfWeek.Visible)
            {
                var selectedDows = AvailabilityParameters.GetSelectedKeys(lbDayOfWeek.Items, false);
                returned.Add(DictionaryParameter.DayOfWeek, selectedDows);    
            }

            if (pnlMonthSelection.Visible)
            {
                
                var monthSelected = int.Parse(ddlMonth.SelectedValue);
                var yearSelected = int.Parse(tbYear.Text);
                var dateSelected = new DateTime(yearSelected, monthSelected, 1);
                returned.Add(DictionaryParameter.StartDate, dateSelected.ToShortDateString());    
            }

            returned.Add(DictionaryParameter.FleetTypes, "4,5,6");
            
            return returned;
        }

        protected void cbBasicParameters_CheckedChanged(object sender, EventArgs e)
        {
            vhParams.Visible = cbBasicParameters.Checked;
            vhMultiParams.Visible = !cbBasicParameters.Checked;
            upnlParameters.Update();
        }
    }
}