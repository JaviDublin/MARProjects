using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.App.UserControls
{
    public partial class HelpIcon : UserControl
    {

        public string HoverImage
        {
            set
            {
                imgHoverImage.ImageUrl = value;
            } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
                BindMultiSelect();
            
        }

        public void BindMultiSelect()
        {
            var javascript = new StringBuilder();

            javascript.Append(@"$(document).ready(function () {
                
                            $('.popupImage').hide();
                $('.wrap').mouseover(function () {
                    $('.popupImage').show();
                }).mouseout(function () {
                    $('.popupImage').hide();
                });

            });");

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "registerHelpJs", javascript.ToString(), true);
        }
    }
}