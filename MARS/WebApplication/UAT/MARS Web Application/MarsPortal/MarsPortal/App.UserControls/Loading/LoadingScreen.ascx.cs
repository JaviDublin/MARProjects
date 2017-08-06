using System;

namespace App.UserControls
{
    public partial class LoadingScreen : System.Web.UI.UserControl
    {
        public bool ShowGenericPleaseWait 
        {
            set
            {
                if (value)
                    lblLoading.Text = GetGlobalResourceObject("LocalizedChartControl", "GenericLoadingScreen").ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}