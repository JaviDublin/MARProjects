using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Vehicles
{
    public partial class VehiclesCheckBoxList : System.Web.UI.UserControl
    {
        #region Properties And Fields

        private string _listBoxCSSClass = "divPopUpCheckBoxList";
        private int _listBoxWidth = 95;

        private string _checkBoxSelectAllToolTip;
        public string ListBoxCSSClass
        {
            get { return _listBoxCSSClass; }
            set { _listBoxCSSClass = value; }
        }

        public int ListBoxWidth
        {
            get { return _listBoxWidth; }
            set { _listBoxWidth = value; }
        }

        public string CheckBoxSelectAllToolTip
        {
            get { return _checkBoxSelectAllToolTip; }
            set { _checkBoxSelectAllToolTip = value; }
        }

        #endregion

        #region Methods

        public void LoadCheckBoxList()
        {

            if (!Page.IsPostBack)
            {
                int totalItems = this.CheckBoxListPopUp.Items.Count;
                int totalSelectedItems = 0;
                foreach (ListItem item in this.CheckBoxListPopUp.Items)
                {
                    if (item.Selected == true)
                    {
                        totalSelectedItems += 1;
                    }
                }

                if (totalItems == 0)
                {
                    this.TextBoxPopupCheckBoxList.Text = Resources.lang.NoneSelected;
                    this.TextBoxPopupCheckBoxList.ToolTip = Resources.lang.NoneSelected;
                    this.CheckBoxSelectAll.Checked = false;

                }
                else if (totalItems >= 1)
                {
                    if (totalItems == totalSelectedItems)
                    {
                        this.TextBoxPopupCheckBoxList.Text = Resources.lang.All;
                        this.TextBoxPopupCheckBoxList.ToolTip = Resources.lang.All;
                        this.CheckBoxSelectAll.Checked = true;
                    }
                    else
                    {
                        this.TextBoxPopupCheckBoxList.Text = Resources.lang.NoneSelected;
                        this.TextBoxPopupCheckBoxList.ToolTip = Resources.lang.NoneSelected;
                        this.CheckBoxSelectAll.Checked = false;
                    }
                }
            }

            string checkBoxClientId = this.CheckBoxListPopUp.ClientID;
            string textBoxClientId = this.TextBoxPopupCheckBoxList.ClientID;


            this.PanelCheckBoxList.CssClass = _listBoxCSSClass;
            this.CheckBoxListPopUp.Width = _listBoxWidth;
            this.CheckBoxSelectAll.ToolTip = _checkBoxSelectAllToolTip;


            dynamic selectAllClientId = this.CheckBoxSelectAll.ClientID;
            string itemCount = this.CheckBoxListPopUp.Items.Count.ToString();
            this.CheckBoxSelectAll.Attributes.Add("onclick", "SetCheckBoxValues('" + checkBoxClientId + "','" + textBoxClientId + "','" + selectAllClientId + "','1','" + itemCount + "')");
            this.CheckBoxListPopUp.Attributes.Add("onclick", "SetCheckBoxValues('" + checkBoxClientId + "','" + textBoxClientId + "','" + selectAllClientId + "','2','" + itemCount + "')");

        }

        #endregion

        public event EventHandler LoadSerials;

        protected void TextBoxPopupCheckBoxList_TextChanged(object sender, EventArgs e)
        {
            SetSessionValue(this.TextBoxPopupCheckBoxList.Text);

            if (LoadSerials != null)
            {
                LoadSerials(this, EventArgs.Empty);
            }

            //this.UpdatePanelPopupCheckBoxList.Update();

        }

        protected void SetSessionValue(string text)
        {
            if (text == "None Selected")
            {
                SessionHandler.MappingSelectedModelDescription = null;
            }
            else if (text == "*** All ***")
            {
                SessionHandler.MappingSelectedModelDescription = null;
            }
            else
            {
                SessionHandler.MappingSelectedModelDescription = this.TextBoxPopupCheckBoxList.Text;
            }
        
        }
 
    }
}