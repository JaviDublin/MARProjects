using System;

namespace App.UserControls.Confirm
{
    public partial class DeleteConfirm : System.Web.UI.UserControl
    {
        private bool _LabelMessage2Visible = true;
        public bool LabelMessage2Visible
        {
            get { return _LabelMessage2Visible; }
            set { _LabelMessage2Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PanelMessage2.Visible = _LabelMessage2Visible;
        }
    }
}