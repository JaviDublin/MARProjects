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
using Mars.App.Classes.DAL.MarsDBContext;
using Rad.Common;
using Rad.Controls;
using Rad.Security;

namespace App.MasterPages
{
    public partial class Application : MasterBase
    {

        #region Properties

        /// <summary>
        /// Property to access the Ribbon Control
        /// </summary>
        public Rad.Controls.RibbonMenu ControlPanelRibbon
        {
            get { return this.RibbonControlPanel; }
        }


        #endregion

        #region Page Methods

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
            if(!IsPostBack)
            {
                UserActivity.LogPageAccess(ApplicationAuthentication.GetGlobalId(), Request.Url.AbsolutePath, ApplicationAuthentication.GetUserName());
            }
            //Check session timeout
            
            base.CheckSessionTimeout();

            this.ControlPanelRibbon.UserLoggedIn = @"User Logged In: " + ApplicationAuthentication.GetUserName();
            this.ControlPanelRibbon.ControlTitle = @"Application Home";

            this.ChangeMenuStyles();


        }
        #endregion

        #region Control Methods

        protected void ChangeMenuStyles()
        {

            StringBuilder startupScript = new StringBuilder();

            //Change style for third menu item

            startupScript.Append("$(function () {$(\".rad__panel-Ribbon-LinkButtonSplit-MenuItems\").css({ width: 8.2 });});");


            //Load the scripts to the page
            Scripts.RegisterStartupClientScript(this.Page, startupScript.ToString(), "menuresize_" + this.ClientID);



        }

        protected void ControlPanel_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e != null)
            {

                // Set variables to hold command and argument
                string option = string.Empty;


                // Get the command name
                switch (e.CommandName)
                {

                    //Logo Button Event
                    case "ImageLogo":
                        option = e.CommandName;


                        break;

                    //Information Button Event
                    case "InformationLink":

                        option = e.CommandName;
                        break;

                    //About Button Event
                    case "AboutLink":

                        option = e.CommandName;
                        break;

                    //FAQ Button Event
                    case "FAQLink":

                        option = e.CommandName;
                        break;

                    //Logout Button Event
                    case "LogoutLink":
                        //Sign user out
                        ApplicationAuthentication.SignOut();

                        option = e.CommandName;
                        break;

                    case "CultureChanged":

                        string culture = e.CommandArgument.ToString();

                        // Change language in PAGE Base

                        break;

                    //Application Link - CommandArgument = Navigate URL
                    case "ApplicationLink":

                        Response.Redirect(e.CommandArgument.ToString());

                        break;


                }
            }
        }

        #endregion
    }
}