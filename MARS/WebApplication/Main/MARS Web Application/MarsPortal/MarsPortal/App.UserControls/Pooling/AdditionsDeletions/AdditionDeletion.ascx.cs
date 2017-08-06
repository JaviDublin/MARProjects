using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.Pooling.AdditionDeletion;

namespace Mars.App.UserControls.Pooling.AdditionsDeletions
{
    public partial class AdditionDeletion : UserControl
    {
        public  AdditionDeletionBusinessLogic AddDelBl { set; private get; }

        /// <summary>
        /// This is used to pass a country Id to the AutoComplete function
        /// </summary>
        public string CarGroupCountry
        {
            set
            {
                acCarGroup.ContextKey = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var javascript = new StringBuilder();
            javascript.Append(string.Format(@"
            $('.QuickLocationGroupInput').keyup(function(event){{
                        if (event.keyCode == 13) {{
                            __doPostBack('{0}', '');   
                            
                    }}
                    }});
                ", upAddDel.ClientID));

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "registerInitializer", javascript.ToString(), true);
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var message = AddAdditionOrDeletion();
            lblMessage.Text = message;
        }

        private string AddAdditionOrDeletion()
        {
            if (acCarGroup.ContextKey == string.Empty)
            {
                return "Select a Country to Map Car Group Codes from";
            }

            if (tbWwd.Text.Substring(0, 2).ToLower() != acCarGroup.ContextKey.ToLower())
            {
                return "The WWD code must match the Country selected in the Parameter Selection";
            }

            var repDate = sdpRepDate.SelectedDateTime;
            if (repDate == null)
            {
                return "Invalid Date Entered";
            }
            int number;
            var succeeded = int.TryParse(tbValue.Text, out number);
            if (!succeeded)
            {
                return "Invalid Amount entered";
            }
            var addDelData = new AdditionDeletionGridViewHolder
                             {
                                 LocationWwd = tbWwd.Text, 
                                 CarGroup = tbCarGroup.Text,
                                 Addition = rblMoveType.SelectedIndex == 0,
                                 RepDate = repDate.Value,
                                 Value = number
                             };
            var message = AddDelBl.InsertManualAdditionDeletion(addDelData);
            RaiseBubbleEvent(this, new CommandEventArgs("RefreshData", null));
            return message;
        }

        public void ClearFields()
        {
            rblMoveType.SelectedIndex = 0;
            tbWwd.Text = string.Empty;
            tbCarGroup.Text = string.Empty;
            sdpRepDate.ClearFields();
            tbValue.Text = string.Empty;
        }
    }
}