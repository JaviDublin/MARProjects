using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.NonRevReWrite.Enums;
using Resources;


namespace App.UserControls.CheckBoxList
{
    public partial class PopupCheckBoxListForm : System.Web.UI.UserControl
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

        public void CheckBoxListUncheck(bool value)
        {
            var items = CheckBoxListPopUp.Items;
            foreach (ListItem s in items)
            {
                s.Selected = value;
            }

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

        public System.Web.UI.WebControls.CheckBoxList GetCheckBoxList
        {
            get { return CheckBoxListPopUp; }
        }

        public bool SetCheckBoxAll
        {
            set { CheckBoxSelectAll.Checked = value; }
        }

        public List<string> GetSelectedGroups
        {
            get
            {
                if (TextBoxPopupCheckBoxList.Text == LocalizedParameterControl.AllParameterSelection)
                {
                    return (from ListItem i in CheckBoxListPopUp.Items select i.Value).ToList();
                }
                return new List<string>(TextBoxPopupCheckBoxList.Text.Split(','));
            }
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

        public void LoadCheckBoxList(int totalSelectedItems)
        {
            if (!Page.IsPostBack)
            {
                int totalItems = this.CheckBoxListPopUp.Items.Count;

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

        public void SetCheckBoxListText(string text)
        {
            this.TextBoxPopupCheckBoxList.Text = text;
            this.TextBoxPopupCheckBoxList.ToolTip = text;
            this.CheckBoxSelectAll.Checked = false;

        }

        #endregion

        #region "Events"

        public event EventHandler CheckChanged;

        protected void CheckBoxSelectAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (CheckChanged != null)
            {
                CheckChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}