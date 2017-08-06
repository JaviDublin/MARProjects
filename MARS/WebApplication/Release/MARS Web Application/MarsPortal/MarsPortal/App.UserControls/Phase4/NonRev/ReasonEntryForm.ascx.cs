using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.NonRev;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class ReasonEntryForm : UserControl
    {
        public int SelectedReasonId
        {
            get { return int.Parse(ddlReasonCode.SelectedValue); }
        }

        public bool ShowAddButton { set { btnAddEntry.Visible = value; } }

        public bool Disabled {
            set
            {
                pnlReasonEntry.Visible = !value;
            } 
        }

        public DateTime? EstimatedResolutionDate
        {
            get
            {
                DateTime returned;
                if (!DateTime.TryParse(tbEstimatedResolvedDate.Text, out returned))
                {
                    return null;
                }
                return returned;
            }
        }

        public string RemarkEntered
        {
            get { return taRemarks.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                using (var dataAccess = new NonRevBaseDataAccess(null))
                {
                    ddlReasonCode.Items.AddRange(dataAccess.GetRemarkReasonsList().ToArray());
                }
            }
        }

        public void ClearFields()
        {
            taRemarks.Text = string.Empty;
            tbEstimatedResolvedDate.Text = string.Empty;
            ddlReasonCode.SelectedIndex = 0;
        }

        protected void btnAddEntry_Click(object sender, EventArgs e)
        {
            if (ddlReasonCode.SelectedIndex == 0)
            {
                lblErrorMessage.Text = "No Reason Selected";
                RaiseBubbleEvent(this, new CommandEventArgs("KeepPopupOpen", "Keep Popup Open"));
                return;
            }
            if (EstimatedResolutionDate == null)
            {
                lblErrorMessage.Text = "Invalid Date Entered";
                RaiseBubbleEvent(this, new CommandEventArgs("KeepPopupOpen", "Keep Popup Open"));
                return;
            }

            

            RaiseBubbleEvent(this, new CommandEventArgs("NewRemark", null));
        }

        public void SetMultipleEntryNoReasonMessage()
        {
            lblErrorMessage.Text = "No Reason Selected";
            RaiseBubbleEvent(this, new CommandEventArgs("KeepMultipleEntryPopupOpen", "Keep Popup Open"));
        }

        public void SetMultipleEntryInvalidDateMessage()
        {
            lblErrorMessage.Text = "Invalid Date Selected";
            RaiseBubbleEvent(this, new CommandEventArgs("KeepMultipleEntryPopupOpen", "Keep Popup Open"));
        }
    }
}