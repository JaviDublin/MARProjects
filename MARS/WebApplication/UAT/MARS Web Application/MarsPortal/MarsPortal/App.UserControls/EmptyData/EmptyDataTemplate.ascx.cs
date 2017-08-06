using App.BLL;
namespace App.UserControls.EmptyData
{
    public partial class EmptyDataTemplate : System.Web.UI.UserControl
    {
        private int? _noResultGridview = null;
        public int? NoResultGridview
        {
            get { return _noResultGridview; }
            set { _noResultGridview = value; }
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (_noResultGridview != null)
            {
                string message = Gridviews.GetEmptyDataText(_noResultGridview);
                if (!(message == null))
                {
                    this.LabelGridviewNoData.Text = message;
                }
            }



        }
    }
}