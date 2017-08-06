using System.Web.UI.WebControls;

namespace App.UserControls.Pages
{
    public partial class PageInformation : System.Web.UI.UserControl
    {
        public Label LastUpdateLabel
        {
            get { return this.LabelLastUpdateInfo; }
        }
    }
}