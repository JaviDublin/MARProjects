using System;

namespace App.UserControls.Confirm
{
    public partial class ModalConfirm : System.Web.UI.UserControl
    {
        private string _validationGroup = "none";
        public string ValidationGroup
        {
            get { return _validationGroup; }
            set { _validationGroup = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelValidationGroup.Text = _validationGroup;
        }
    }
}