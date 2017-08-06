//=================================================================
//  File:		
//
//  Namespace:	
//
//	Classes:	
//
//  Purpose:	
//
//===================================================================
// 
//===================================================================

using System;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using App.BLL;
using Mars.App.Classes.DAL.MarsDataAccess;
using Rad.Common;
using Rad.Controls;
using Rad.Security;

namespace App.MasterPages
{
    public partial class Fao : MasterBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Add style Sheets
            //** JQuery style sheet is not to be minified on release
            base.AddStyleSheet("jquery-ui-1.8.11.css", "MJquery", false);
            base.AddStyleSheet("jquery.ui.selectmenu.css", "MJquery-Select-CSS", false);
            base.AddStyleSheet("Layout-Application.css", "MLayout", false);
            base.AddStyleSheet("Controls.css", "MControls", false);
            base.AddStyleSheet("Availability.css", "MAvailability", false);
            base.AddStyleSheet("jquery.ptTimeSelect.css", "MTimepicker", false);
            base.AddStyleSheet("Phase4Style.css", "MPhase4Style", false);

            //Add Javascript Files

            base.IncludeScripts("jquery-1.5.1.js", "MJquery", false);
            base.IncludeScripts("jquery-ui-1.8.11.custom.js", "MJquery-Custom", false);


            base.IncludeScripts("jquery.bgiframe-2.1.2.js", "MJquery-BGI", false);
            base.IncludeScripts("ui.dropdownchecklist.js", "Mui.dropdownchecklist", false);
            base.IncludeScripts("jquery.cookie.js", "MJquery-Cookie", false);
            base.IncludeScripts("jquery.ui.selectmenu.js", "mJquery-Select", false);
            base.IncludeScripts("Application.js", "MApplication", false);
            base.IncludeScripts("jquery.fixedheadertable.js", "MJquery-FixedHeader", false);
            //base.IncludeScripts("AutoComplete.js", "Custom-Control", false);
            base.IncludeScripts("jquery.ptTimeSelect.js", "MTimePicker", false);
            base.IncludeScripts("jquery.multiselect.js", "MMultiSelect", false);

        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
   





    }
}